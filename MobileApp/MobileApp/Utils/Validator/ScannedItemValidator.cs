using MobileApp.Exceptions;
using MobileApp.Models;

namespace MobileApp.Utils.Validator
{
    public class ScannedItemValidator
    {
        public static bool ValidateScannedItemWithId(int itemId, Inventory scannedItem)
        {
            if (scannedItem.ItemId != itemId)
            {
                throw new ValidationException($"Scanned item ({scannedItem.Item.Name}) does not match with required item");
            }
            return true;
        }
    }
}
