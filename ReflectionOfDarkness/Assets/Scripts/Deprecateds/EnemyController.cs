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
        [SerializeField] [OnInspector(ReadOnly = true)] private bool ŭAmFollowing;
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
                    }   // enemy en yakŭn hedefi bulup target'ŭ Destroy eder.
                }
            }

            if (currentTarget != null)      //seçili düŝman yoksa en yakŭn mesafedek düŝman atamasŭ yapŭlŭr.
            {
                agent.destination = currentTarget.position;
            }
        }

        private void RayCastHitting()  //RycastHit ile enemy'nin forwad yön do?rulanŭr.
        {                              //Hit fonksyonunun çalŭŝmasŭ için triggerlerin tetiklenmesi ve target enemy'nin forwad yönünde olmasŭ gereklidir.

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
                Destroy(collision.gameObject);  //Destroy iŝlemŝeri yapŭlŭr.
            }
        }

        private void TargetVisible()
        {
#pragma warning disable CS0665 // Assignment in conditional expression is always constant
            if (targetManagment.isVisibleTarget == true)
#pragma warning restore CS0665 // Assignment in conditional expression is always constant
            {
                iSeePlayer = true; // targetManagmentdeki isVisibleTarget true döndürür.  if ŭSeePlayer kontrolleri buradan yapŭlacak
            }
            else
            {
                iSeePlayer = false;
            }
        }


        private void YorumVeHatŭrlatma()
        {
            // takip ediyorum false
            // if takip ediyorum true ve ardŭndan false ise kaçtŭ++; kaçtŭ == 3; öncelikli takip. onu gördü?ünde hedef de?iŝtirir. ama kaçabilmesi muhtemeldir hala. if kaçtŭ == 4 death mode for kaçtŭ ==4 olan player.

            // takip ettikçe öfkeforbüyücü ++0,15f; if == 100; 1 adet vuruŝ. iŝaretlemeden vuruŝa geçer.
        }

        #region Enemy's all Anger functions and Hit!!!
        private void CounterForEnemyAnger()
        {
            if (enemysAnger > 100)
            {
                enemysAnger = 100;          // enemy anger 100 den byük olmamasŭ için atanmŭŝtŭr. !!!Mathf.clemp(min,max) ile güncellenmelidir.
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

                    //Debug.Log("Enemy kŭzŭyor gaççŭnnn");

                    if (enemysAnger == 100)
                    {
                        scaryMode = true;
                        attackMode = true;
                        killMode = true;  //kill mode örnek almasŭ için ça?ŭrŭldŭ. önce kaç defa hit aldŭ?ŭ kontrol edilecek sonrasŭnda hit sayŭsŭna göre
                                          // büyücünün hangi moda girece?i kontrol edilecek.
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


            // 0,100 arasŭ olan sŭnŭr min,max olarak de?iŝtir. min i her ifde de?iŝtir. ör if >=25 min=25;

            // vuruŝ yaptŭktan sonra anger in sŭfŭrlanmasŭ ve di?er iŝlemler de yapŭlacak.
        }

        public void FollowTimes()   //player'ŭn enemyden kaçŭp kaçamadŭ?ŭnŭ ve kaç defa kaçtŭ?ŭnŭ hesaplar.
        {
            if (scares.inTriggerEnter == true)      //EnemyScareControllerden gelen de?iŝken.
            {
                followCounter += Time.deltaTime;

                followFalseCounter = 0;

                if (followCounter >= 15)
                {
                    ŭAmFollowing = true;

                    cancelFollowCounter = 0;            //Enemy, player'i 15 sn boyunca trigger içinde tutarsa, Player'i takip ediyorum döndürür.
                }
            }
            else
            {
                if (followCounter > 1 && followCounter < 15)        //follow Counter 0dan büyük ve 15 den küçükse
                {
                    followFalseCounter += Time.deltaTime;

                    if (followFalseCounter > 10)    //takip counter' sŭfŭrlar. Böylece enemy yanlŭŝlŭkla cancalFollowCounter'ŭ arttŭrmaz.
                    {
                        followCounter = 0;
                    }
                }

                if (ŭAmFollowing == true)
                {
                    cancelFollowCounter += Time.deltaTime;

                    if (cancelFollowCounter > 8)
                    {
                        ŭAmFollowing = false;

                        followCounter = 0;

                        cancelFollow++;
                    }
                    Debug.Log(cancelFollow);  //enemy, player'i 15 sn takip ettikten sonra, Player triggerŭn dŭŝŭnda 8 snden fazla durursa CancelFollowCounter++ iŝlemini yapar.
                }
            }
            //Debug.Log(ŭAmFollowing + "     " + " ŭ am following target");

            if (cancelFollow == 3)
            {
                attackMode = true;

                attackCounter += Time.deltaTime;

                if (attackCounter > 10)
                {
                    attackMode = false;

                    cancelFollow = 0;
                    //!!!            // CancalFollow 3 e eŝit oldu?unda önce player'in aldŭ?ŭ hasarlar kontrol edilmelidir. //!!!
                    //!!!            // hiç hasar almamŭŝsa scaryMode (3),                                                  //!!!
                    //!!!            // scary mode 3 defa çalŭŝmŭŝsa, attackMode (3),                                       //!!!
                    //!!!            // attackMode 3 defa çalŭŝmŭssa, killMode (3),                                         //!!!
                    //!!!            //killMode 3 defa çalŭŝmŭssa random sayŭ döndürerek killMode çalŭŝtŭrmaya devam eder.  //!!!
                    //!!!            //Random sayŭ tutarsa player.Death(eklenecek fonksyon) çalŭŝŭr.                        //!!!
                }   //!!!                                                                                                        //!!!

            }
        }

        private void EnemyHit()
        {
            // enemy hit olaylarŭ burada gerçekleŝtirilecek.
            // beyaz çizgi == scary mode
            //sarŭ çizgi == attack mode
            //kŭrmŭzŭ çizgi == kill mode

            // follow times 3 oldu?unda scary mode aktif olacak.
            // e?er scary mode aktifse ve player 3 adet beyaz çizgi hakkŭnŭ doldurduysa sarŭ çizgiye yani attak mode geç dicez.

            // if beyaz çizgi true ise 3, if sar = true 4, if kŭrmŭzŭ =true 5; adet cancel follow counter çalŭŝmasŭ gerek.

            // en son kill mode da e?er 3 çizgi vurduysa, 4. den itibaren random olarak ölme ihtimalini döndürecek.
            // yazŭlacak en klay kod 1-10 arasŭnda random sayŭ döndür. if x =6 ise kill, if false random counter++
            // if random counter == 1 ve random sayŭ 1-8 arasŭnda, x int =4, if true kill, if false random cunter++;
            // if random counter ==2;  random sayŭ 1-6 arasŭnda, random int=1; if true kill, false random counter++

            //bu mantŭkla 1-4, 1-3,1-2, en son else kill olarak çalŭŝŭr.





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


        //!!!   //yŭkarŭdan gelen bilgiler ve kaç defa hit aldŭ?ŭ kontrol edilmelidir.  //!!!
        //!!!   //enemyScare fonksyonu yazŭlmalŭ ve 3 defa çalŭŝŭp çalŭŝmad?ŭ kontrol edilmeli. 3 edefa çalŭŝtŭysa fonskyon terar çalŭŝamaz.    //!!!
        //!!!   //aynŭ adŭmlar attackMode ve Kill mode için takŝp edilirç   //!!
        //!!!   //kill mode'da 3 den sonra random ölme ŝansŭ eklenecektir. easy, normal, hard için ayrŭ random döngüleri yazŭlacaktŭr.
        //!!!   //easy için =>Random.Range(1,10);       her fonksondan sonra Random.Range(min,max-1); iŝlemi yapŭlŭr. bir yerden sonra Random.Range(1,2); sabit bŭrakŭlŭr. bu ŝekilde sabit bir sayŭda kesin olüm meydana gelmez.
        //!!!   //normal için =>Random.Range(1,6);      her fonksondan sonra Random.Range(min,max-1); iŝlemi yapŭlŭr. bir yerden sonra Random.Range(1,2); sabit bŭrakŭlŭr. bu ŝekilde sabit bir sayŭda kesin olüm meydana gelmez.
        //!!!   //hard için =>Random.Range(1,3);        her fonksondan sonra Random.Range(min,max-1); iŝlemi yapŭlŭr. bir yerden sonra Random.Range(1,2); sabit bŭrakŭlŭr. bu ŝekilde sabit bir sayŭda kesin olüm meydana gelmez.
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yapŭp 2 true de?er dödürerek ihtimal %50 den %33 e düŝürülebilir. Bu yöntemle %10 da düŝürülebilir.(hayatta kalma ŝansŭ düŝürülür).
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yapŭp 2 true de?er dödürerek ihtimal %50 den %33 e düŝürülebilir. Bu yöntemle %10 da düŝürülebilir.(hayatta kalma ŝansŭ düŝürülür).
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yapŭp 2 true de?er dödürerek ihtimal %50 den %33 e düŝürülebilir. Bu yöntemle %10 da düŝürülebilir.(hayatta kalma ŝansŭ düŝürülür).

        private void EnemyScary()  //enmy playeri korkutur. (ŭŝŭnlanma); playerController.myFear==100;
        {

        }
        #endregion
    }
}