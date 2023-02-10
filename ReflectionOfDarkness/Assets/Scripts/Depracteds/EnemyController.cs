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
        [SerializeField] [OnInspector(ReadOnly = true)] private bool �AmFollowing;
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
                    }   // enemy en yak�n hedefi bulup target'� Destroy eder.
                }
            }

            if (currentTarget != null)      //se�ili d��man yoksa en yak�n mesafedek d��man atamas� yap�l�r.
            {
                agent.destination = currentTarget.position;
            }
        }

        private void RayCastHitting()  //RycastHit ile enemy'nin forwad y�n do�rulan�r.
        {                              //Hit fonksyonunun �al��mas� i�in triggerlerin tetiklenmesi ve target enemy'nin forwad y�n�nde olmas� gereklidir.

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
                Destroy(collision.gameObject);  //Destroy i�lem�eri yap�l�r.
            }
        }

        private void TargetVisible()
        {
#pragma warning disable CS0665 // Assignment in conditional expression is always constant
            if (targetManagment.isVisibleTarget == true)
#pragma warning restore CS0665 // Assignment in conditional expression is always constant
            {
                iSeePlayer = true; // targetManagmentdeki isVisibleTarget true d�nd�r�r.  if �SeePlayer kontrolleri buradan yap�lacak
            }
            else
            {
                iSeePlayer = false;
            }
        }


        private void YorumVeHat�rlatma()
        {
            // takip ediyorum false
            // if takip ediyorum true ve ard�ndan false ise ka�t�++; ka�t� == 3; �ncelikli takip. onu g�rd���nde hedef de�i�tirir. ama ka�abilmesi muhtemeldir hala. if ka�t� == 4 death mode for ka�t� ==4 olan player.

            // takip ettik�e �fkeforb�y�c� ++0,15f; if == 100; 1 adet vuru�. i�aretlemeden vuru�a ge�er.
        }

        #region Enemy's all Anger functions and Hit!!!
        private void CounterForEnemyAnger()
        {
            if (enemysAnger > 100)
            {
                enemysAnger = 100;          // enemy anger 100 den by�k olmamas� i�in atanm��t�r. !!!Mathf.clemp(min,max) ile g�ncellenmelidir.
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

                    //Debug.Log("Enemy k�z�yor ga���nnn");

                    if (enemysAnger == 100)
                    {
                        scaryMode = true;
                        attackMode = true;
                        killMode = true;  //kill mode �rnek almas� i�in �a��r�ld�. �nce ka� defa hit ald��� kontrol edilecek sonras�nda hit say�s�na g�re
                                          // b�y�c�n�n hangi moda girece�i kontrol edilecek.
                        Debug.Log(scaryMode + "   " + " Scary mode:");
                        Debug.Log(attackMode + "   " + "AttackMode mode:");
                        Debug.Log(killMode + "     " + " Kill mode:");
                    }
                }
            }
        }

        public void EnemyAngerActiobs()
        {
            // i�i player actions gibi doldurulacak.


            // 0,100 aras� olan s�n�r min,max olarak de�i�tir. min i her ifde de�i�tir. �r if >=25 min=25;

            // vuru� yapt�ktan sonra anger in s�f�rlanmas� ve di�er i�lemler de yap�lacak.
        }

        public void FollowTimes()   //player'�n enemyden ka��p ka�amad���n� ve ka� defa ka�t���n� hesaplar.
        {
            if (scares.inTriggerEnter == true)      //EnemyScareControllerden gelen de�i�ken.
            {
                followCounter += Time.deltaTime;

                followFalseCounter = 0;

                if (followCounter >= 15)
                {
                    �AmFollowing = true;

                    cancelFollowCounter = 0;            //Enemy, player'i 15 sn boyunca trigger i�inde tutarsa, Player'i takip ediyorum d�nd�r�r.
                }
            }
            else
            {
                if (followCounter > 1 && followCounter < 15)        //follow Counter 0dan b�y�k ve 15 den k���kse
                {
                    followFalseCounter += Time.deltaTime;

                    if (followFalseCounter > 10)    //takip counter' s�f�rlar. B�ylece enemy yanl��l�kla cancalFollowCounter'� artt�rmaz.
                    {
                        followCounter = 0;
                    }
                }

                if (�AmFollowing == true)
                {
                    cancelFollowCounter += Time.deltaTime;

                    if (cancelFollowCounter > 8)
                    {
                        �AmFollowing = false;

                        followCounter = 0;

                        cancelFollow++;
                    }
                    Debug.Log(cancelFollow);  //enemy, player'i 15 sn takip ettikten sonra, Player trigger�n d���nda 8 snden fazla durursa CancelFollowCounter++ i�lemini yapar.
                }
            }
            //Debug.Log(�AmFollowing + "     " + " � am following target");

            if (cancelFollow == 3)
            {
                attackMode = true;

                attackCounter += Time.deltaTime;

                if (attackCounter > 10)
                {
                    attackMode = false;

                    cancelFollow = 0;
                    //!!!            // CancalFollow 3 e e�it oldu�unda �nce player'in ald��� hasarlar kontrol edilmelidir. //!!!
                    //!!!            // hi� hasar almam��sa scaryMode (3),                                                  //!!!
                    //!!!            // scary mode 3 defa �al��m��sa, attackMode (3),                                       //!!!
                    //!!!            // attackMode 3 defa �al��m�ssa, killMode (3),                                         //!!!
                    //!!!            //killMode 3 defa �al��m�ssa random say� d�nd�rerek killMode �al��t�rmaya devam eder.  //!!!
                    //!!!            //Random say� tutarsa player.Death(eklenecek fonksyon) �al���r.                        //!!!
                }   //!!!                                                                                                        //!!!

            }
        }

        private void EnemyHit()
        {
            // enemy hit olaylar� burada ger�ekle�tirilecek.
            // beyaz �izgi == scary mode
            //sar� �izgi == attack mode
            //k�rm�z� �izgi == kill mode

            // follow times 3 oldu�unda scary mode aktif olacak.
            // e�er scary mode aktifse ve player 3 adet beyaz �izgi hakk�n� doldurduysa sar� �izgiye yani attak mode ge� dicez.

            // if beyaz �izgi true ise 3, if sar = true 4, if k�rm�z� =true 5; adet cancel follow counter �al��mas� gerek.

            // en son kill mode da e�er 3 �izgi vurduysa, 4. den itibaren random olarak �lme ihtimalini d�nd�recek.
            // yaz�lacak en klay kod 1-10 aras�nda random say� d�nd�r. if x =6 ise kill, if false random counter++
            // if random counter == 1 ve random say� 1-8 aras�nda, x int =4, if true kill, if false random cunter++;
            // if random counter ==2;  random say� 1-6 aras�nda, random int=1; if true kill, false random counter++

            //bu mant�kla 1-4, 1-3,1-2, en son else kill olarak �al���r.





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


        //!!!   //y�kar�dan gelen bilgiler ve ka� defa hit ald��� kontrol edilmelidir.  //!!!
        //!!!   //enemyScare fonksyonu yaz�lmal� ve 3 defa �al���p �al��mad�� kontrol edilmeli. 3 edefa �al��t�ysa fonskyon terar �al��amaz.    //!!!
        //!!!   //ayn� ad�mlar attackMode ve Kill mode i�in tak�p edilir�   //!!
        //!!!   //kill mode'da 3 den sonra random �lme �ans� eklenecektir. easy, normal, hard i�in ayr� random d�ng�leri yaz�lacakt�r.
        //!!!   //easy i�in =>Random.Range(1,10);       her fonksondan sonra Random.Range(min,max-1); i�lemi yap�l�r. bir yerden sonra Random.Range(1,2); sabit b�rak�l�r. bu �ekilde sabit bir say�da kesin ol�m meydana gelmez.
        //!!!   //normal i�in =>Random.Range(1,6);      her fonksondan sonra Random.Range(min,max-1); i�lemi yap�l�r. bir yerden sonra Random.Range(1,2); sabit b�rak�l�r. bu �ekilde sabit bir say�da kesin ol�m meydana gelmez.
        //!!!   //hard i�in =>Random.Range(1,3);        her fonksondan sonra Random.Range(min,max-1); i�lemi yap�l�r. bir yerden sonra Random.Range(1,2); sabit b�rak�l�r. bu �ekilde sabit bir say�da kesin ol�m meydana gelmez.
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yap�p 2 true de�er d�d�rerek ihtimal %50 den %33 e d���r�lebilir. Bu y�ntemle %10 da d���r�lebilir.(hayatta kalma �ans� d���r�l�r).
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yap�p 2 true de�er d�d�rerek ihtimal %50 den %33 e d���r�lebilir. Bu y�ntemle %10 da d���r�lebilir.(hayatta kalma �ans� d���r�l�r).
        //!!!   //Random.Range1,2); ye sabitlemek yerine Random.Range(1,3); yap�p 2 true de�er d�d�rerek ihtimal %50 den %33 e d���r�lebilir. Bu y�ntemle %10 da d���r�lebilir.(hayatta kalma �ans� d���r�l�r).

        private void EnemyScary()  //enmy playeri korkutur. (���nlanma); playerController.myFear==100;
        {

        }
        #endregion
    }
}