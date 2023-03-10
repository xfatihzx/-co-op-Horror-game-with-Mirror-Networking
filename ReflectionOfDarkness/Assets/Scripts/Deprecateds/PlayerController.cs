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
            if (Input.GetKeyDown(KeyCode.K))   // d??zenlenecek
            {
                int randomNumber = Random.Range(1, 25);
                Debug.Log(randomNumber + "random number");

                if (randomNumber == 7)
                {
                    Debug.Log(randomNumber + "    " + "??ay var simit var neye bak??yon.");
                }
            }
            #endregion random say?? d??nd??rme. 
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
            if (Input.GetKeyDown(KeyCode.F))  // a?? kapa fonksyonlar?? i??in f tu??u atan??r.
            {
                openFlash = !openFlash;     //oyun ba??lad??????nda fener kapal?? olarak atan??r.
                if (openFlash)
                {
                    FindObjectOfType<FlashlightManager>().Switch(true);     //fener player'??n elindeyse a?? kapa fonksyonlar??n?? kullanabilir.

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
            if (character.isGrounded)       //player'??n yerde olup olmad??????n?? kontrol eder.
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
                    // yava??latma ve ses efekti burada yap??lacak 
                }

                fallTime = 0;       //yere temas edince saya???? s??f??rlar.
            }
            else
            {
                fallTime += Time.deltaTime;     // player'??n havadaki s??resini ??l??er.
            }
        }

        #region Player all fear things      //player'??n t??m korku i??lemleri bu alanda yap??l??r
        private void isPlayerHasFear()      //
        {
            if (scares.inFearTrigger == true)  //limit de??eri updete i??inde tan??mland??.     //enemeyScareController'daki onTrigeerEnter metodunu kontrol eder.
            {
                fearCounterPlas += Time.deltaTime;

                if (fearCounterPlas >= 1) //s??re 5 olarak de??i??tirilecek
                {
                    myFear++;
                    myFearPlasBool = true;          //normalde bu kod yap??s?? gereksiz ama onTriggerExit update'de hata verdi??i i??in kullan??l??r.

                    fearCounterPlas = 0;
                    fearCounterMinus = 0;           //trigger true oldupunda saya?? ba??lar. saya?? if'i kar????lad??????nda saya?? s??f??rlan??r ve korku 1 artar.
                }
            }
            else if (scares.inFearTrigger == false)
            {
                if (myFearPlasBool == true)     //korku en az 1 kere artm???? olmal??. 0 a e??it olmamal??.
                {
                    fearCounterMinus += Time.deltaTime;

                    if (myFear == 0)
                    {
                        myFearPlasBool = false;   //gereksiz kod sadece 0 olunca sayac??n ??al????may?? b??rakmas?? i??in yaz??ld??.
                    }

                    if (fearCounterMinus >= 1) //s??re 5 olarak de??i??tirilecek
                    {
                        myFear--;

                        fearCounterMinus = 0;
                        fearCounterPlas = 0;        //trigger true oldupunda saya?? ba??lar. saya?? if'i kar????lad??????nda saya?? s??f??rlan??r ve korku 1 azal??r.
                    }
                }
            }               //!!! enter trigger ??al????t??????nda exit triggerin sayac?? s??f??rlanmaz.!!! //!!! exit trigger ??al????t??????nda enter triggerin sayac?? s??f??rlanmaz.!!!
        }
        //player fear actionsda korkunun belli miktarda d??????????n?? ayarla. ??rn: 35 ise 25 e kadar d????er. 25den 0 a indirmek i??in b??y?? yapmas?? yada bir objeyle etkile??ime girmesi gerekir.

        private void FearValueForActions()  // for player   //player'??n korkusuna g??re ??al????an bool'ler yer al??r.
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
                    myFearLimitMin = 25;            //korku belli bir seviyeyi ge??ince min limit atan??r. Atanan limitin alt??na inmesi i??in oyun i??i g??rev gerekir(potion dirinking).
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

                myFearLimitMin = 50;    //50 nin alt??na d????mesi i??in b??y?? gerekiyor.    b??y?? ile 50 nin alt??na inmesini sa??layacak g??revi olu??tur.

                objectInteractionIs50percent = true;  // obje ile etkile??ime ge??me ??ans?? y??zde 50 olur.
            }//korku belli bir seviyeyi ge??ince min limit atan??r. Atanan limitin alt??na inmesi i??in oyun i??i g??rev gerekir(potion dirinking).
            else if (myFear >= 70 && myFear <= 84 && myFearLimitMin <= 70)
            {
                interaction.isPlayerFear = true;
                fearIs50 = false;
                fearIs70 = true;
                fearIs85 = false;
                fearIs100 = false;   //70 ve 85 de hata veriyor hatay?? ????z.

                myFearLimitMin = 70;

                objectInteractionIs30percent = true;
                objectInteractionIs50percent = false;
                // obje ile etkile??ime girme ??ans?? y??zde 30 olacak.

                //Debug.Log("I scary (70)");
            }//korku belli bir seviyeyi ge??ince min limit atan??r. Atanan limitin alt??na inmesi i??in oyun i??i g??rev gerekir(potion dirinking).
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
                //obje ile etkile??ime girme ??ans?? y??zde 20.

                //Debug.Log("I Really scary(85)");
            }   //korku belli bir seviyeyi ge??ince min limit atan??r. Atanan limitin alt??na inmesi i??in oyun i??i g??rev gerekir(potion dirinking).
            else if (myFear == 100 && myFearLimitMin <= 100)   // min de??er 100 olamayaca???? i??in 85 olarak ayarland??.
            {
                interaction.isPlayerFear = true;
                fearIs50 = false;       //obje etkile??imleri 15 sb duracak ve 15 sn korku 100 olacak sonra 85 e d????ecek.
                fearIs70 = false;
                fearIs85 = false;
                fearIs100 = true;

                myFearLimitMin = 100; // fear 100 oldu??unda 15 sn beklemek zorunldur. iksir i??erek ka????n??lamaz.

                objectInteractionIsImpossible = true;
                objectInteractionIs50percent = false;
                objectInteractionIs30percent = false;
                objectInteractionIs20percent = false;
                //objeler ile 15 sn etkile??ime ge??ilemez.

                if (objectInteractionIsImpossible == true)
                {
                    // korku 100 oldu??unda bir saya?? ??al??s??r ve 15 snden geriye sayar.
                    // bu s??re boyunca hi??bir obje ile etkile??ime girilemez. s??renin sonunda hala y??z olup olmad?????? kontrol edilir.

                    StartCoroutine(WaitForFear15second());      //private IEnumerator WaitForFear15second()'a g??nderir. 380.sat??r.
                                                                //Debug.Log("Start coroutine is work" + "    " + "you have to wait 15 secend");
                }
                if (Logger.IsEnabled(LogLevel.Info))
                    Logger.@do.Info("I want to go home(100)");
            }
        }

        /// <summary>
        /// Korku 100 olunca 15 saniye boyunca 100 kalmas?? i??in ??al??????r.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForFear15second()
        {
            yield return new WaitForSeconds(15);        //15 sn bekler.

            objectInteractionIsImpossible = false;      // 15 sn'nin ard??ndan obje ile tekrar etkile??ime ge??ilebilir.

            myFear = myFear - 5;

            myFearLimitMin = 85;                        // korku 95 e e??itlenip min limit 85 e atan??r. karakter iksir i??erek azaltmak zorundad??r.
        } //fear action i??in kullan??l??yor.

        /// <summary>
        /// InteractionController i??inde kontrol i??in yap??ld??.
        /// Karater korkuyorken etkile??ime ge??me i??lemine ihtimal katmak i??in.
        /// </summary>
        /// <returns>Etkile??im ihtimalini boolean olarak d??nd??r??r.</returns>
        public bool PlayerInteractPossibilityForFear()
        {
            bool isLucky = false;//etkile??ime ge??ecek kadar ??ansl?? m?? ?

            int luckyNumber = 2;//??ansl?? say??
            int randomNumber = 0;//??retilecek say??

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
                randomNumber = 1;//bilerek 1 verdim ????nk?? etkile??ime ge??emesin diye.
            }
            else//e??er 50 'yi daha ge??mediyse direk luckyNumber al diyorum. 100% i??lem yapabilmesi i??in 
            {
                randomNumber = luckyNumber;
            }

            if (randomNumber == luckyNumber)
            {
                isLucky = true;//wow, ??ansl?? say??y?? tutturdu. Bu player ??ansl??ym????.
            }

            return isLucky;
        }

        /* private void RandomEnemyandPlayersTings30scond()  //30 snde bir ??al??s??r. i??indeki rakamlar easy=10, normal = 15, hard =20. rakamlar de??i??tirilebilir.
         {
             int randomNumber = 0;//??retilecek say??

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
                     else if (randomNumber == 2) // ilk say?? 2 d??nd??????nde 2.defa else ife girip de??er d??nd??r??yor.
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

        /* private void RandomEnemyandPlayersTings30scond()  //30 snde bir ??al??s??r. i??indeki rakamlar easy=10, normal = 15, hard =20. rakamlar de??i??tirilebilir.
         {
             int randomNumber = 0;//??retilecek say??

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

        private void RandomEnemyandPlayersTings30scond() // tam tersi korku ve anger azaltan fonk yada yoz. sadece game==true i??in
        {
            int randomNumber = 0;//??retilecek say??

            if (gameModeIsEasy == true)
            {
                randomNumber = Random.Range(1, 7); // (1,7), (1,6), (1,5);

                CounterForThings += Time.deltaTime; // int olarak nas??l yazd??r??rl??r ara??t??r.

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

                        //saya?? 30 snden b??y??k olunca ve oyun modu easy ise random say?? d??nd??r??l??r. Random say?? 2 ye e??itse korku ve ebemy anger 5 artar.
                    }
                    else
                    {
                        myFear++;
                        EnemyController.enemysAnger++;
                        CounterForThings = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random say?? 2 ye e??it de??ilse player'??nkorkusy ve enemy'nin ??fkesi 1 artar.
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

                        //saya?? 30 snden b??y??k olunca ve oyun modu normal ise random say?? d??nd??r??l??r. Random say?? 2 ye e??itse korku ve ebemy anger 10 artar.
                    }
                    else
                    {
                        myFear = myFear + 2;
                        EnemyController.enemysAnger = EnemyController.enemysAnger + 2;
                        CounterForThings = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random say?? 2 ye e??it de??ilse player'??nkorkusy ve enemy'nin ??fkesi 2 artar.
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

                        //saya?? 30 snden b??y??k olunca ve oyun modu hard ise random say?? d??nd??r??l??r. Random say?? 2 ye e??itse korku ve ebemy anger 15 artar.
                    }
                    else
                    {
                        myFear = myFear + 4;
                        EnemyController.enemysAnger = EnemyController.enemysAnger + 4;
                        CounterForThings = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random say?? 2 ye e??it de??ilse player'??nkorkusy ve enemy'nin ??fkesi 4 artar.
                    }
                }
            }
        }
        //enemy ve my fear 100 ge??memeli kontrollerini yap.
        private void RandomPlayersTings3Minute()// 180sn dkde bir ??al??????r. 180sn(1,4), 170sn (1,3), 155sn(1,2)  enemy anger ve my fear da artt??r??r.
        {
            int randomNumber = 0;//??retilecek say??

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

                        //game mode easy ve random number e??ittir 2 ise; player'??n korkusu 100 olur, enemy'nin korkusuna 10 eklenir.
                    }
                    else
                    {
                        myFear += 10;
                        EnemyController.enemysAnger += 10; // else de her 3 4 dkde bir d??nd?????? i??in +10 gibi rakam eklenebilir.
                        CounterForThings2 = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random number 2 ye e??it de??ilse; player'??nkorkusu ve enemy'nin ??fkesi 10 artar.
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

                        //game mode normal ve random number e??ittir 2 ise; player'??n korkusu 100 olur, enemy'nin korkusuna 15 eklenir.
                    }
                    else
                    {
                        myFear += 15;
                        EnemyController.enemysAnger += 15;
                        CounterForThings2 = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random number 2 ye e??it de??ilse; player'??nkorkusu ve enemy'nin ??fkesi 15 artar.
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

                        //game mode hard ve random number e??ittir 2 ise; player'??n korkusu 100 olur, enemy'nin korkusuna 30 eklenir.
                    }
                    else
                    {
                        myFear += 20;
                        EnemyController.enemysAnger += 20;
                        CounterForThings2 = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //Random number 2 ye e??it de??ilse; player'??nkorkusu ve enemy'nin ??fkesi 20 artar.
                    }
                }
            }
        }

        private void RandomEnemyThings4Minute()
        {
            int randomNumber = 0;//??retilecek say??

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

                        //240 snde bir random say?? d??nd??r??r. Randomsay?? 2 ye e??it ise; enemy'sFear 100 olur.
                    }
                    else
                    {
                        myFear += 10;
                        EnemyController.enemysAnger += 10;  // 10 15 20
                        CounterForThings3 = 0;
                        Debug.Log("Game mode easy: " + gameModeIsEasy + "Random number: Fasle" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);


                        //else durumuna  player'??n korkusuna ve enemy'nin korkusuna 10 ekler.
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

                        //220 snde bir random say?? d??nd??r??r. Randomsay?? 2 ye e??it ise; enemy'sFear 100 olur. 
                    }
                    else
                    {
                        myFear += 15;
                        EnemyController.enemysAnger += 15;
                        CounterForThings3 = 0;
                        Debug.Log("Game mode normal: " + gameModeIsNormal + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //else durumuna  player'??n korkusuna ve enemy'nin korkusuna 15 ekler.
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

                        //200 snde bir random say?? d??nd??r??r. Randomsay?? 2 ye e??it ise; enemy'sFear 100 olur. 
                    }
                    else
                    {
                        myFear += 20;
                        EnemyController.enemysAnger += 20;
                        CounterForThings3 = 0;
                        Debug.Log("Game mode hard: " + gameModeIsHard + "Random number: False" + randomNumber + "my fear:" + myFear + "enemy anger: " + EnemyController.enemysAnger);

                        //else durumuna  player'??n korkusuna ve enemy'nin korkusuna 20 ekler.
                    }
                }
            }
        }  // sadece enemy ==100 yapar.

        /*  private void RandomEnemyandPlayersTings4Minute() // 4 dkde bir ??al??????r.
          {
              int randomNumber = 0;//??retilecek say??

              int playerNumber = 0;
              int enemyNumber = 1;

              if (gameModeIsEasy == true)
              {
                  CounterForThings2 += Time.deltaTime;

                  if (CounterForThings2 >= 240) //4 dkde bir ??al??????r.
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

                  if (CounterForThings2 >= 210)  // 3.5 dkde bir ??al??????r.
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
        private void potion()       //korkuyu 10 azaltmak i??in pot i??ilir.
        {
            if (myFear == 0)        //!!! yaz??lan fonksyon daha verimli ve d??zg??n yaz??labilir g??zden ge??ir!!!
            {
                // you can not drink potion     // can 0 oldu??u i??in bo??una bot ????esi harcanmas??n?? ??nler.
            }
            else if (myFear <= 24)
            {
                myFear = myFear - 10;

                if (myFear < 0)     // korku eksiye d????mesin diye al??nm???? ??nlem
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
                        myFearLimitMin = 0;     // myFearMinLimit fearActionsda bir kere belirtilmi??. Burada 2.defa tan??mlanm????
                                                // if my fear>=25 my minlimit==25 olmal??, burada yanl???? tan??mlanm????. Gerekmedi??itaktirde buradaki tan??mlamalr?? kald??r.
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
            }   // pot kulland??k??a enventerden pot ????kar. sonras??nda pot+1 yap??l??p eski haline d??ndr??lecek.
            else if (true)
            {
                //yorum
            }
        }
        #endregion
    }
}
