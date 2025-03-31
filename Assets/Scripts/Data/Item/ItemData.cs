using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemID;
        public string displayName;
        public ECategory category;
        public GameObject prefab;
        public Sprite Icon;
    }
}