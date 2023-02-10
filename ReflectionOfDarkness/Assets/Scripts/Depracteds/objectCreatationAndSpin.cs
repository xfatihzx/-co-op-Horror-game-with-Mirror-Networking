using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Depracteds
{
    public class objectCreatationAndSpin : MonoBehaviour
    {
        [SerializeField] float spinRotateSpeed;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            spin();
        }

        private void spin()
        {
            transform.Rotate(0f, spinRotateSpeed * Time.deltaTime, 0f, Space.Self);
        }
    }
}