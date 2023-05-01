using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "Player Input")]
public class PlayerInput : ScriptableObject, GameControls.IPlayerActions, GameControls.IMenusActions
{
	// ++++ Player ++++

	public Vector3 RawMovementInput { get; private set; }

	public UnityAction crouchEvent;
	public UnityAction crouchCanceledEvent;
	public UnityAction jumpEvent;
	public UnityAction jumpCanceledEvent;
	public UnityAction interactEvent;
	public UnityAction pauseEvent;
	public UnityAction<Vector2> moveEvent;
	public UnityAction<Vector2> cameraLookEvent;

	// ++++ Menus ++++

	public UnityAction unPauseEvent;
	public UnityAction cancelEvent;
	public UnityAction submitEvent;
	public UnityAction<Vector2> navigateEvent;

	private GameControls gameControls;

	private void OnEnable()
	{
		if (gameControls == null)
		{
			gameControls = new GameControls();

			gameControls.Player.SetCallbacks(this);
			gameControls.Menus.SetCallbacks(this);
		}
	}

	private void OnDisable()
	{
		DisableAllInput();
	}

	public void EnablePlayerInput()
	{
		gameControls.Menus.Disable();

		gameControls.Player.Enable();
	}

	public void EnableMenusInput()
	{
		gameControls.Player.Disable();

		gameControls.Menus.Enable();
	}

	public void DisableAllInput()
	{
		gameControls.Player.Disable();
		gameControls.Menus.Disable();
	}

	public void OnCrouch(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			crouchEvent?.Invoke();

		if (context.phase == InputActionPhase.Canceled)
			crouchCanceledEvent?.Invoke();
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			interactEvent?.Invoke();
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			jumpEvent?.Invoke();

		if (context.phase == InputActionPhase.Canceled)
			jumpCanceledEvent?.Invoke();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		cameraLookEvent?.Invoke(context.ReadValue<Vector2>());
	}

	public void OnMove(InputAction.CallbackContext context)
	{
#if UNITY_EDITOR
		Debug.Log("Moving!!");
#endif
		Vector2 input = context.ReadValue<Vector2>();

		moveEvent?.Invoke(input);

		RawMovementInput = new Vector3(input.x, 0, input.y);
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			pauseEvent?.Invoke();
	}

	/*
	 * Menus
	 */

	public void OnUnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			unPauseEvent?.Invoke();
	}

	public void OnSubmit(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			submitEvent?.Invoke();
	}

	public void OnCancel(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			cancelEvent?.Invoke();
	}

	public void OnNavigate(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();

		navigateEvent?.Invoke(input);
	}

	public void OnPoint(InputAction.CallbackContext context)
	{
		
	}

	public void OnClick(InputAction.CallbackContext context)
	{
		
	}

	public void OnScrollWheel(InputAction.CallbackContext context)
	{
		
	}

	public void OnMiddleClick(InputAction.CallbackContext context)
	{
		
	}

	public void OnRightClick(InputAction.CallbackContext context)
	{
		
	}

	public void OnTrackedDevicePosition(InputAction.CallbackContext context)
	{
		
	}

	public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
	{
		
	}
}
