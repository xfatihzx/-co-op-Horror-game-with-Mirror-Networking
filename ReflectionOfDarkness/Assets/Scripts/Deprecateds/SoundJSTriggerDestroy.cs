using UnityEngine;

namespace Depracteds
{
    public class SoundJSTriggerDestroy : MonoBehaviour
    {
        [SerializeField] [OnInspector(ReadOnly = true)] private AudioSource source;
        [SerializeField] private AudioClip soundJumpScare;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool isTriggerTrue;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool playSource;

        void Start()
        {
            source = GetComponent<AudioSource>();
            source.clip = soundJumpScare;
        }

        void Update()
        {
            DestroyTrigger();
        }

        private void targetTriggerEnter(Collider enter)
        {
            if (enter.gameObject.tag == ("Player"))
            {
                isTriggerTrue = true;
            }
        }

        private void DestroyTrigger()
        {
            if (isTriggerTrue == true && playSource == false)
            {
                playSource = true;

                Debug.Log("sound if e girdi.");
                source.Play();
                // Debug.Log("sound is playing now");
                //Destroy(target.gameObject);
            }
        }
    }
}
