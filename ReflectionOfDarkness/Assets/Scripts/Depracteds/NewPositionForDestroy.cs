using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Depracteds
{
public class NewPositionForDestroy : MonoBehaviour
{
    [SerializeField] Transform newPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            other.transform.position = newPosition.position;
            other.transform.rotation = newPosition.rotation;
        }
    }
}
}
