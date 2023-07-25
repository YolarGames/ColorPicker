using System;
using Cinemachine;
using GameCore.GameServices;
using GameCore.StateMachine;
using UnityEngine;

namespace Components.Level
{
	public class CinemachineSetup : MonoBehaviour
	{
		private CinemachineVirtualCamera _cmCamera;
		
		private void Awake() => 
			SetCameraFollowPlayer();

		private void OnEnable() => 
			Services.FactoryService.OnPlayerCreated += SetPlayerAsTarget;

		private void OnDisable() => 
			Services.FactoryService.OnPlayerCreated -= SetPlayerAsTarget;

		private void SetPlayerAsTarget(Transform player) => 
			_cmCamera.Follow = player;

		private void SetCameraFollowPlayer() => 
			_cmCamera = GetComponent<CinemachineVirtualCamera>();
	}
}
