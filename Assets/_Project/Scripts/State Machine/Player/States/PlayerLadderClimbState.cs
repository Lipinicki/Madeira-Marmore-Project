using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderClimbState : PlayerBaseState
{
	public PlayerLadderClimbState(PlayerStateMachine stateMachine) : base(stateMachine)
	{
	}

	public override void Enter()
	{
		_stateMachine.PlayerInput.interactEvent += OnRelease;

		Vector3 offSetPos = new Vector3(
			_stateMachine.ActiveLadder.position.x,
			0,
			_stateMachine.ActiveLadder.position.z
			);
		_stateMachine.transform.position = offSetPos +
			_stateMachine.ActiveLadder.forward *
			_stateMachine.LadderStartOffsetFoward +
			_stateMachine.transform.up *
			_stateMachine.LadderStartOffsetHeight;
		_stateMachine.transform.rotation = Quaternion.LookRotation(-_stateMachine.ActiveLadder.transform.forward);
	}

	public override void FixedTick(float fixedDeltaTime)
	{
		HandleLadderClimb(fixedDeltaTime);
		ClampsHorizontalVelocity();
	}

	public override void Tick(float deltaTime)
	{
		CheckForLadder();
	}

	private void CheckForLadder()
	{
		Ray ray = new Ray(
					new Vector3(
					_stateMachine.transform.position.x,
					_stateMachine.transform.position.y + _stateMachine.RayCastOffset,
					_stateMachine.transform.position.z
					),
					_stateMachine.transform.forward
					);

		if (Physics.Raycast(ray, out RaycastHit hit, _stateMachine.RayCastMaxDistance, _stateMachine.LadderLayers, QueryTriggerInteraction.Ignore))
		{
			_stateMachine.ActiveLadder = hit.transform;
		}
		else
		{
			_stateMachine.ActiveLadder = null;
		}
	}

	public override void Exit()
	{
		_stateMachine.PlayerInput.interactEvent -= OnRelease;
	}

	private void HandleLadderClimb(float deltaTime)
	{
		if (_stateMachine.ActiveLadder == null)
		{
			ApplyForceToLeft();
			return;
		}

		Vector3 climbDirection = new Vector3(0f, _stateMachine.InputVector.z, 0f);

		_stateMachine.transform.Translate(climbDirection * _stateMachine.LadderClimbingSpeed * deltaTime);

		if (_stateMachine.IsGrounded())
		{
			OnRelease();
		}
	}

	private void ClampsHorizontalVelocity()
	{
		Vector3 xzVel = new Vector3(_stateMachine.MainRigidbody.velocity.x, 0, _stateMachine.MainRigidbody.velocity.z);
		Vector3 yVel = new Vector3(0, _stateMachine.MainRigidbody.velocity.y, 0);

		xzVel = Vector3.ClampMagnitude(xzVel, 0);

		_stateMachine.MainRigidbody.velocity = xzVel + yVel;
	}

	private void ApplyForceToLeft()
	{
		_stateMachine.MainRigidbody.AddForce(
			_stateMachine.transform.up * 
			_stateMachine.ForceToLeftLadder + 
			_stateMachine.transform.forward * 
			_stateMachine.ForceToLeftLadder, 
			ForceMode.Impulse
			);
		_stateMachine.SwitchCurrentState(new PlayerFallingState(_stateMachine));
	}

	private void OnRelease()
	{
		_stateMachine.ActiveLadder = null;
		_stateMachine.SwitchCurrentState(new PlayerFallingState(_stateMachine));
	}
}