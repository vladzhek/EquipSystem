using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameController>();

            if (bootstrapper == null)
                SceneManager.LoadScene(0);
        }
    }
}