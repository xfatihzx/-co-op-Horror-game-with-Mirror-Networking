using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReflectionOfDarknes.General
{
    public class SetRandomColor : MonoBehaviour
    {
        void Start()
        {
            if (GetComponent<Renderer>() is Renderer renderer && renderer != null)
                renderer.material.color = Random.ColorHSV();
        }

        void Update()
        {

        }
    }
}