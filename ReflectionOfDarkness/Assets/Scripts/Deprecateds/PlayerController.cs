using System.Collections;
using UnityEngine;

namespace Depracteds
{
#pragma warning disable
    [AddComponentMenu("")]
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera Settings :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool cursorVisible = false;
        [SerializeField] [OnInspector(ReadOnly = true)] private CursorLockMode localState = CursorLockMode.Locked;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float lookSpeed = 2.0f;
        [SerializeField] private float lookXLimitUp = -30;
        [SerializeField] private float lookXLimitDown = 75;

        [Header("Movement Settings :")]
        [SerializeField] [OnInspector(ReadOnly = true)] public bool canMove;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool canJump;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool isJumping;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool isRunning;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool OnGround;
        [SerializeField] [OnInspector(ReadOnly = true)] private float fallTime;
        [SerializeField] [OnInspector(ReadOnly = true)] private float curSpeedX = 0;
        [SerializeField] [OnInspector(ReadOnly = true)] private float curSpeedY = 0;
        [SerializeField] private float walkingSpeed = 7.5f;
        [SerializeField] private float runningSpeed = 11.5f;
        [SerializeField] private float jumpSpeed = 8.0f;
        [SerializeField] private float gravity = 20.0f;

        [Header("Flashlight settings :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool openFlash;
        [SerializeField] private GameObject flashlight_On;
        [SerializeField] private GameObject flashlight_Off;

        [Header("Player fear things :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private float fearCounterPlas;
        [SerializeField] [OnInspector(ReadOnly = true)] private float fearCounterMinus;
        [SerializeField] [OnInspector(ReadOnly = false)] private float myFear;
        [SerializeField] [OnInspector(ReadOnly = true)] private float myFearLimitMin = 0;
        [SerializeField] [OnInspector(ReadOnly = true)] private float myFearLimitMax = 100;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool myFearPlasBool;

        [Header("Player fear bools :")]
        [SerializeField] [OnInspector(ReadOnly = true)] public bool fearIs50;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool fearIs70;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool fearIs85;
        [SerializeField] [OnInspector(ReadOnly = true)] public bool fearIs100;

        [Header("Player fear object interact percent :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool objectInteractionIs50percent;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool objectInteractionIs30percent;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool objectInteractionIs20percent;
        [SerializeField] [OnInspector(ReadOnly = true)] private bool objectInteractionIsImpossible;

        [Header("Player potion things :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private bool drinkPotion;

        [Header("Game modes: ")]
        [SerializeField] [OnInspector(ReadOnly = false)] private bool gameModeIsEasy;
        [SerializeField] [OnInspector(ReadOnly = false)] private bool gameModeIsNormal;
        [SerializeField] [OnInspector(ReadOnly = false)] private bool gameModeIsHard;
        [SerializeField] [OnInspector(ReadOnly = true)] private float CounterForThings;
        [SerializeField] [OnInspector(ReadOnly = true)] private float CounterForThings2;
        [SerializeField] [OnInspector(ReadOnly = true)] private float CounterForThings3;

        [Header("Random things: ")]


        [Header("Other Settings :")]
        [SerializeField] [OnInspector(ReadOnly = true)] private Animator animator;
        [SerializeField] [OnInspector(ReadOnly = true)] private CharacterController character;
        [SerializeField] [OnInspector(ReadOnly = true)] private InteractionController interaction;

        public EnemyScareController scares;
        public InventoryController inventory;
        public EnemyController EnemyController;

        #region Global fields
        private Vector3 moveDirection;
        private float rotationX;
        #endregion

        private void Start()
        {
            #region Player Movement and Look fields sets
            moveDirection = Vector3.zero;
            rotationX = 0;
            canMove = true;
            #endregion

            #region Components sets
            animator = GetComponent<Animator>();
            character = GetComponent<CharacterController>();
            interaction = GetComponent<InteractionController>();
            #endregion

            #region  Flashlight sets
            flashlight_On.SetActive(false);
            flashlight_Off.SetActive(true);
            #endregion

            #region Cursor properties sets
            Cursor.visible = cursorVisible;
            Cursor.lockState = localState;
            #endregion
        }
        private void Update()
        {
            MouseAndMovement();
            FlahlightController();
            PlayerIsground();

            RandomEnemyandPlayersTings30scond();
            RandomPlayersTings3Minute();
            RandomEnemyThings4Minute();
        }

        private void FixedUpdate()
        {
            isPlayerHasFear();
            FearValueForActions();

            #region set limit for my fear
            myFear = Mathf.Clamp(myFear, myFearLimitMin, myFearLimitMax);
            #endregion

            #region random  //silinecek
            if (Input.GetKeyDown(KeyCode.K))   // düzenlenecek
            {
                int randomNumber = Random.Range(1, 25);
                Debug.Log(randomNumber + "random number");

                if (randomNumber == 7)
                {
                    Debug.Log(randomNumber + "    " + "çay var simit var neye bakıyon.");
                }
            }
            #endregion random sayı döndürme. 
        }

        private void MouseAndMovement()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            isRunning = Input.GetKey(KeyCode.LeftShift);

            canJump = canMove && character.isGrounded;

            if (Input.GetButtonUp("Jump") && canJump)
            {
                isJumping = true;
            }
            else if (isJumping && character.isGrounded)
            {
                isJumping = false;
            }

            curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;


            animator.SetBool("JumpParam", isJumping);
            animator.SetBool("JumpParamForward", isJumping && isRunning);

            animator.SetBool("RunParam", isRunning);

            animator.SetFloat("Horizontal", curSpeedY);
            animator.SetFloat("Vertical", curSpeedX);

            float movementDirectionY = moveDirection.y;

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButtonUp("Jump") && canJump)
            {
                fallTime -= 0.58f;
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!character.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            character.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, lookXLimitUp, lookXLimitDown);

                playerCamera
                    .transform
                    .localRotation = Quaternion.Euler(rotationX, 0, 0);

                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }

        private void FlahlightController()
        {
            if (Input.GetKeyDown(KeyCode.F))  // aç kapa fonksyonları için f tuşu atanır.
            {
                openFlash = !openFlash;     //oyun başladığında fener kapalı olarak atanır.
                if (openFlash)
                {
                    FindObjectOfType<FlashlightManager>().Switch(true);     //fener player'ın elindeyse aç kapa fonksyonlarını kullanabilir.

                    flashlight_On.SetActive(true);
                    flashlight_Off.SetActive(false);
                }
                else
                {
                    FindObjectOfType<FlashlightManager>().Switch(false);

                    flashlight_On.SetActive(false);
                    flashlight_Off.SetActive(true);
                }
            }
        }

        private void PlayerIsground()
        {
            if (character.isGrounded)       //player'ın yerde olup olmadığını kontrol eder.
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }

            if (OnGround == true)
            {
                if (fallTime > 0.4f)
                {
                    // yavaşlatma ve ses efekti burada yapılacak 
                }

                fallTime = 0;       //yere temas edince sayaçı sıfırlar.
            }
            else
            {
                fallTime += Time.deltaTime;     // player'ın havadaki süresini ölçer.
            }
        }

        #region Player all fear things      //player'ın tüm korku işlemleri bu alanda yapılır
        private void isPlayerHasFear()      //
        {
            if (scares.inFearTrigger == true)  //limit değeri updete içinde tanımlandı.     //enemeyScareController'daki onTrigeerEnter metodunu kontrol eder.
            {
                fearCounterPlas += Time.deltaTime;

                if (fearCounterPlas >= 1) //süre 5 olarak değiştirilecek
                {
                    myFear++;
                    myFearPlasBool = true;          //normalde bu kod yapısı gereksiz ama onTriggerExit update'de hata verdiği için kullanılır.

                    fearCounterPlas = 0;
                    fearCounterMinus = 0;           //trigger true oldupunda sayaç başlar. sayaç if'i karşıladığında sayaç sıfırlanır ve korku 1 artar.
                }
            }
            else if (scares.inFearTrigger == false)
            {
                if (myFearPlasBool == true)     //korku en az 1 kere artmış olmalı. 0 a eşit olmamalı.
                {
                    fearCounterMinus += Time.deltaTime;

                    if (myFear == 0)
                    {
                        myFearPlasBool = false;   //gereksiz kod sadece 0 olunca sayacın çalışmayı bırakması için yazıldı.
                    }

                    if (fearCounterMinus >= 1) //süre 5 olarak değiştirilecek
                    {
                        myFear--;

                        fearCounterMinus = 0;
                        fearCounterPlas = 0;        //trigger true oldupunda sayaç başlar. sayaç if'i karşıladığında sayaç sıfırlanır ve korku 1 azalır.
                    }
                }
            }               //!!! enter trigger çalıştığında exit triggerin sayacı sıfırlanmaz.!!! //!!! exit trigger çalıştığında enter triggerin sayacı sıfırlanmaz.!!!
        }
        //player fear actionsda korkunun belli miktarda düşüşünü ayarla. örn: 35 ise 25 e kadar düşer. 25den 0 a indirmek için büyü yapması yada bir objeyle etkileşime girmesi gerekir.

        private void FearValueForActions()  // for player   //player'ın korkusuna göre çalışan bool'ler yer alır.
        {
            if (myFear <= 49)
            {
                interaction.isPlayerFear = false;
                fearIs50 = false;
                fearIs70 = false;
                fearIs85 = false;
                fearIs100 = false;

                if (myFear >= 25 && myFear < 49)
                {
                    myFearLimitMin = 25;            //korku belli bir seviyeyi geçince min limit atanır. Atanan limitin altına inmesi için oyun içi görev gerekir(potion dirinking).
                }
                //Debug.Log("I do not scary anything");
            }
            else if (myFear >= 50 && myFear <= 69 && myFearLimitMin <= 50)
            {
                interaction.isPlayerFear = true;
                fearIs50 = true;
                fearIs70 = false;
                fearIs85 = false;
                fearIs100 = false;

                myFearLimitMin = 50;    //50 nin altına düşmesi için büyü gerekiyor.    büyü ile 50 nin altına inmesini sağlayacak görevi oluştur.

                objectInteractionIs50percent = true;  // obje ile etkileşime geçme şansı yüzde 50 olur.
            }//korku belli bir seviyeyi geçince min limit atanır. Atanan limitin altına inmesi için oyun içi görev gerekir(potion dirinking).
            else if (myFear >= 70 && myFear <= 84 && myFearLimitMin <= 70)
            {
                interaction.isPlayerFear = true;
                fearIs50 = false;
                fearIs70 = true;
                fearIs85 = false;
                fearIs100 = false;   //70 ve 85 de hata veriyor hatayı çöz.

                myFearLimitMin = 70;

                objectInteractionIs30percent = true;
                objectInteractionIs50percent = false;
                // obje ile etkileşime girme şansı yüzde 30 olacak.

                //Debug.Log("I scary (70)");
            }//korku belli bir seviyeyi geçince min limit atanır. Atanan limitin altına inmesi için oyun içi görev gerekir(potion dirinking).
            else if (myFear >= 85 && myFear <= 99 && myFearLimitMin <= 85)
            {
                interaction.isPlayerFear = true;
                fearIs50 = false;
                fearIs70 = false;
                fearIs85 = true;
                fearIs100 = false;

                myFearLimitMin = 85;

                objectInteractionIs20percent = true;
                objectInteractionIs50percent = false;
                objectInteractionIs30percent = false;
                //obje ile etkileşime girme şansı yüzde 20.

                //Debug.Log("I Really scary(85)");
            }   //korku belli bir seviyeyi geçince min limit atanır. Atanan limitin altına inmesi için oyun içi görev gerekir(potion dirinking).
            else if (myFear == 100 && myFearLimitMin <= 100)   // min değer 100 olamayacağı için 85 olarak ayarlandı.
            {
                interaction.isPlayerFear = true;
                fearIs50 = false;       //obje etkileşimleri 15 sb duracak ve 15 sn korku 100 olacak sonra 85 e düşecek.
                fearIs70 = false;
                fearIs85 = false;
                fearIs100 = true;

                myFearLimitMin = 100; // fear 100 olduğunda 15 sn beklemek zorunldur. iksir içerek kaçınılamaz.

                objectInteractionIsImpossible = true;
                objectInteractionIs50percent = false;
                objectInteractionIs30percent = false;
                objectInteractionIs20percent = false;
                //objeler ile 15 sn etkileşime geçilemez.

                if (objectInteractionIsImpossible == true)
                {
                    // korku 100 olduğunda bir sayaç çalısır ve 15 snden geriye sayar.
                    // bu süre boyunca hiçbir obje ile etkileşime girilemez. sürenin sonunda hala yüz olup olmadıüı kontrol edilir.

                    StartCoroutine(WaitForFear15second());      //private IEnumerator WaitForFear15second()'a gönderir. 380.satır.
                                                                //Debug.Log("Start coroutine is work" + "    " + "you have to wait 15 secend");
                }
                if (Logger.IsEnabled(LogLevel.Info))
                    Logger.@do.Info("I want to go home(100)");
            }
        }

        /// <summary>
        /// Korku 100 olunca 15 saniye boyunca 100 kalması için çalışır.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForFear15second()
        {
            yield return new WaitForSeconds(15);        //15 sn bekler.

            objectInteractionIsImpossible = false;      // 15 sn'nin ardından obje ile tekrar etkileşime geçilebilir.

            myFear = myFear - 5;

            myFearLimitMin = 85;                        // korku 95 e eşitlenip min limit 85 e atanır. karakter iksir içerek azaltmak zorundadır.
        } //fear action için kullanılıyor.

        /// <summary>
        /// InteractionController içinde kontrol için yapıldı.
        /// Karater korkuyorken etkileşime geçme işlemine ihtimal katmak için.
        /// </summary>
        /// <returns>Etkileşim ihtimalini boolean olarak döndürür.</returns>
        public bool PlayerInteractPossibilityForFear()
        {
            bool isLucky = false;//etkileşime geçecek kadar şanslı mı ?

            int luckyNumber = 2;//şanslı sayı
            int randomNumber = 0;//üretilecek sayı

            if (fearIs50)// luck : 1/2
            {
                randomNumber = Random.Range(1, 2);
            }
            else if (fearIs70)// luck : 1/3
            {
                randomNumber = Random.Range(1, 3);
            }
            else if (fearIs85)// luck : 1/5
            {
                randomNumber = Random.Range(1, 5);
            }
            else if (fearIs100)// luck : blow enemy's carrot
            {
                randomNumber = 1;//bilerek 1 verdim çünkü etkileşime geçemesin diye.
            }
            else//eğer 50 'yi daha geçmediyse direk luckyNumber al diyorum. 100% işlem yapabilmesi için 
            {
                randomNumber = luckyNumber;
            }

            if (randomNumber == luckyNumber)
            {
                isLucky = true;//wow, şanslı sayıyı tutturdu. Bu player şanslıymış.
            }

            return isLucky;
        }

        /* private void RandomEnemyandPlayersTings30scond()  //30 snde bir çalısır. içindeki rakamlar easy=10, normal = 15, hard =20. rakamlar değiştirilebilir.
         {
             int randomNumber = 0;//üretilecek sayı

             int playerNumber = 0;
             int enemyNumber = 1;

             if (gameModeIsEasy == true)
             {
                 CounterForThings += Time.deltaTime;

                 if (CounterForThings >= 3)
                 {
                     randomNumber = Random.Range(1, 10);  //random ==2 ise
                     Debug.Log("Random number for game mode easy: " + "(30 second)     " + randomNumber);

                     if (randomNumber != 2)
                     {
                         randomSecond = false;

                         //CounterForThings = 0;
                     }
                     else if (randomNumber == 2) // ilk sayı 2 döndüğünde 2.defa else ife girip değer döndürüyor.
                     {
                         //EnemyController.enemysAnger += 10;

                         randomSecond = true;

                         CounterForThings = 0;
                     }
                     Debug.Log("Random second: " + randomSecond);
                 }
                 if (CounterForThings >= 4)
                 {
                     if (randomNumber != 2)
                     {
                         CounterForThings = 0;
                     }
                     else if (randomNumber == 2)
                     {
                         myFear += 10;

                         playerNumber = 0;
                         enemyNumber = 1;

                         CounterForThings = 0;
                     }
                 }
             }
             else if (gameModeIsNormal == true)
             {
                 CounterForThings += Time.deltaTime;

                 if (CounterForThings >= 30)
                 {
                     randomNumber = Random.Range(1, 6);  //random ==2 ise
                     Debug.Log("Random number for game mode normal: " + "(30 second)    " + randomNumber);

                     if (randomNumber == 2 && playerNumber == 0 && enemyNumber == 1)
                     {
                         EnemyController.enemysAnger += 15;

                         playerNumber = 1;
                         enemyNumber = 0;

                         CounterForThings = 0;
                     }
                     else if (randomNumber != 2 && playerNumber == 0 && enemyNumber == 1)
                     {
                         CounterForThings = 0;
                     }

                     if (randomNumber == 2 && playerNumber == 1 && enemyNumber == 0)
                     {
                         myFear += 15;

                         playerNumber = 0;
                         enemyNumber = 1;

                         CounterForThings = 0;
                     }
                     else if (randomNumber != 2 && playerNumber == 1 && enemyNumber == 0)
                     {
                         CounterForThings = 0;
                     }
                 }
             }
             else if (gameModeIsHard == true)
             {
                 CounterForThings += Time.deltaTime;

                 if (CounterForThings >= 30)
                 {
                     randomNumber = Random.Range(1, 4);  //random ==2 ise
                     Debug.Log("Random number for game mode hard: " + "(30 second)     " + randomNumber);

                     if (randomNumber == 2 && playerNumber == 0 && enemyNumber == 1)
                     {
                         EnemyController.enemysAnger += 20;

                         playerNumber = 1;
                         enemyNumber = 0;

                         CounterForThings = 0;
                     }
                     else if (randomNumber != 2 && playerNumber == 0 && enemyNumber == 1)
                     {
                         CounterForThings = 0;
                     }

                     if (randomNumber == 2 && playerNumber == 1 && enemyNumber == 0)
                     {
                         myFear += 20;

                         playerNumber = 0;
                         enemyNumber = 1;

                         CounterForThings = 0;
                     }
                     else if (randomNumber != 2 && playerNumber == 1 && enemyNumber == 0)
                     {
                         CounterForThings = 0;
                     }
                 }
             }
         }
        */

        /* private void RandomEnemyandPlayersTings30scond()  //30 snde bir çalısır. içindeki rakamlar easy=10, normal = 15, hard =20. rakamlar değiştirilebilir.
         {
             int randomNumber = 0;//üretilecek sayı

             if (gameModeIsEasy == true)
             {
                 randomNumber = Random.Range(1, 5);

                 CounterForThings += Time.deltaTime;

                 if (CounterForThings >= 30)
                 {
                     if (randomNumber == 2 && randomSecond == true)
                     {
                         Debug.Log("Random number: " + randomNumber + "   " + "Random second==true " + "Game mode easy");

                         if (RandomBool == true)
                         {
                             EnemyController.enemysAnger += 10;
                             RandomBool = false;
                             StartCoroutine(WaitForFear28second());
                         }
                         CounterForThings = 0;

                         RandomBool = true;
                     }
                     else if (randomNumber == 2 && randomSecond == false)
                     {
                         if (RandomBool==true)
                         {
                             myFear = myFear + 10;
                             RandomBool = false;

                             StartCoroutine(WaitForFear28second());
                         }
                         CounterForThings = 0;

                         randomSecond = true;

                         Debug.Log("Random number: " + randomNumber + "   " + "Random second==false " + "Game mode easy");
                     }
                     else if (randomNumber != 2)
                     {
                         CounterForThings = 0;

                         Debug.Log("Random number: " + randomNumber + "   " + "Game mode easy");
                     }
                 }
             }
         }

         private IEnumerator WaitForFear28second()
         {
             yield return new WaitForSeconds(28);
             Debug.Log("start corantine e girdi");

             randomSecond = false;
         }
        */

        private void RandomEnemyandPlayersTings30scond() // tam tersi korku ve anger azaltan fonk yada yoz. sadece game==true için
        {
            int randomNumber = 0;//üretilecek sayı

            if (gameModeIsEasy == true)
            {
                randomNumber = Random.Range(1, 7); // (1,7), (1,6), (1,5);

                CounterForThings += Time.deltaTime; // int olarak nasıl yazdırırlır araştır.

                if ((CounterForThings) >= 30)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        myFear += 5;
                        EnemyController.enemysAnger += 5;
                        CounterForThings = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //sayaç 30 snden büyük olunca ve oyun modu easy ise random sayı döndürülür. Random sayı 2 ye eşitse korku ve ebemy anger 5 artar.
                    }
                    else
                    {
                        myFear++;
                        EnemyController.enemysAnger++;
                        CounterForThings = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random sayı 2 ye eşit değilse player'ınkorkusy ve enemy'nin öfkesi 1 artar.
                    }
                }
            }
            else if (gameModeIsNormal == true)
            {
                randomNumber = Random.Range(1, 6); // (1,7), (1,6), (1,5);

                CounterForThings += Time.deltaTime;

                if (CounterForThings >= 30)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        myFear += 10;
                        EnemyController.enemysAnger += 10;
                        CounterForThings = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //sayaç 30 snden büyük olunca ve oyun modu normal ise random sayı döndürülür. Random sayı 2 ye eşitse korku ve ebemy anger 10 artar.
                    }
                    else
                    {
                        myFear = myFear + 2;
                        EnemyController.enemysAnger = EnemyController.enemysAnger + 2;
                        CounterForThings = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random sayı 2 ye eşit değilse player'ınkorkusy ve enemy'nin öfkesi 2 artar.
                    }
                }
            }
            else if (gameModeIsHard == true)
            {
                randomNumber = Random.Range(1, 7); // (1,7), (1,6), (1,5);

                CounterForThings += Time.deltaTime;

                if (CounterForThings >= 30)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        myFear += 15;
                        EnemyController.enemysAnger += 15;
                        CounterForThings = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //sayaç 30 snden büyük olunca ve oyun modu hard ise random sayı döndürülür. Random sayı 2 ye eşitse korku ve ebemy anger 15 artar.
                    }
                    else
                    {
                        myFear = myFear + 4;
                        EnemyController.enemysAnger = EnemyController.enemysAnger + 4;
                        CounterForThings = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random sayı 2 ye eşit değilse player'ınkorkusy ve enemy'nin öfkesi 4 artar.
                    }
                }
            }
        }
        //enemy ve my fear 100 geçmemeli kontrollerini yap.
        private void RandomPlayersTings3Minute()// 180sn dkde bir çalışır. 180sn(1,4), 170sn (1,3), 155sn(1,2)  enemy anger ve my fear da arttırır.
        {
            int randomNumber = 0;//üretilecek sayı

            if (gameModeIsEasy == true)
            {
                randomNumber = Random.Range(1, 4);

                CounterForThings2 += Time.deltaTime;

                if (CounterForThings2 >= 180)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        myFear = 100;
                        EnemyController.enemysAnger += 10;
                        CounterForThings2 = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //game mode easy ve random number eşittir 2 ise; player'ın korkusu 100 olur, enemy'nin korkusuna 10 eklenir.
                    }
                    else
                    {
                        myFear += 10;
                        EnemyController.enemysAnger += 10; // else de her 3 4 dkde bir döndüğü için +10 gibi rakam eklenebilir.
                        CounterForThings2 = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random number 2 ye eşit değilse; player'ınkorkusu ve enemy'nin öfkesi 10 artar.
                    }
                }
            }
            else if (gameModeIsNormal == true)
            {
                randomNumber = Random.Range(1, 3);

                CounterForThings2 += Time.deltaTime;

                if (CounterForThings2 >= 170)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        myFear = 100;
                        EnemyController.enemysAnger += 15;
                        CounterForThings2 = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //game mode normal ve random number eşittir 2 ise; player'ın korkusu 100 olur, enemy'nin korkusuna 15 eklenir.
                    }
                    else
                    {
                        myFear += 15;
                        EnemyController.enemysAnger += 15;
                        CounterForThings2 = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random number 2 ye eşit değilse; player'ınkorkusu ve enemy'nin öfkesi 15 artar.
                    }
                }
            }
            else if (gameModeIsHard == true)
            {
                randomNumber = Random.Range(1, 2);

                CounterForThings2 += Time.deltaTime;

                if (CounterForThings2 >= 155)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        myFear = 100;
                        EnemyController.enemysAnger += 30;
                        CounterForThings2 = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //game mode hard ve random number eşittir 2 ise; player'ın korkusu 100 olur, enemy'nin korkusuna 30 eklenir.
                    }
                    else
                    {
                        myFear += 20;
                        EnemyController.enemysAnger += 20;
                        CounterForThings2 = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random number 2 ye eşit değilse; player'ınkorkusu ve enemy'nin öfkesi 20 artar.
                    }
                }
            }
        }

        private void RandomEnemyThings4Minute()
        {
            int randomNumber = 0;//üretilecek sayı

            if (gameModeIsEasy == true)
            {
                randomNumber = Random.Range(1, 4);

                CounterForThings3 += Time.deltaTime;

                if (CounterForThings3 >= 240)    //240 220  200
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        EnemyController.enemysAnger = 100;
                        CounterForThings3 = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //240 snde bir random sayı döndürür. Randomsayı 2 ye eşit ise; enemy'sFear 100 olur.
                    }
                    else
                    {
                        myFear += 10;
                        EnemyController.enemysAnger += 10;  // 10 15 20
                        CounterForThings3 = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: Fasle" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);


                        //else durumuna  player'ın korkusuna ve enemy'nin korkusuna 10 ekler.
                    }
                }
            }
            else if (gameModeIsNormal == true)
            {
                randomNumber = Random.Range(1, 3);

                CounterForThings3 += Time.deltaTime;

                if (CounterForThings3 >= 220)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        EnemyController.enemysAnger = 100;
                        CounterForThings3 = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //220 snde bir random sayı döndürür. Randomsayı 2 ye eşit ise; enemy'sFear 100 olur. 
                    }
                    else
                    {
                        myFear += 15;
                        EnemyController.enemysAnger += 15;
                        CounterForThings3 = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //else durumuna  player'ın korkusuna ve enemy'nin korkusuna 15 ekler.
                    }
                }
            }
            else if (gameModeIsHard == true)
            {
                randomNumber = Random.Range(1, 2);

                CounterForThings3 += Time.deltaTime;

                if (CounterForThings3 >= 200)
                {
                    if (randomNumber == 2)
                    {
                        Debug.Log("Random number: True " + randomNumber);
                        EnemyController.enemysAnger = 100;
                        CounterForThings3 = 0;
                        randomNumber = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: True" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //200 snde bir random sayı döndürür. Randomsayı 2 ye eşit ise; enemy'sFear 100 olur. 
                    }
                    else
                    {
                        myFear += 20;
                        EnemyController.enemysAnger += 20;
                        CounterForThings3 = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //else durumuna  player'ın korkusuna ve enemy'nin korkusuna 20 ekler.
                    }
                }
            }
        }  // sadece enemy ==100 yapar.

        /*  private void RandomEnemyandPlayersTings4Minute() // 4 dkde bir çalışır.
          {
              int randomNumber = 0;//üretilecek sayı

              int playerNumber = 0;
              int enemyNumber = 1;

              if (gameModeIsEasy == true)
              {
                  CounterForThings2 += Time.deltaTime;

                  if (CounterForThings2 >= 240) //4 dkde bir çalışır.
                  {
                      randomNumber = Random.Range(1, 10);  //random ==2 ise
                      Debug.Log("Random number for game mode easy: " + "(4 minute)     " + randomNumber);

                      if (randomNumber == 2 && playerNumber == 0 && enemyNumber == 1)
                      {
                          EnemyController.enemysAnger = 100;

                          playerNumber = 1;
                          enemyNumber = 0;

                          CounterForThings2 = 0;
                      }
                      else if (randomNumber != 2 && playerNumber == 0 && enemyNumber == 1)
                      {
                          CounterForThings2 = 0;
                      }

                      if (randomNumber == 2 && playerNumber == 1 && enemyNumber == 0)
                      {
                          myFear = 100;

                          playerNumber = 0;
                          enemyNumber = 1;

                          CounterForThings2 = 0;
                      }
                      else if (randomNumber != 2 && playerNumber == 1 && enemyNumber == 0)
                      {

                      }
                  }
              }
              else if (gameModeIsNormal == true)
              {
                  CounterForThings2 += Time.deltaTime;

                  if (CounterForThings2 >= 210)  // 3.5 dkde bir çalışır.
                  {
                      randomNumber = Random.Range(1, 6);  //random ==2 ise
                      Debug.Log("Random number for game mode normal: " + "(3.5 minute)     " + randomNumber);

                      if (randomNumber == 2 && playerNumber == 0 && enemyNumber == 1)
                      {
                          EnemyController.enemysAnger = 100;

                          playerNumber = 1;
                          enemyNumber = 0;

                          CounterForThings2 = 0;
                      }
                      else if (randomNumber != 2 && playerNumber == 0 && enemyNumber == 1)
                      {
                          CounterForThings2 = 0;
                      }

                      if (randomNumber == 2 && playerNumber == 1 && enemyNumber == 0)
                      {
                          myFear = 100;

                          playerNumber = 0;
                          enemyNumber = 1;

                          CounterForThings2 = 0;
                      }
                      else if (randomNumber != 2 && playerNumber == 1 && enemyNumber == 0)
                      {
                          CounterForThings2 = 0;
                      }
                  }
              }
              else if (gameModeIsHard == true)
              {
                  CounterForThings2 += Time.deltaTime;

                  if (CounterForThings2 >= 180) // 3dkde bir
                  {
                      randomNumber = Random.Range(1, 4);  //random ==2 ise
                      Debug.Log("Random number for game mode hard: " + "(3 minute)     " + randomNumber);

                      if (randomNumber == 2 && playerNumber == 0 && enemyNumber == 1)
                      {
                          EnemyController.enemysAnger = 100;

                          playerNumber = 1;
                          enemyNumber = 0;

                          CounterForThings2 = 0;
                      }
                      else if (randomNumber != 2 && playerNumber == 0 && enemyNumber == 1)
                      {
                          CounterForThings2 = 0;
                      }

                      if (randomNumber == 2 && playerNumber == 1 && enemyNumber == 0)
                      {
                          myFear = 100;

                          playerNumber = 0;
                          enemyNumber = 1;

                          CounterForThings2 = 0;
                      }
                      else if (randomNumber != 2 && playerNumber == 1 && enemyNumber == 0)
                      {
                          CounterForThings2 = 0;
                      }
                  }
              }
          }
        */
        private void potion()       //korkuyu 10 azaltmak için pot içilir.
        {
            if (myFear == 0)        //!!! yazılan fonksyon daha verimli ve düzgün yazılabilir gözden geçir!!!
            {
                // you can not drink potion     // can 0 olduğu için boşuna bot öğesi harcanmasını önler.
            }
            else if (myFear <= 24)
            {
                myFear = myFear - 10;

                if (myFear < 0)     // korku eksiye düşmesin diye alınmış önlem
                {
                    myFear = 0;
                }
            }
            else if (myFear >= 25 && myFear <= 49)
            {
                if (drinkPotion == true)
                {
                    myFear = myFear - 10;

                    if (myFear <= 24)
                    {
                        myFearLimitMin = 0;     // myFearMinLimit fearActionsda bir kere belirtilmiş. Burada 2.defa tanımlanmış
                                                // if my fear>=25 my minlimit==25 olmalı, burada yanlış tanımlanmış. Gerekmediğitaktirde buradaki tanımlamalrı kaldır.
                    }
                }
            }
            else if (myFear >= 50 && myFear <= 69)
            {
                myFear = myFear - 10;

                if (myFear <= 49)
                {
                    myFearLimitMin = 25;
                }
            }
            else if (myFear >= 70 && myFear <= 84)
            {
                myFear = myFear - 10;

                if (myFear <= 69)
                {
                    myFearLimitMin = 50;
                }
            }
            else if (myFear >= 85 && myFear <= 99)
            {
                myFear = myFear - 10;

                if (myFear <= 84)
                {
                    myFearLimitMin = 70;
                }
            }
            else if (myFear == 100)
            {
                // you can not drink potion.
            }   // pot kullandıkça enventerden pot çıkar. sonrasında pot+1 yapılıp eski haline döndrülecek.
            else if (true)
            {
                //yorum
            }
        }
        #endregion
    }
}
