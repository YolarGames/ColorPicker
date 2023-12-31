using GameCore.StateMachine;
using UnityEngine;
using Utils;
using Utils.Constants;

namespace Level
{
	public class LevelEnder : MonoBehaviour
	{
		[SerializeField] private SceneSets _sceneSetToLoad;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.IsPlayer())
				return;

			Destroy(other.gameObject);
			LoadNextLevel();
		}

		private void LoadNextLevel() =>
			GameStateMachine.Instance.EnterLoadLevelState(_sceneSetToLoad);
	}
}