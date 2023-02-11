using System.Collections;
using UnityEngine;

namespace Depracteds
{
public class JumpScare : MonoBehaviour
{
    [Header("trigger settings :")]
    [SerializeField] private GameObject TriggerImage;
    [SerializeField] private AudioSource AudioSource;

    private bool isScare;

    void Start()
    {
        TriggerImage.SetActive(false);
        isScare = false;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isScare == false)
        {
            isScare = true;
            AudioSource.Play();
            TriggerImage.SetActive(true);
            StartCoroutine(JumpScare0());
        }
    }

    IEnumerator JumpScare0()
    {
        yield return new WaitForSeconds(3);
        isScare = false;
        TriggerImage.SetActive(false);
    }
}
}
