using System;
using Services;
using UnityEngine;
using View.UI;

namespace View
{
    public class EquipView : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private ItemUI _itemUi;

        private void Start()
        {
            var items = GameController.Services.Get<DataService>().ItemsData;
            foreach (var item in items)
            {
                var prefabUI = Instantiate(_itemUi, _parent);
                prefabUI.Init(item);
            }
        }
    }
}