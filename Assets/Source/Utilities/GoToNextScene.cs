using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class GoToNextScene : MonoBehaviour
    {
        public int SceneToGo;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (SceneToGo <= SceneManager.sceneCount + 1)
            {
                SceneManager.LoadSceneAsync(SceneToGo);
            }
        }
    }
}


