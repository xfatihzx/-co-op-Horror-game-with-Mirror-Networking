using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Depracteds
{
    public class SoundJumpScare : MonoBehaviour
    {
        [Header("Audio jumpscare settings :")]
        [SerializeField] [OnInspector(ReadOnly = true)] public bool isTriggerTrue;



        private void Start()
        {

        }
        private void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == ("Player"))
            {
                isTriggerTrue = true;
            }
        }



        // ses triggerde her geçildiðinde çalýyor. Düzeltme gerekli;
    }
}
