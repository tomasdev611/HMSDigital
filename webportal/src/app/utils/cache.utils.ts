import {checkInternalUser} from './common.utils';

export function storeCache([myUser, enums, roles, featureRes]) {
  const rolesObj = roles.reduce((obj: any, r: any) => {
    obj[r.id] = r;
    return obj;
  }, {});
  const permissions = myUser.userRoles.flatMap((ur: any) => {
    return rolesObj[ur.roleId].permissions;
  });

  localStorage.setItem('me', JSON.stringify(myUser));
  localStorage.setItem('roles', JSON.stringify(rolesObj));
  localStorage.setItem('enum', JSON.stringify(enums));
  localStorage.setItem('userPermissions', JSON.stringify(permissions));
  localStorage.setItem('featureFlags', JSON.stringify(featureRes.records));
  const isInternalUser = checkInternalUser();
  localStorage.setItem('isInternalUser', isInternalUser.toString());
}
