export const Menus = [
  {
    label: 'Dashboard',
    icon: 'pi pi-home',
    routerLink: '/dashboard',
    access: ['Orders'],
  },
  {
    label: 'Patients',
    icon: 'pi pi-user',
    routerLink: '/patients',
    access: ['Patient'],
  },
  {
    label: 'Metrics',
    icon: 'pi pi-chart-bar',
    routerLink: '/report',
    access: ['Metrics'],
  },
  {
    label: 'Hospice',
    icon: 'pi pi-microsoft',
    routerLink: '/hospice',
    access: ['Hospice'],
  },
  {
    label: 'Sites',
    icon: 'pi pi-sitemap',
    routerLink: '/sites',
    access: ['Site'],
  },
  {
    label: 'Vehicles',
    icon: 'pi pi-ticket',
    routerLink: '/vehicles',
    access: ['Vehicle'],
  },
  {
    label: 'Drivers',
    icon: 'pi pi-fw pi-users',
    routerLink: '/drivers',
    access: ['Driver'],
  },
  {
    label: 'Inventory',
    icon: 'pi pi-folder',
    routerLink: '/inventory',
    access: ['Inventory'],
  },
  {
    label: 'Users',
    icon: 'pi pi-fw pi-users',
    routerLink: '/users',
    access: ['User'],
  },
  {
    label: 'Finance',
    icon: 'pi pi-dollar',
    routerLink: '/finance',
    access: ['Finance'],
  },
  {
    label: 'System',
    icon: 'pi pi-cog',
    routerLink: '/system',
    access: ['System'],
  },
];
