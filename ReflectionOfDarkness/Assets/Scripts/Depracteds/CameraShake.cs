using UnityEngine;

namespace Depracteds
{
    public class CameraShake : MonoBehaviour
    {

        [SerializeField] private float progress;
        [SerializeField] private int passedStep = 1;
        [SerializeField] private float defCamPos = 0;
        [SerializeField] private float defCamRot = 0;
        [SerializeField] private Transform myTransform;
        [SerializeField] private float targetTime = 0.2f;
        [SerializeField] private float smooth = 10;
        [SerializeField] private float amplitudeHeight = 0.1f;
        [SerializeField] private float amplitudeRot = 1.5f;


        void Start()
        {
            myTransform = transform;
            defCamPos = myTransform.localPosition.y;
            defCamRot = myTransform.localEulerAngles.z;
        }


        void Update()
        {
            if (FindObjectOfType<PlayerController>().isJumping == false)
            {
                float Pssd = Passed();

                Vector3 GoalPos = new Vector3(myTransform.localPosition.x, Pssd * amplitudeHeight + defCamPos, myTransform.localPosition.z);

                myTransform.localPosition = Vector3.Lerp(myTransform.localPosition, GoalPos, Time.deltaTime * smooth);

                if (Mathf.Abs(Input.GetAxis("Horizontal")) == 1 && Mathf.Abs(Input.GetAxis("Vertical")) == 0)
                {
                    Pssd = 0;
                }

                Vector3 GoalRot = new Vector3(myTransform.localPosition.x, myTransform.localPosition.y, Pssd * amplitudeRot + defCamRot);

                myTransform.localEulerAngles = Vector3.Lerp(myTransform.localPosition, GoalRot, Time.deltaTime * smooth);
            }
        }


        private float Passed()
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0 && Mathf.Abs(Input.GetAxis("Vertical")) == 0)
            {
                passedStep = 1;

                return (progress = 0);
            }

            progress += (Time.deltaTime * (1f / targetTime)) * passedStep;

            if (Mathf.Abs(progress) >= 1)
            {
                passedStep *= -1;
            }

            return progress;
        }
    }
}