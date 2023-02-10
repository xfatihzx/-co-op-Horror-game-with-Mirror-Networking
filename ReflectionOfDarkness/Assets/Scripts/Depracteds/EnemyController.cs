using UnityEngine;
using UnityEngine.AI;

namespace Depracteds
{
#pragma warning disable
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] [OnInspector(ReadOnly = true)] private NavMeshAgent agent;
        [SerializeField] [OnInspector(ReadOnly = true)] private Transform currentTarget;
        [SerializeField] [OnInspector(ReadOnly = false)] private Transform[] targets;
        [SerializeField] [OnInspector(ReadOnly = false)] private TargetManagment targetManagment;


        [Header("Mage Anger"), Space(15)]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool iSeePlayer;
        [SerializeField] [OnInspector(ReadOnly = true)] private float enemyAngerCounter;
        [SerializeField] [OnInspector(ReadOnly = true)] public float enemysAnger;

        [Header("Mage modes"), Space(15)]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool killMode;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool attackMode;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool scaryMode;
        [SerializeField] [OnInspector(ReadOnly = true)] private float attackCounter;

        [Header("Mage Follow things"), Space(15)]
        [SerializeField] [OnInspector(ReadOnly = true)] private float followCounter;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool ýAmFollowing;
        [SerializeField] [OnInspector(ReadOnly = true)] private float cancelFollow;
        [SerializeField] [OnInspector(ReadOnly = true)] private float cancelFollowCounter;
        [SerializeField] [OnInspector(ReadOnly = true)] private float followFalseCounter;

        [Header("Hit things"), Space(15)]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool hit;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool whiteLine;
        [SerializeField] [OnInspector(ReadOnly = true)] private float whiteLineCounter;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool yellowLine;
        [SerializeField] [OnInspector(ReadOnly = true)] private float yellowLineCounter;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool redLine;
        [SerializeField] [OnInspector(ReadOnly = true)] private float redLineCounter;
        [SerializeField] [OnInspector(ReadOnly = true)] private int randomNumber;


        public EnemyScareController scares;
        public PlayerController PlayerController;


        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            EnemyAI();
            RayCastHitting();
            CounterForEnemyAnger();
            FollowTimes();
            TargetVisible();
            EnemyHit();
        }

        private void EnemyAI()
        {
            if (currentTarget == null)
            {
                float distance = float.MaxValue;

                for (int i = 0; i < targets.Length; i++)
                {
                    Transform target = targets[i];
                    if (target != null && target.gameObject.activeSelf)
                    {
                        float tempDistance = Vector3.Distance(transform.position, target.position);

                        if (distance > tempDistance)
                        {
                            distance = tempDistance;
                            currentTarget = target;
                        }
                    }   // enemy en yakýn hedefi bulup target'ý Destroy eder.
                }
            }

            if (currentTarget != null)      //seçili düþman yoksa en yakýn mesafedek düþman atamasý yapýlýr.
            {
                agent.destination = currentTarget.position;
            }
        }

        private void RayCastHitting()  //RycastHit ile enemy'nin forwad yön doðrulanýr.
        {                              //Hit fonksyonunun çalýþmasý için triggerlerin tetiklenmesi ve target enemy'nin forwad yönünde olmasý gereklidir.

            #region Raycast draw area (if debugging)
            if (Debug.isDebugBuild)
            {
                Color selectedColor = Color.black;

                Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 5, selectedColor);
            }
            #endregion
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Target")
            {
                currentTarget = null;
                Destroy(collision.gameObject);  //Destroy iþlemþeri yapýlýr.
            }
        }

        private void TargetVisible()
        {
#pragma warning disable CS0665 // Assignment in conditional expression is always constant
            if (targetManagment.isVisibleTarget == true)
#pragma warning restore CS0665 // Assignment in conditional expression is always constant
            {
                iSeePlayer = true; // targetManagmentdeki isVisibleTarget true döndürür.  if ýSeePlayer kontrolleri buradan yapýlacak
            }
            else
            {
                iSeePlayer = false;
            }
        }


        private void YorumVeHatýrlatma()
        {
            // takip ediyorum false
            // if takip ediyorum true ve ardýndan false ise kaçtý++; kaçtý == 3; öncelikli takip. onu gördüðünde hedef deðiþtirir. ama kaçabilmesi muhtemeldir hala. if kaçtý == 4 death mode for kaçtý ==4 olan player.

            // takip ettikçe öfkeforbüyücü ++0,15f; if == 100; 1 adet vuruþ. iþaretlemeden vuruþa geçer.
        }

        #region Enemy's all Anger functions and Hit!!!
        private void CounterForEnemyAnger()
        {
            if (enemysAnger > 100)
            {
                enemysAnger = 100;          // enemy anger 100 den byük olmamasý için atanmýþtýr. !!!Mathf.clemp(min,max) ile güncellenmelidir.
            }

            if (scares.counterForAngerBool == true)
            {
                //Debug.Log("Counter for anger(scares 1. if e girdi)");

                enemyAngerCounter += Time.deltaTime;

                if (enemyAngerCounter >= 10)
                {
                    //Debug.Log("(angerCounter)2. if e girdi");

                    enemysAnger++;

                    enemyAngerCounter = 0;

                    //Debug.Log("Enemy kýzýyor gaççýnnn");

                    if (enemysAnger == 100)
                    {
                        scaryMode = true;
                        attackMode = true;
                        killMode = true;  //kill mode örnek almasý için çaðýrýldý. önce kaç defa hit aldýðý kontrol edilecek sonrasýnda hit sayýsýna göre
                                          // büyücünün hangi moda gireceði kontrol edilecek.
                        Debug.Log(scaryMode + "   " + " Scary mode:");
                        Debug.Log(attackMode + "   " + "AttackMode mode:");
                        Debug.Log(killMode + "     " + " Kill mode:");
                    }
                }
            }
        }

        public void EnemyAngerActiobs()
        {
            // içi player actions gibi doldurulacak.


            // 0,100 arasý olan sýnýr min,max olarak deðiþtir. min i her ifde deðiþtir. ör if >=25 min=25;

            // vuruþ yaptýktan sonra anger in sýfýrlanmasý ve diðer iþlemler de yapýlacak.
        }

        public void FollowTimes()   //player'ýn enemyden kaçýp kaçamadýðýný ve kaç defa kaçtýðýný hesaplar.
        {
            if (scares.inTriggerEnter == true)      //EnemyScareControllerden gelen deðiþken.
            {
                followCounter += Time.deltaTime;

                followFalseCounter = 0;

                if (followCounter >= 15)
                {
                    ýAmFollowing = true;

                    cancelFollowCounter = 0;            //Enemy, player'i 15 sn boyunca trigger içinde tutarsa, Player'i takip ediyorum döndürür.
                }
            }
            else
            {
                if (followCounter > 1 && followCounter < 15)        //follow Counter 0dan büyük ve 15 den küçükse
                {
                    followFalseCounter += Time.deltaTime;

                    if (followFalseCounter > 10)    //takip counter' sýfýrlar. Böylece enemy yanlýþlýkla cancalFollowCounter'ý arttýrmaz.
                    {
                        followCounter = 0;
                    }
                }

                if (ýAmFollowing == true)
                {
                    cancelFollowCounter += Time.deltaTime;

                    if (cancelFollowCounter > 8)
                    {
                        ýAmFollowing = false;

                        followCounter = 0;

                        cancelFollow++;
                    }
                    Debug.Log(cancelFollow);  //enemy, player'i 15 sn takip ettikten sonra, Player triggerýn dýþýnda 8 snden fazla durursa CancelFollowCounter++ iþlemini yapar.
                }
            }
            //Debug.Log(ýAmFollowing + "     " + " ý am following target");

            if (cancelFollow == 3)
            {
                attackMode = true;

                attackCounter += Time.deltaTime;

                if (attackCounter > 10)
                {
                    attackMode = false;

                    cancelFollow = 0;
                    //!!!            // CancalFollow 3 e eþit olduðunda önce player'in aldýðý hasarlar kontrol edilmelidir. //!!!
                    //!!!            // hiç hasar almamýþsa scaryMode (3),                                                  //!!!
                    //!!!            // scary mode 3 defa çalýþmýþsa, attackMode (3),                                       //!!!
                    //!!!            // attackMode 3 defa çalýþmýssa, killMode (3),                                         //!!!
                    //!!!            //killMode 3 defa çalýþmýssa random sayý döndürerek killMode çalýþtýrmaya devam eder.  //!!!
                    //!!!            //Random sayý tutarsa player.Death(eklenecek fonksyon) çalýþýr.                        //!!!
                }   //!!!                                                                                                        //!!!

            }
        }

        private void EnemyHit()
        {
            // enemy hit olaylarý burada gerçekleþtirilecek.
            // beyaz çizgi == scary mode
            //sarý çizgi == attack mode
            //kýrmýzý çizgi == kill mode

            // follow times 3 olduðunda scary mode aktif olacak.
            // eðer scary mode aktifse ve player 3 adet beyaz çizgi hakkýný doldurduysa sarý çizgiye yani attak mode geç dicez.

            // if beyaz çizgi true ise 3, if sar = true 4, if kýrmýzý =true 5; adet cancel follow counter çalýþmasý gerek.

            // en son kill mode da eðer 3 çizgi vurduysa, 4. den itibaren random olarak ölme ihtimalini döndürecek.
            // yazýlacak en klay kod 1-10 arasýnda random sayý döndür. if x =6 ise kill, if false random counter++
            // if random counter == 1 ve random sayý 1-8 arasýnda, x int =4, if true kill, if false random cunter++;
            // if random counter ==2;  random sayý 1-6 arasýnda, random int=1; if true kill, false random counter++

            //bu mantýkla 1-4, 1-3,1-2, en son else kill olarak çalýþýr.





            if (hit == true)
            {
                if (whiteLineCounter < 4)
                {
                    whiteLine = true;

                    if (whiteLineCounter >= 3)
                    {
                        whiteLine = false;
                    }
                }
                else if (yellowLineCounter < 4)
                {
                    yellowLine = true;

                    if (yellowLineCounter >= 3)
                    {
                        yellowLine = false;
                    }
                }
                else if (redLineCounter < 4)
                {
                    redLine = true;

                    if (redLineCounter >= 3)
                    {
                        randomNumber = Random.Range(1, 10);
                        Debug.Log(randomNumber);
                    }
                }
            }
        }


        //!!!   //yýkarýdan gelen bilgiler ve kaç defa hit aldýðý kontrol edilmelidir.  //!!!
        //!!!   //enemyScare fonksyonu yazýlmalý ve 3 defa çalýþýp çalýþmadðý kontrol edilmeli. 3 edefa çalýþtýysa fonskyon terar çalýþamaz.    //!!!
        //!!!   //ayný adýmlar attackMode ve Kill mode için takþp edilirç   //!!
        //!!!   //kill mode'da 3 den sonra random ölme þansý eklenecektir. easy, normal, hard için ayrý random döngüleri yazýlacaktýr.
        //!!!   //easy için =>Random.Range(1,10);       her fonksondan sonra Random.Range(min,max-1); iþlemi yapýlýr. bir yerden sonra Random.Range(1,2); sabit býrakýlýr. bu þekilde sabit bir sayýda kesin olüm meydana gelmez.
        //!!!   //normal için =>Random.Range(1,6);      her fonksondan sonra Random.Range(min,max-1); iþlemi yapýlýr. bir yerden sonra Random.Range(1,2); sabit býrakýlýr. bu þekilde sabit bir sayýda kesin olüm meydana gelmez.
        //!!!   //hard için =>Random.Range(1,3);        her fonksondan sonra Random.Range(min,max-1); iþlemi yapýlýr. bir yerden sonra Random.Range(1,2); sabit býrakýlýr. bu þekilde sabit bir sayýda kesin olüm meydana gelmez.
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yapýp 2 true deðer dödürerek ihtimal %50 den %33 e düþürülebilir. Bu yöntemle %10 da düþürülebilir.(hayatta kalma þansý düþürülür).
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yapýp 2 true deðer dödürerek ihtimal %50 den %33 e düþürülebilir. Bu yöntemle %10 da düþürülebilir.(hayatta kalma þansý düþürülür).
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yapýp 2 true deðer dödürerek ihtimal %50 den %33 e düþürülebilir. Bu yöntemle %10 da düþürülebilir.(hayatta kalma þansý düþürülür).

        private void EnemyScary()  //enmy playeri korkutur. (ýþýnlanma); playerController.myFear==100;
        {

        }
        #endregion
    }
}