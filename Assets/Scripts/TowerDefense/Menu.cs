
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Menu : MonoBehaviour
{

   public CanvasGroup CompleteLevel;
   private int count = 0;

   private static string last;
   void Start()
   {
    //  CompleteLevel.alpha = 0;
   }

   public static void Restart()
   {
        last = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("RestartLevel");
   }
   public void StartGame()
   {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
            count = 1;
        }
        else if(SceneManager.GetActiveScene().name == "MapOne")  
        {
         
            SceneManager.LoadScene("MapTwo"); 
            count = 2;
        }
        else if(SceneManager.GetActiveScene().name == "MapTwo" || SceneManager.GetSceneByBuildIndex(2).name == "MapTwo")
        {
            SceneManager.LoadScene("MapThree"); 
            count = 3;
        }
        else if(SceneManager.GetActiveScene().name == "MapThree")
        {
            SceneManager.LoadScene("Credits"); 
            count = 4;
        }
        else if(SceneManager.GetActiveScene().name == "Credits")
        {
            SceneManager.LoadScene("Menu"); 
            count = 0;
        }
   }

   


   public void Retry()
   {
      SceneManager.LoadScene(last);
   }

   public void Complete()
   {
     // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
      CompleteLevel.alpha = 1;
      
   }
}
