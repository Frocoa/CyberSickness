using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
   public static StateManager Instance { get; private set; }

   public enum LocomotionTechnique
   {
      None,
      Walk,
      Swing,
      Hook,
      Propel,
   }


   public LocomotionTechnique SelectedLocomotion { get; set; }
   

   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
      }
      else
      {
         Instance = this;
      }
      DontDestroyOnLoad(gameObject);
   }

   public void SetSelectedLocomotion(string locomotion)
   {
      SelectedLocomotion = (LocomotionTechnique) System.Enum.Parse(typeof(LocomotionTechnique), locomotion);
   }
   
   public void LoadScene(string sceneName)
   {
      SceneManager.LoadScene(sceneName);
   }
}
