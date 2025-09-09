using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void LoadStage1()
        {
            SceneManager.LoadScene("Stage1");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}