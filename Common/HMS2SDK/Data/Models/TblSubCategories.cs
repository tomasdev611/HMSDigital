
namespace HMS2SDK.Data.Models
{
    public partial class TblSubCategories
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductLimit { get; set; }
        public int? RequiredSubCategoryId1 { get; set; }
        public int? RequiredSubCategoryId2 { get; set; }
        public int? RequiredSubCategoryId3 { get; set; }
        public int? SubCatRank { get; set; }
    }
}
