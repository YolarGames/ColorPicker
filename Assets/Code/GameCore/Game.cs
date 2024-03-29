using System;
using Configs;
using GameCore.GameServices;
using GameCore.GameUI;
using GameCore.StateMachine;
using Logger;
using UnityEditor;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace GameCore
{
	public sealed class Game
	{
		public static readonly GameLogger GameLogger = new();
		private readonly LoadingScreen _loadingScreen;
		private readonly ICoroutineRunner _coroutineRunner;
		private GameStateMachine _gameStateMachine;
		private Services _services;

		public static bool IsPaused { get; private set; }

		public Game(GameSetupParameters parameters, LoadingScreen loadingScreen, ICoroutineRunner coroutineRunner)
		{
			SetupCheatWindow(parameters.EnableCheats);
			SetFrameRate(parameters.Fps.ToString());

			GameLogger.SwitchAllLogActivity(parameters.EnableLogger);

			_coroutineRunner = coroutineRunner;
			_loadingScreen = loadingScreen;

			StartGameFlow();
		}

		private static void SetupCheatWindow(bool isOn)
		{
			var cheatWindow = Object.FindFirstObjectByType<CheatWindow>();

			if (!isOn)
				cheatWindow.gameObject.SetActive(false);

			cheatWindow.CreateToggle("Game pause", false, SetPause);
			cheatWindow.CreateInputInt("Frame rate", Application.targetFrameRate, SetFrameRate);
		}

		public static void Quit()
		{
#if UNITY_EDITOR || UNITY_EDITOR_64
			EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
		}

		public static void SetPause(bool value)
		{
			IsPaused = value;
			Time.timeScale = value ? 0 : 1;
		}

		private static void SetFrameRate(string value) =>
			Application.targetFrameRate = value.IsEmpty() ? -1 : Convert.ToInt32(value);

		private async void StartGameFlow()
		{
			_services = new Services();
			await _services.InitServices();

			_gameStateMachine = new GameStateMachine(_loadingScreen, _coroutineRunner);
			_gameStateMachine.EnterBootstrapState();
		}
	}
}