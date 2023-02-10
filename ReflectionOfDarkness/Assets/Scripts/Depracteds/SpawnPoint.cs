using ReflectionOfDarknes.Player.Controller;
using UnityEngine;

namespace Depracteds
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private GameObject targetLocation;
        private void OnTriggerEnter(Collider other)
        {
            if (targetLocation != null && other.gameObject.CompareTag("Player"))
            {
                if (other.TryGetComponent(out CharacterController controller))
                    if (other.TryGetComponent(out FPS fpc))
                    {
                        fpc.enabled = false;
                        controller.enabled = false;

                        other.transform.SetPositionAndRotation(
                            targetLocation.transform.position + new Vector3(0, 0, 1),
                            targetLocation.transform.rotation
                            );

                        controller.enabled = true;
                        fpc.enabled = true;
                    }
            }
        }
    }
}