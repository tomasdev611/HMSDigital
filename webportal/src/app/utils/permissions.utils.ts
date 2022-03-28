export function IsPermissionAssigned(permissionName: string, attribute: string) {
  const permissions = JSON.parse(localStorage.getItem('userPermissions'));
  const permissionToFind = `${permissionName}:${attribute}`;
  return permissions?.filter(p => p === permissionToFind).length !== 0;
}
