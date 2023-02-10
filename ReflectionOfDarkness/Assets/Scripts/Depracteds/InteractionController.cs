using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Depracteds
{
    public class InteractionController : MonoBehaviour
    {
        [Header("Informations : ")]
        [SerializeField]
        [OnInspector(ReadOnly = true, Comment = "Oyuncu etkileþime geçebilir mi ?")]
        public bool isPlayerInteractionable;
        [SerializeField]
        [OnInspector(ReadOnly = true, Comment = "Oyuncu korktu mu ?")]
        public bool isPlayerFear;
        [SerializeField]
        [OnInspector(ReadOnly = true, Comment = "Çarpýþma var mý ?")]
        private bool isHit;
        [SerializeField]
        [OnInspector(ReadOnly = true, Comment = "Tuþa bastý mý ?")]
        private bool isRead;
        [SerializeField]
        [OnInspector(ReadOnly = true, Comment = "Obje etkileþime geçebiliyor mu ?")]
        private bool isInteractionable;
        [SerializeField]
        [OnInspector(ReadOnly = true, Comment = "Son çarpýþan obje")]
        private GameObject lasthitObject;
        [SerializeField]
        [OnInspector(ReadOnly = true, Comment = "Son basýlan tuþ")]
        private KeyCode presssingKey;
        [SerializeField]
        private RaycastHit raycastHit;

        [Header("Raycast Settings :")]
        [SerializeField] private Text pressInfo;
        [SerializeField] private float raycastRange = 5;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        [SerializeField] private LineRenderer lineRenderer;

        [Header("Inspect Settings :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool onInspect;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool isSet;
        [SerializeField] private float inspectDuration = 0.2f;
        [SerializeField] private float inspectedRotateSpeed = 125f;
        [SerializeField] private Transform inspectScreenTranform;
        [SerializeField] [OnInspector(ReadOnly = true)] private int inspectedObjectSiblingIndex;
        [SerializeField] [OnInspector(ReadOnly = true)] private GameObject inspectedObject;
        [SerializeField] [OnInspector(ReadOnly = true)] private Vector3 inspectedObjectPosition;
        [SerializeField] [OnInspector(ReadOnly = true)] private Quaternion inspectedObjectRotation;
        [SerializeField] [OnInspector(ReadOnly = true)] private Transform inspectedObjectParent;

        [Header("Debugging :")]
        [SerializeField] private Color debuggingColor = Color.red;
        [SerializeField] private Color debuggingColorOtherHit = Color.yellow;
        [SerializeField] private Color debuggingColorHit = Color.green;

        [Header("Other Settings :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private PlayerController player;

        private void Start()
        {
            player = GetComponent<PlayerController>();
            presssingKey = KeyCode.None;
            isSet = false;
            isPlayerInteractionable = true;
            isPlayerFear = false;
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        }

        private void LateUpdate()
        {
            isHit = false;
            isRead = false;
            isInteractionable = false;
            RayCast();
            OnInspect();
        }

        private void RayCast()
        {
            if (Debug.isDebugBuild)
            {
                foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(kcode))
                        presssingKey = kcode;
                }
            }

            if (!onInspect && !isRead)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, raycastRange))
                {
                    isHit = true;

                    lasthitObject = raycastHit.collider.gameObject;

                    if (isPlayerInteractionable && lasthitObject.HasTagAny(EntityType.InteractionalObject))
                    {
                        isInteractionable = true;

                        if ((!isPlayerFear || (isPlayerFear && player.PlayerInteractPossibilityForFear())) && Input.GetKeyDown(interactionKey))
                        {
                            isRead = true;

                            if (lasthitObject.HasTagAny(EntityType.InspectObject))
                            {
                                onInspect = true;
                                isSet = false;
                            }
                            else if (lasthitObject.HasTagAny(EntityType.CollectibleObject, EntityType.Holdableobject))
                            {
                                if (lasthitObject.HasComponent(out CollectibleObject collectibleObject))
                                {
                                    GetComponent<InventoryController>().AddObject(collectibleObject);
                                }
                            }
                            else if (lasthitObject.HasComponent(out Entity objectInteraction))
                            {
                                objectInteraction.Action();
                            }

                            return;
                        }
                    }
                }
            }

            RayCastHitting();

            RaycastPressInfo(isInteractionable, $@"Press [{interactionKey}]");
        }

        private void OnInspect()
        {
            if (onInspect && isSet == false)
            {
                isSet = true;

                inspectedObject = lasthitObject;

                inspectedObjectPosition = lasthitObject.transform.position;
                inspectedObjectRotation = lasthitObject.transform.rotation;

                inspectedObjectSiblingIndex = inspectedObject.transform.GetSiblingIndex();

                inspectedObjectParent = lasthitObject.transform.parent;

                StartCoroutine(InspectPickup());
            }

            if (onInspect)
            {
                inspectedObject.transform.position = Vector3.Lerp(inspectedObject.transform.position, inspectScreenTranform.position, inspectDuration);
                inspectedObject.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * inspectedRotateSpeed);
            }
            else if (inspectedObject != null)
            {
                inspectedObject.transform.SetParent(inspectedObjectParent);

                inspectedObject.transform.SetSiblingIndex(inspectedObjectSiblingIndex);

                inspectedObject.transform.rotation = Quaternion.Lerp(inspectedObject.transform.rotation, inspectedObjectRotation, inspectDuration);

                inspectedObject.transform.position = Vector3.Lerp(inspectedObject.transform.position, inspectedObjectPosition, inspectDuration);
            }

            if (Input.GetKeyDown(interactionKey) && onInspect && !isRead)
            {
                StartCoroutine(InspectDrop());
                isRead = true;
                onInspect = false;
            }
        }

        private IEnumerator InspectPickup()
        {
            FindObjectOfType<PlayerController>().canMove = false;

            yield return new WaitForSeconds(inspectDuration);

            inspectedObject.transform.SetParent(inspectScreenTranform);
        }

        private IEnumerator InspectDrop()
        {
            yield return new WaitForSeconds(inspectDuration);

            FindObjectOfType<PlayerController>().canMove = true;
        }

        private void RayCastHitting()
        {
            #region Raycast draw area (if debugging)
            if (Debug.isDebugBuild)
            {
                Color selectedColor = isInteractionable ? debuggingColorHit : (isHit ? debuggingColorOtherHit : debuggingColor);

                Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward) * raycastRange;

                Debug.DrawRay(Camera.main.transform.position, forward, selectedColor);

                lineRenderer.startColor = selectedColor;
                lineRenderer.endColor = selectedColor;
                lineRenderer.material.color = selectedColor;

                lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, -0.1f, 0), new Vector3(0, 0, raycastRange / 2) });
            }
            #endregion
        }

        private void RaycastPressInfo(bool hover, string text = null)
        {
            #region Show Info Press Key
            pressInfo.text = text;
            pressInfo.gameObject.SetActive(hover);
            #endregion
        }
    }
}