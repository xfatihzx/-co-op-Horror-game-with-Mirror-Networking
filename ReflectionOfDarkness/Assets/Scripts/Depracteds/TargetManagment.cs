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


        private bool isVisible(Camera camera, GameObject target) // görme olaylarý için bir camera ve bir obje atanýr. Reel cam ile algýlama yapýldýðý için enemy'e cam vermemiz gerekmekte.
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
            var targetRender = target.GetComponent<Renderer>();     //obje göründüðünde kýrmýzý renk atanýr. Görünmediðinde yeþil renk atanýr.

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
