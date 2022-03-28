using System.Collections.Generic;

namespace HMS2SDK.Data.Models
{
    public partial class MailingLists
    {
        public MailingLists()
        {
            MailingListEmails = new HashSet<MailingListEmails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Constant { get; set; }

        public virtual ICollection<MailingListEmails> MailingListEmails { get; set; }
    }
}
