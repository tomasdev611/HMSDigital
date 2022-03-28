using Sieve.Services;
using System;
using System.Linq;
using HMSDigital.Core.Data.Models;

namespace HMSDigital.Core.BusinessLayer.Sieve
{
    public class SieveCustomSorts : ISieveCustomSortMethods
    {
        public IQueryable<Sites> Driver(IQueryable<Sites> vehicles, bool useThenBy, bool desc)
        {
            var result = desc ? vehicles.OrderByDescending(v => v.DriversCurrentVehicle.User.FirstName) :
                                vehicles.OrderBy(v => v.DriversCurrentVehicle.User.FirstName);

            return result;
        }

        public IQueryable<Roles> PermissionsLength(IQueryable<Roles> roles, bool useThenBy, bool desc)
        {
            var result = desc ? roles.OrderByDescending(r => r.RolePermissions.Count()) :
                                roles.OrderBy(r => r.RolePermissions.Count());

            return result;
        }
    }
}
