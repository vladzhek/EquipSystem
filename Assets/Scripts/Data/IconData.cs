using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "IconData", menuName = "Data/IconData")]
    public class IconData : ScriptableObject
    {
        public List<IconStruct> IconsList;
    }
}