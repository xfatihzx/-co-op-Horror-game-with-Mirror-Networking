using Mirror;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ReflectionOfDarknes.Player.Dynamic
{
	public class PlayerInputCatcher : MonoBehaviour
	{
		[Space(50)]
		[OnInspector(ReadOnly = true, Comment = "Oyuncu Hareketleri (�leri-Geri-Sa�a-Sola)")]
		public Vector2 move;
		[OnInspector(ReadOnly = true, Comment = "Kamera Hareketi (Yukar�-A�a��-Sa�a-Sola)")]
		public Vector2 look;
		[OnInspector(ReadOnly = true, Comment = "Oyuncu z�plad� m� ?")]
		public bool jump;
		[OnInspector(ReadOnly = true, Comment = "Oyuncu ko�uyor mu ?")]
		public bool sprint;

		[Space(50)]
		[OnInspector(ReadOnly = true, Comment = "Analog hareket a��k m� ?")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Space(50)]
		[OnInspector(ReadOnly = true, Comment = "�mle� kitlensin mi ?")]
		public bool cursorLocked = true;
		[OnInspector(ReadOnly = true, Comment = "�mle� y�n� bak�� y�n� m�")]
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#else
		// old input sys if we do decide to have it (most likely wont)...
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

#if !UNITY_IOS || !UNITY_ANDROID
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			//Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
#endif
	}
}