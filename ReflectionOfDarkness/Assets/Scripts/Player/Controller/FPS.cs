using Mirror;
using ReflectionOfDarknes.Player.Dynamic;
using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ReflectionOfDarknes.Player.Controller
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FPS : MonoBehaviour
    {
        [Space(50)]
        [OnInspector(ReadOnly = true, Comment = "Oyuncuyu hareket ettirme olsun mu ?")]
        public bool enableMove = false;
        [OnInspector(ReadOnly = true, Comment = "Kameray� haraket ettirme olsun mu ?")]
        public bool enableLook = false;
        [OnInspector(ReadOnly = true, Comment = "Z�plama aktif olsun mu ?")]
        public bool enableJump = false;
        [OnInspector(ReadOnly = true, Comment = "Ko�ma aktif olsun mu ?")]
        public bool enableSprint = false;

        [Space(50)]
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Karakter hareket h�z� | m/s")]
        private float moveSpeed = 4.0f;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Karakter ko�ma h�z� | m/s")]
        private float sprintSpeed = 6.0f;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Karakter d�nme h�z�")]
        private float rotationSpeed = 1.0f;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "H�zlanma ve yava�lama h�z�")]
        private float speedChangeRate = 10.0f;

        [Space(50)]
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Oyuncunun z�plarkenki a��rl���")]
        private float jumpHeight = 1.2f;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Oyuncunun sahip oldu�u yer �ekimi kuvveti. The engine default is -9.81f")]
        private float gravity = -15.0f;

        [Space(50)]
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Tekrar z�plamas�n� �nler. E�er s�f�r yaparsan �st �ste z�plar.")]
        private float jumpTimeout = 0.1f;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "D��me durumuna d��me zaman�. Merdiven inerken kullan��l�")]
        private float fallTimeout = 0.15f;

        [Space(50)]
        [SerializeField, OnInspector(ReadOnly = true, Comment = "E�er karakter yerde ise. CharacterController ground �zelli�i de�ildir")]
        private bool grounded = true;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Engebeli zeminler i�in kullan��l�")]
        private float groundedOffset = -0.14f;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Oyuncunun yar� �ap�.")]
        private float groundedRadius = 0.5f;
        [SerializeField, OnInspector(ReadOnly = false, Comment = "Karakter hangi katmanlar� zemin olarak g�r�r.")]
        private LayerMask groundLayers;

        [Space(50)]
        [SerializeField, OnInspector(ReadOnly = true, Comment = "CinemaMachine kameras�n�n oyuncuyu takip edece�i lokasyon")]
        public GameObject cinemachineCameraTarget;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Kamera yukar�dan ne kadar a�� alabilece�i")]
        private float topClamp = 85.0f;
        [SerializeField, OnInspector(ReadOnly = true, Comment = "Kamera a�a��dan ne kadar a�� alabilece�i")]
        private float bottomClamp = -85.0f;

        // cinemachine
        private float cinemachineTargetPitch;

        // oyuncu
        private float speed;
        private float rotationVelocity;
        private float verticalVelocity;
        private readonly float terminalVelocity = 53.0f;

        // zaman a��m� de�erleri
        private float jumpTimeoutDelta;
        private float fallTimeoutDelta;

        private const float threshold = 0.01f;

        private void Start()
        {
            jumpTimeoutDelta = jumpTimeout;
            fallTimeoutDelta = fallTimeout;
        }
        public void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }
        public void CameraRotation(PlayerInput inputHandler, PlayerInputCatcher inputCatcher)
        {
            if (!enableLook) return;

            // if there is an input
            if (inputCatcher.look.sqrMagnitude >= threshold)
            {
                //Don't multiply mouse input by Time.deltaTime
                float deltaTimeMultiplier = inputHandler.currentControlScheme == "KeyboardMouse" ? 1.0f : Time.deltaTime;

                cinemachineTargetPitch += inputCatcher.look.y * rotationSpeed * deltaTimeMultiplier;
                rotationVelocity = inputCatcher.look.x * rotationSpeed * deltaTimeMultiplier;

                // clamp our pitch rotation
                cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

                // Update Cinemachine camera target pitch
                cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * rotationVelocity);
            }
        }
        public void Move(PlayerInputCatcher inputCatcher, CharacterController controller)
        {
            if (!enableMove) return;

            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = inputCatcher.sprint && enableSprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (inputCatcher.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = inputCatcher.analogMovement ? inputCatcher.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(inputCatcher.move.x, 0.0f, inputCatcher.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (inputCatcher.move != Vector2.zero)
            {
                // move
                inputDirection = transform.right * inputCatcher.move.x + transform.forward * inputCatcher.move.y;
            }

            // move the player
            controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        }
        public void JumpAndGravity(PlayerInputCatcher inputCatcher)
        {
            if (!enableMove) return;

            if (grounded && enableJump)
            {
                // reset the fall timeout timer
                fallTimeoutDelta = fallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }

                // Jump
                if (inputCatcher.jump && jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                // jump timeout
                if (jumpTimeoutDelta >= 0.0f)
                {
                    jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                jumpTimeoutDelta = jumpTimeout;

                // fall timeout
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }

                // if we are not grounded, do not jump
                inputCatcher.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }
        public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
        }
    }
}