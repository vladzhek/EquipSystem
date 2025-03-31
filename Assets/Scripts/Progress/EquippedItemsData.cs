using System;
using System.Collections.Generic;

namespace Data.Progress
{
    [Serializable]
    public class EquippedItemsData
    {
        public List<ECategory> categories = new();
        public List<string> itemIDs = new();

        public string GetItem(ECategory category)
        {
            var index = categories.IndexOf(category);
            return index >= 0 ? itemIDs[index] : null;
        }

        public void SetItem(ECategory category, string itemID)
        {
            var index = categories.IndexOf(category);
            if (index >= 0)
            {
                itemIDs[index] = itemID;
            }
            else
            {
                categories.Add(category);
                itemIDs.Add(itemID);
            }
        }
    }
}