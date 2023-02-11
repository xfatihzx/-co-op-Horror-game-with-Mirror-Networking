using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Depracteds
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject uiPanelInventoryObjects;
        [SerializeField] private GameObject prefabPanelInventoryObject;
        [SerializeField] [OnInspector(ReadOnly = true)] public Dictionary<string, InventoryItem> collectibleObjects;
        [SerializeField] [OnInspector(ReadOnly = true)] public Dictionary<string, InventoryItem> holdableObjects;

        private void Awake()
        {
            collectibleObjects = new Dictionary<string, InventoryItem>();
            holdableObjects = new Dictionary<string, InventoryItem>();
        }

        public bool AddObject(CollectibleObject collectibleObject)
        {
            if (collectibleObject != null && collectibleObject.Entity().isUsableObject && collectibleObject.Entity().isSet)
            {
                if (collectibleObject.gameObject.HasTag(EntityType.CollectibleObject))
                {
                    return Increment(SelectDictionary(EntityType.CollectibleObject), collectibleObject);
                }
                else if (collectibleObject.gameObject.HasTag(EntityType.Holdableobject))
                {
                    return Increment(SelectDictionary(EntityType.Holdableobject), collectibleObject);
                }
            }
            return false;
        }

        public void RemoveObject(EntityType objectInteractionTag, string guid)
        {
            if (objectInteractionTag == EntityType.CollectibleObject)
            {
                Decrement(SelectDictionary(EntityType.CollectibleObject), guid);
            }
            else if (objectInteractionTag == EntityType.Holdableobject)
            {
                Decrement(SelectDictionary(EntityType.Holdableobject), guid);
            }
        }

        public bool HasObject(EntityType objectInteractionTag, string guid)
        {
            bool has = false;

            if (objectInteractionTag == EntityType.CollectibleObject)
            {
                has = collectibleObjects.ContainsKey(guid);
            }
            else if (objectInteractionTag == EntityType.Holdableobject)
            {
                has = holdableObjects.ContainsKey(guid);
            }

            return has;
        }

        private Dictionary<string, InventoryItem> SelectDictionary(EntityType objectInteractionTag)
        {
            Dictionary<string, InventoryItem> dictionary = null;

            if (objectInteractionTag == EntityType.CollectibleObject)
            {
                dictionary = collectibleObjects;
            }
            else if (objectInteractionTag == EntityType.Holdableobject)
            {
                dictionary = holdableObjects;
            }

            return dictionary;
        }

        private bool Increment(Dictionary<string, InventoryItem> dictionary, CollectibleObject collectibleObject)
        {
            string guid = dictionary.Keys.FirstOrDefault(guid => guid == collectibleObject.Entity().id);

            if (guid is null)
            {
                guid = collectibleObject.Entity().id;

                dictionary.Add(guid, InventoryItemCreate(dictionary.Count, collectibleObject));
            }

            bool isIncrement = dictionary[guid].SetCount(dictionary[guid].Count + 1);

            if (isIncrement) Destroy(collectibleObject.gameObject);

            return isIncrement;
        }

        private void Decrement(Dictionary<string, InventoryItem> dictionary, string guid)
        {
            dictionary[guid].SetCount(dictionary[guid].Count - 1);

            if (dictionary[guid].Count < 1)
            {
                Destroy(dictionary[guid].gameObject);

                dictionary.Remove(guid);

                for (int i = 0; i < uiPanelInventoryObjects.transform.childCount; i++)
                {
                    Transform child = uiPanelInventoryObjects.transform.GetChild(i);

                    child.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(90 * i, 0, 0);
                }
            }
        }

        private InventoryItem InventoryItemCreate(int index, CollectibleObject collectibleObject)
        {
            GameObject newInstantiate = Instantiate(prefabPanelInventoryObject);

            newInstantiate.transform.SetParent(uiPanelInventoryObjects.transform);

            RectTransform cRectTransform = newInstantiate.GetComponent<RectTransform>();

            cRectTransform.localScale = new Vector3(1, 1, 1);

            cRectTransform.pivot = new Vector2(0, 0);

            cRectTransform.anchorMin = new Vector2(0, 0);

            cRectTransform.anchorMax = new Vector2(0, 0);

            cRectTransform.anchoredPosition3D = new Vector3(90 * index, 0, 0);

            cRectTransform.sizeDelta = new Vector2(90, 90);

            InventoryItem InventoryItem = newInstantiate.GetComponent<InventoryItem>();

            InventoryItem.Entity().SetGuid();

            InventoryItem.Entity().id = collectibleObject.Entity().id;

            InventoryItem.Count = 0;

            InventoryItem.UISprite.sprite = collectibleObject.sprite;

            return InventoryItem;
        }
    }
}