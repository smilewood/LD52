using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
   private static Dictionary<string, GameObject> menus;
   public List<GameObject> Menus;
   public AudioMixer mixer;
   
   private static MenuFunctions _instance;
   public static MenuFunctions Instance
   {
      get
      {
         return _instance;
      }
   }
   private void Awake()
   {
      if (menus is null)
      {
         menus = new Dictionary<string, GameObject>();
      }
      if (_instance is null)
      {
         _instance = this;
      }
      foreach (GameObject go in Menus)
      {
         menus.Add(go.name, go);
      }
      SceneManager.sceneUnloaded += OnSceneUnload;
   }

   private void OnSceneUnload(Scene scene)
   {
      menus.Clear();
   }

   public static void ExitGame()
   {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
   }

   public static void StartLevel(string level)
   {
      ResumeGame();
      SceneManager.LoadScene(level);
   }

   private static float preMuteVolume = 100;
   public static void Mute()
   {
      if (AudioListener.volume == 0)
      {
         AudioListener.volume = preMuteVolume;
      }
      else
      {
         preMuteVolume = AudioListener.volume;
         AudioListener.volume = 0;
      }
   }

   public void SetMasterVolume(float setting)
   {
      mixer.SetFloat("MasterVolume", Mathf.Log10(setting) * 20);
   }

   public void SetMusicVolume(float setting)
   {
      mixer.SetFloat("MusicVolume", Mathf.Log10(setting) * 20);
   }

   public static void PauseGame()
   {
      Time.timeScale = 0;
   }

   public static void ResumeGame()
   {
      Time.timeScale = 1;
   }

   public static void RestartLevel()
   {
      ResumeGame();
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }

   public void ShowMenu(string menu)
   {
      if (menus.ContainsKey(menu))
      {
         menus[menu].SetActive(true);
      }
   }

   public void HideMenu(string menu)
   {
      if (menus.ContainsKey(menu))
      {
         menus[menu].SetActive(false);
      }
   }

}
