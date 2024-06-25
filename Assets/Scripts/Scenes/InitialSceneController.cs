using UnityEngine;
using UnityEngine.SceneManagement;

namespace JustChess.Scenes
{
    public class InitialSceneController : SceneController
    {
        [SerializeField] private string sceneToLoad;

        private void Start()
        {
            SceneManager.LoadSceneAsync(sceneToLoad);
        }
    }
}
