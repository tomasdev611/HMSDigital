import {Menus, hms} from 'src/app/constants';

export function getAccessibleMenus() {
  const permissions = JSON.parse(localStorage.getItem('userPermissions'));
  let menus = [];
  if (permissions) {
    menus = Menus.filter(menu => {
      const isMenuAllowed = menu.access.reduce((isAllowed: boolean, entity: any) => {
        const p = permissions.find((x: any) => x === `${entity}:Read`);
        return isAllowed && p;
      }, true);
      if (isMenuAllowed) {
        return menu;
      }
    });
  }
  return menus;
}

export function getDefaultRoute(path) {
  if (location.pathname && location.pathname === '/login') {
    const menus = getAccessibleMenus();
    path = menus && menus.length > 0 ? menus[0].routerLink : hms.defaultRoute;
  }
  return path;
}
