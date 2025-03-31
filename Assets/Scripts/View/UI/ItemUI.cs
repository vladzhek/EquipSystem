using System;
using Data;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Button _equipBtn;
        [SerializeField] private Button _unEquipBtn;

        private ItemData _data;
        
        private void OnEnable()
        {
            _equipBtn.onClick.AddListener(Equip);
            _unEquipBtn.onClick.AddListener(UnEquip);
            GameController.Services.Get<ItemsPoolService>().OnEquip += UpdateUI;
            GameController.Services.Get<ItemsPoolService>().OnUnEquip += UpdateUI;
        }

        private void OnDisable()
        {
            _equipBtn.onClick.RemoveListener(Equip);
            _unEquipBtn.onClick.RemoveListener(UnEquip);
            GameController.Services.Get<ItemsPoolService>().OnEquip -= UpdateUI;
            GameController.Services.Get<ItemsPoolService>().OnUnEquip -= UpdateUI;
        }

        public void Init(ItemData data)
        {
            _data = data;
            _image.sprite = _data.Icon;
            _titleText.text = _data.displayName;
            UpdateUI(_data.itemID);
        }

        private void Equip()
        {
            GameController.Services.Get<ItemsPoolService>().EquipItem(_data.itemID);
            var progress = GameController.Services.Get<ProgressService>().PlayerProgress;
            progress.MarkAsEquipped(_data.category, _data.itemID);
            GameController.Services.Get<SaveLoadService>().SaveProgress();
        }
        
        private void UnEquip()
        {
            GameController.Services.Get<ItemsPoolService>().UnequipCategory(_data.category);
            var progress = GameController.Services.Get<ProgressService>().PlayerProgress;
            progress.MarkAsEquipped(_data.category, null);
            GameController.Services.Get<SaveLoadService>().SaveProgress();
        }

        private void UpdateUI(string ID)
        {
            if(ID != _data.itemID) return;
            
            var isEquipped = GameController.Services.Get<ItemsPoolService>().IsItemEquipped(_data.itemID);
            _equipBtn.gameObject.SetActive(!isEquipped);
            _unEquipBtn.gameObject.SetActive(isEquipped);
        }
    }
}