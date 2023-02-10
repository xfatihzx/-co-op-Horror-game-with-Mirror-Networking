using System.Collections;
using UnityEngine;

namespace Depracteds
{
    public class DoorAction : MonoBehaviour
    {
        [Header("General Options :")]
        [Space(5)]
        [SerializeField] private float doorRotationSpeed = 3;
        [SerializeField] private float startRotation = 0;
        [SerializeField] private GameObject hinge;
        [SerializeField] [OnInspector(ReadOnly = true)] private float selectedRotation;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool isOpen;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool isOpenning;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool isUsedKey;

        [Header("Key Options :")]
        [Space(25)]
        [SerializeField] private bool isLockedDoor = false;
        [SerializeField] [OnInspector(Comment = @"Anahtar olan objenin guid'ini buraya yapýþtýr. isLockedDoor deðeri true olduðunda iþe yarar")] private string keyId;

        [Header("Front Options :")]
        [Space(25)]
        [SerializeField] private float rotationFront = 90;
        [SerializeField] private Entity entityFront;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool onTheAreaFront;

        [Header("Back Options :")]
        [Space(25)]
        [SerializeField] private float rotationBack = -90;
        [SerializeField] private Entity entityBack;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool onTheAreaBack;

        private void Start()
        {
            entityFront.AsOnTriggerEnter += (Collider collider) =>
            {
                if (collider.gameObject.tag == Tags.Player)
                {
                    onTheAreaFront = true;
                }
            };
            entityFront.AsOnTriggerExit += (Collider collider) =>
            {
                if (collider.gameObject.tag == Tags.Player)
                {
                    onTheAreaFront = false;
                }
            };
            entityBack.AsOnTriggerEnter += (Collider collider) =>
            {
                if (collider.gameObject.tag == Tags.Player)
                {
                    onTheAreaBack = true;
                }
            };
            entityBack.AsOnTriggerExit += (Collider collider) =>
            {
                if (collider.gameObject.tag == Tags.Player)
                {
                    onTheAreaBack = false;
                }
            };
        }
        public void DoorTrigger()
        {
            if (FindObjectOfType<InventoryController>().HasObject(EntityType.CollectibleObject, keyId))
            {
                if (isLockedDoor)
                {
                    isUsedKey = true;

                    FindObjectOfType<InventoryController>().RemoveObject(EntityType.CollectibleObject, keyId);
                }
                else
                {
                    if (!isOpenning) WaitDoorRotation();
                    isOpenning = true;
                }
            }

            if (isLockedDoor && isUsedKey)
            {
                if (!isOpenning) WaitDoorRotation();
                isOpenning = true;
            }

            if (!isOpenning)
            {
                return;
            }

            if (onTheAreaFront && !isOpen)
            {
                isOpen = true;
                selectedRotation = rotationFront;
            }
            else if (onTheAreaBack && !isOpen)
            {
                isOpen = true;
                selectedRotation = rotationBack;
            }
            else
            {
                isOpen = false;
                selectedRotation = startRotation;
            }
        }

        private void FixedUpdate()
        {
            if (isOpenning)
            {
                hinge.transform.localRotation = Quaternion.Slerp(hinge.transform.localRotation, Quaternion.Euler(0, selectedRotation, 0), doorRotationSpeed * Time.deltaTime);
            }
        }

        private IEnumerator WaitDoorRotation()
        {
            yield return new WaitForSeconds(doorRotationSpeed);
            isOpenning = false;
        }
    }
}