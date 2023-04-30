using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, GameControls.IPlayerActions
{
	public Vector3 RawMovementInput { get; private set; }

	public UnityAction crouchEvent;
	public UnityAction crouchCanceledEvent;
	public UnityAction jumpEvent;
	public UnityAction jumpCanceledEvent;
	public UnityAction interactEvent;
	public UnityAction pauseEvent;
	public UnityAction inventoryEvent;
	public UnityAction<Vector2> moveEvent;
	public UnityAction<Vector2> cameraLookEvent;

	GameControls gameControls;

	void OnEnable()
	{
		if (gameControls == null)
		{
			gameControls = new GameControls();
			gameControls.Player.SetCallbacks(this);
		}
		gameControls.Player.Enable();
	}

	void OnDisable()
	{
		if (gameControls != null)
		{
			gameControls.Player.RemoveCallbacks(this);
		}
		gameControls.Player.Disable();
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
		Vector2 input = context.ReadValue<Vector2>();

		moveEvent?.Invoke(input);

		RawMovementInput = new Vector3(input.x, 0, input.y);
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
			pauseEvent?.Invoke();
	}

	public void OnInventory(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started) inventoryEvent?.Invoke();
	}
}
