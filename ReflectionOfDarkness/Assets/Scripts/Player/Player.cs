using Mirror;
using ReflectionOfDarknes.Player.Dynamic;
using ReflectionOfDarknes.Player.Controller;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Assets.Scripts.Player
{
    public class Player : NetworkBehaviour
    {
        [SerializeField, OnInspector(Tooltip = "(Firt Person Shooter)", Comment = "Karakter hareketi ve kamera hareketi", ReadOnly = true)]
        private FPS fps;

        [SerializeField, OnInspector(Tooltip = "New Input System", Comment = "Hareket ve kamera girişlerin yeni sistemi", ReadOnly = true)]
        private PlayerInput inputHandler;

        [SerializeField, OnInspector(Comment = "Yeni giriş sistemi orta katman class'ı", ReadOnly = true)]
        private PlayerInputCatcher inputCatcher;

        [SerializeField, OnInspector(ReadOnly = true)]
        private CharacterController controller;

        private void OnValidate()
        {
            fps = gameObject.GetComponent<FPS>();
            inputHandler = gameObject.GetComponent<PlayerInput>();
            inputCatcher = gameObject.GetComponent<PlayerInputCatcher>();
            controller = gameObject.GetComponent<CharacterController>();

            fps.enabled = false;
            inputHandler.enabled = false;
            controller.enabled = false;

            fps.enableLook = false;
            fps.enableMove = false;
        }

        public override void OnStartLocalPlayer()
        {
            controller.enabled = true;
            inputHandler.enabled = true;
            fps.enabled = true;

            fps.enableLook = true;
            fps.enableMove = true;

            GameObject cameraPosition = gameObject.transform.GetChild(0).gameObject;

            fps.cinemachineCameraTarget = cameraPosition;

            Tags.PlayerFollowCamera.FindWithTag().GetComponent<CinemachineVirtualCamera>().Follow = cameraPosition.transform;
        }
        private void Update()
        {
            if (!isLocalPlayer || controller == null || !controller.enabled)
                return;

            fps.JumpAndGravity(inputCatcher);
            fps.GroundedCheck();
            fps.Move(inputCatcher, controller);
        }
        private void LateUpdate()
        {
            if (!isLocalPlayer || controller == null || !controller.enabled)
                return;

            fps.CameraRotation(inputHandler, inputCatcher);
        }
    }
}