
namespace HMS2SDK.Data.Models
{
    public partial class MailingListEmails
    {
        public int Id { get; set; }
        public int MailingListId { get; set; }
        public string Email { get; set; }

        public virtual MailingLists MailingList { get; set; }
    }
}
