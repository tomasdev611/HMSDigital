using System;
namespace MobileApp.Models
{
    public class SiteMember : User
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public Site Site { get; set; }
    }
}
