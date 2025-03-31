using System.Collections.Generic;
using Data;
using Infrastructure;
using UnityEngine;

namespace Services
{
    public class DataService : IGameService
    {
        public List<ItemData> ItemsData = new();

        public void Initialize()
        {
            LoadItems();
        }

        private void LoadItems()
        {
            var data = Resources.LoadAll<ItemData>("Configs/Items");
            foreach (var item in data)
            {
                ItemsData.Add(item);
            }
        }

        public void Cleanup()
        {
 
        }
    }
}