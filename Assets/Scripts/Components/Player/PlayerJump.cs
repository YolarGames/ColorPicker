using System;
using GameCore.GameServices;
using UnityEngine;

namespace Components.Player
{
	[RequireComponent(typeof(GroundCheck), typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
	public class PlayerJump : MonoBehaviour
	{
		[SerializeField] private float _force;
		private Rigidbody2D _rigidbody2D;
		private GroundCheck _groundCheck;
		private InputService _inputService;
		private bool _canJump;
		public event Action<bool> CanJump;

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_groundCheck = GetComponent<GroundCheck>();
			_inputService = Services.InputService;
		}

		private void OnEnable() =>
			_inputService.OnJumpPressed += Jump;

		private void OnDisable() =>
			_inputService.OnJumpPressed -= Jump;

		private void Update()
		{
			SetCanJump(_groundCheck.IsGrounded);
			
			if (Input.GetKeyUp(KeyCode.Space))
				Jump();
		}

		private void Jump()
		{
			if (!_canJump)
				return;

			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.AddForce(Vector2.up * _force, ForceMode2D.Impulse);
		}

		private void SetCanJump(bool value)
		{
			if (_canJump == value)
				return;

			_canJump = value;
			CanJump?.Invoke(value);
		}
	}
}