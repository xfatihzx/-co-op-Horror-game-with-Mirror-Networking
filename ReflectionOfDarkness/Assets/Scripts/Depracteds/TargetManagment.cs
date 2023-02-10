using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Depracteds
{
    public class TargetManagment : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private Camera cam;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool isVisibleTarget;


        private bool isVisible(Camera camera, GameObject target) // g�rme olaylar� i�in bir camera ve bir obje atan�r. Reel cam ile alg�lama yap�ld��� i�in enemy'e cam vermemiz gerekmekte.
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            var point = target.transform.position;

            foreach (var plane in planes)
            {
                if (plane.GetDistanceToPoint(point) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void Update()
        {
            targetRender();
        }

        private void targetRender()
        {
            var targetRender = target.GetComponent<Renderer>();     //obje g�r�nd���nde k�rm�z� renk atan�r. G�r�nmedi�inde ye�il renk atan�r.

            if (isVisible(cam, target))
            {
                targetRender.material.SetColor("_Color", Color.red);
                isVisibleTarget = true;
            }
            else
            {
                targetRender.material.SetColor("_Color", Color.green);
                isVisibleTarget = false;
            }
        }
    }
}
