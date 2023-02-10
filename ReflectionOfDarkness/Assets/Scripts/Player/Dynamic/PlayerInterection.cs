using Depracteds;
using ReflectionOfDarknes.Player.Controller;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfDarknes.Player.Dynamic
{
    public class PlayerInterection : MonoBehaviour
    {
        [Header("Informations : ")]
        [Tooltip("Oyuncu etkileþime geçebilir mi ?")]
        public bool isPlayerInteractionable;
        [Tooltip("Oyuncu korktu mu ?")]
        public bool isPlayerFear;
        [SerializeField]
        [Tooltip("Çarpýþma var mý ?")]
        private bool isHit;
        [SerializeField]
        [Tooltip("Tuþa bastý mý ?")]
        private bool isRead;
        [SerializeField]
        [Tooltip("Obje etkileþime geçebiliyor mu ?")]
        private bool isInteractionable;
        [SerializeField]
        [Tooltip("Son çarpýþan obje")]
        private GameObject lasthitObject;
        [SerializeField]
        [Tooltip("Son basýlan tuþ")]
        private KeyCode presssingKey;
        [SerializeField]
        private RaycastHit raycastHit;

        [Header("Raycast Settings :")]
        [SerializeField]
        private Text pressInfo;
        [SerializeField]
        private float raycastRange = 5;
        [SerializeField]
        private KeyCode interactionKey = KeyCode.E;
        [SerializeField]
        private LineRenderer lineRenderer;

        [Header("Inspect Settings :")]
        [SerializeField]
        private bool onInspect;
        [SerializeField]
        private bool isSet;
        [SerializeField]
        private float inspectDuration = 0.2f;
        [SerializeField]
        private float inspectedRotateSpeed = 125f;
        [SerializeField]
        private Transform inspectScreenTranform;
        [SerializeField]
        private int inspectedObjectSiblingIndex;
        [SerializeField]
        private GameObject inspectedObject;
        [SerializeField]
        private Vector3 inspectedObjectPosition;
        [SerializeField]
        private Quaternion inspectedObjectRotation;
        [SerializeField]
        private Transform inspectedObjectParent;

        [Header("Debugging :")]
        [SerializeField]
        private Color debuggingColor = Color.red;
        [SerializeField]
        private Color debuggingColorOtherHit = Color.yellow;
        [SerializeField]
        private Color debuggingColorHit = Color.green;

        [Header("Other Settings :")]
        [SerializeField]
        private FPS player;

        private void Start()
        {
            player = GetComponent<FPS>();
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

                    if (isPlayerInteractionable && lasthitObject.TryGetComponent(out InterectAction interect))
                    {
                        isInteractionable = true;

                        if (Input.GetKeyDown(interactionKey))
                        {
                            isRead = true;

                            interect.Action();

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
            player.enableMove = false;

            yield return new WaitForSeconds(inspectDuration);

            inspectedObject.transform.SetParent(inspectScreenTranform);
        }

        private IEnumerator InspectDrop()
        {
            yield return new WaitForSeconds(inspectDuration);

            player.enableMove = true;
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