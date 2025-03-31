using System;
using System.Collections.Generic;

namespace Data.Progress
{
    [Serializable]
    public class PlayerProgress
    {
        public EquippedItemsData EquippedItems = new();

        public PlayerProgress()
        {
            EquippedItems.SetItem(ECategory.Top, null);
            EquippedItems.SetItem(ECategory.Bottom, null);
            EquippedItems.SetItem(ECategory.Hair, null);
        }
        
        public void MarkAsEquipped(ECategory category, string itemID)
        {
            EquippedItems.SetItem(category, itemID);
        }
    }
}