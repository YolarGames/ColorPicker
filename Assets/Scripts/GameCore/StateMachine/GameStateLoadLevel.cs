using Configs;
using GameCore.GameServices;
using GameCore.GameUI;
using UnityEngine;
using Utils.Constants;

namespace GameCore.StateMachine
{
	public sealed class GameStateLoadLevel : IGameStatePayloaded<SceneSets>
	{
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly FactoryService _factoryService;
		private readonly ProgressService _progressService;

		public GameStateLoadLevel(GameStateMachine stateMachine, LoadingScreen loadingScreen)
		{
			_stateMachine = stateMachine;
			_sceneLoader = new SceneLoader(loadingScreen);
			_factoryService = Services.FactoryService;
			_progressService = Services.ProgressService;
		}

		public async void Enter(SceneSets payload)
		{
			if (payload != SceneSets.MainMenu)
				_progressService.SaveLevel(payload);
			
			await _sceneLoader.LoadSceneSet(
				GetSceneContainer(payload),
				OnLevelLoaded);
			
			_stateMachine.EnterGameLoopState();
		}

		private void OnLevelLoaded() =>
			TryPlacePlayer();

		private void TryPlacePlayer()
		{
			if (TryFindRespawn(out Transform respawn))
				_factoryService.CreatePlayer(respawn.position);
			else
				Debug.Log("Can't find player respawn");
		}

		private static bool TryFindRespawn(out Transform respawn) =>
			(respawn = GameObject.FindGameObjectWithTag(Tags.Respawn).transform) != null;

		public void Update()
		{
		}

		public void Exit()
		{
		}

		private SceneContainer GetSceneContainer(SceneSets sceneSet) =>
			Services.ConfigService.scenesConfig.GetSceneContainerWithSet(sceneSet);
	}
}