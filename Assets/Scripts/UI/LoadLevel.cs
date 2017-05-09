using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoadLevel : MonoBehaviour
    {
        [SerializeField] private string _levelName;

        public void Load()
        {
            Load(_levelName);
        }

        public void Load(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}