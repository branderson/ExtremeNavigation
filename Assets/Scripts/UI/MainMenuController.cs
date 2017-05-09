using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("LevelSelect");
        }

        public void HowToPlay()
        {
            SceneManager.LoadScene("HowToPlay");
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}