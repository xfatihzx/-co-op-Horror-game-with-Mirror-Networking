using UnityEngine;
using UnityEngine.UI;

namespace Depracteds
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] [OnInspector(ReadOnly = true)] public int Count;
        [SerializeField] [OnInspector(ReadOnly = true)] public int maxCount;
        public Image UISprite;
        public Text UICount;

        public bool SetCount(int count)
        {
            if (maxCount != 0 && count == maxCount)
            {
                return false;
            }

            Count = count;
            UICount.text = count.ToString();
            return true;
        }
    }
}