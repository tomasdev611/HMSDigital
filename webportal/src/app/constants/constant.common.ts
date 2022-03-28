export const hms = {
  defaultRoute: '/dashboard',
  orderRoutes: {
    editPatient: '/patients/edit',
    addNurse: {
      pre: '/hospice',
      post: '/members/add',
    },
  },
};
