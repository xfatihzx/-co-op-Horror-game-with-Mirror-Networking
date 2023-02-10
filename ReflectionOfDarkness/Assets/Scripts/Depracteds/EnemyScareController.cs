using UnityEngine;

namespace Depracteds
{
    public class EnemyScareController : MonoBehaviour
    {

        [Header("Playerin korku parametreleri, Player class'ýna taþýnacak"), Space(15)]
        [SerializeField] [OnInspector(ReadOnly = true, Comment = "Eðer true ise oyuncunun collideri mage collediri ile çarpýþmaktadýr.")] public bool inFearTrigger;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool inTriggerEnter;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool counterForAngerBool;

        // player ve enemy karþýlaþmalarýný bu yapýyor. sakýn silme!!!. yoksa kalan tüm kodlar hata verir gardas. 
        // if you delete this, unity will get error for all params code.
        private void Update()
        {

        }

        #region onTriggerEnter and onTriggerExit
        public void OnTriggerEnter(Collider other)  // player ve enemy çarpýþmalarýný kontrol eder.
        {
            if (other.gameObject.tag == "FearParameter")
            {
                inFearTrigger = true;
                counterForAngerBool = true;
                inTriggerEnter = true;

            }
        }
        public void OnTriggerExit(Collider other)   // player ve enemy çarpýþmalarýný kontrol eder.
        {
            if (other.gameObject.tag == "FearParameter")
            {
                inFearTrigger = false;
                counterForAngerBool = false;
                inTriggerEnter = false;
            }
        }
        #endregion


    }
}