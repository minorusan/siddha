using Core.Map;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core.Gameplay
{
	public class GamePlay : MonoBehaviour
	{
		public void RestartLevel ()
		{
            var nonWalkables = FindObjectsOfType<NonWalkable>();
            foreach (var item in nonWalkables)
            {
                item.Active = false;
            }

			SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
		}
	}

}
