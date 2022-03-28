
namespace HMS2SDK.Data.Models
{
    public partial class TblFavorites
    {
        public int FavsId { get; set; }
        public int? UserId { get; set; }
        public string FavoriteArray { get; set; }
        public string CategoryArray { get; set; }
    }
}
