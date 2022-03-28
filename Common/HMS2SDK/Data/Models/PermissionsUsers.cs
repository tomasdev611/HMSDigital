
namespace HMS2SDK.Data.Models
{
    public partial class PermissionsUsers
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PermissionType { get; set; }
        public string PermissionValue { get; set; }
    }
}
