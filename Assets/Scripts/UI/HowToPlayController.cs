using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class HowToPlayController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("LevelSelect");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}