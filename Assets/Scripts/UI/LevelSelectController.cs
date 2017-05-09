using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LevelSelectController : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}