using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HMSDigital.Core.Data.Models;

namespace HMSDigital.Core.BusinessLayer.Sieve
{
    public class SieveCustomFilters : ISieveCustomFilterMethods
    {
        public IQueryable<HospiceMember> HospiceLocationId(IQueryable<HospiceMember> source, string op, string[] values)
        {
            var result = source.Where(p => p.HospiceLocationMembers.Count() != 0 && p.HospiceLocationMembers.Any(l => values.Contains(l.HospiceLocationId.ToString())));

            return result;
        }

        public IQueryable<Items> CategoryId(IQueryable<Items> source, string op, string[] values)
        {
            var result = source.Where(i => i.ItemCategoryMapping.Count() != 0 && i.ItemCategoryMapping.Any(c => values.Contains(c.ItemCategoryId.ToString())));

            return result;
        }

        public IQueryable<Items> EquipmentSettingsExist(IQueryable<Items> source, string op, string[] values)
        {
            var result = source.Where(i => i.EquipmentSettingsConfig.Count() != 0);

            return result;
        }

        public IQueryable<Items> AddOnsExist(IQueryable<Items> source, string op, string[] values)
        {
            var result = source.Where(i => i.AddOnGroups.Count() != 0);

            return result;
        }

        public IQueryable<Users> RoleId(IQueryable<Users> source, string op, string[] values)
        {
            var result = source.Where(u => u.UserRoles.Count() != 0 && u.UserRoles.Any(us => us.RoleId.HasValue && values.Contains(us.RoleId.ToString())));

            return result;
        }

        public IQueryable<OrderHeaders> CompletedOrdersWithException(IQueryable<OrderHeaders> source, string op, string[] values)
        {
            var result = source.Where(o => o.StatusId == (int)Data.Enums.OrderHeaderStatusTypes.Cancelled
                                        || o.StatusId == (int)Data.Enums.OrderHeaderStatusTypes.Completed
                                        || o.IsExceptionFulfillment);

            return result;
        }

        public IQueryable<ContractRecords> CategoryId(IQueryable<ContractRecords> source, string op, string[] values)
        {
            var result = source.Where(i => i.Item.ItemCategoryMapping.Count() != 0 && i.Item.ItemCategoryMapping.Any(c => values.Contains(c.ItemCategoryId.ToString())));

            return result;
        }

        public IQueryable<ContractRecords> SubCategoryId(IQueryable<ContractRecords> source, string op, string[] values)
        {
            var result = source.Where(i => i.Item.ItemSubCategoryMapping.Count() != 0 && i.Item.ItemSubCategoryMapping.Any(c => values.Contains(c.ItemSubCategoryId.ToString())));

            return result;
        }

        public IQueryable<Hms2ContractItems> CategoryId(IQueryable<Hms2ContractItems> source, string op, string[] values)
        {
            var result = source.Where(i => i.Item.ItemCategoryMapping.Count() != 0 && i.Item.ItemCategoryMapping.Any(c => values.Contains(c.ItemCategoryId.ToString())));

            return result;
        }

        public IQueryable<Hms2ContractItems> SubCategoryId(IQueryable<Hms2ContractItems> source, string op, string[] values)
        {
            var result = source.Where(i => i.Item.ItemSubCategoryMapping.Count() != 0 && i.Item.ItemSubCategoryMapping.Any(c => values.Contains(c.ItemSubCategoryId.ToString())));

            return result;
        }
    }
}
