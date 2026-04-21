using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   public void OpenMainScene()
   {
      SceneManager.LoadScene(0, LoadSceneMode.Single);
   }

   public void OpenGameScene()
   {
      SceneManager.LoadScene(1, LoadSceneMode.Additive);
   }
}
