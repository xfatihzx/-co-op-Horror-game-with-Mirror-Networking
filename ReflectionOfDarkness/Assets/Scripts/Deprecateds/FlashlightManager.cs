using UnityEngine;

namespace Depracteds
{
    public class FlashlightManager : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool is_enabled;
        [SerializeField] private GameObject lights;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem particleDust;
        [SerializeField] private Light spotlight;
        [SerializeField] private Material ambient_light_material;
        [SerializeField] [OnInspector(ReadOnly = true)] private Color ambient_mat_color;

        void Start()
        {
            //Change_Intensivity(60);
        }

        public void Change_Intensivity(float percentage)
        {
            percentage = Mathf.Clamp(percentage, 0, 100);


            spotlight.intensity = (8 * percentage) / 100;

            ambient_light_material.SetColor("_TintColor", new Color(ambient_mat_color.r, ambient_mat_color.g, ambient_mat_color.b, percentage / 2000));
        }

        public void Switch(bool IsActive)
        {
            is_enabled = IsActive;

            audioSource.Play();

            spotlight.enabled = IsActive;

            if (is_enabled)
            {
                particleDust.gameObject.SetActive(true);
                particleDust.Play();
            }
            else
            {
                particleDust.Stop();
                particleDust.gameObject.SetActive(false);
            }
        }
    }
}