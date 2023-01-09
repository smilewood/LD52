using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   // Update is called once per frame
   void Update()
   {
      if (Input.GetButtonDown("Cancel"))
      {
         MenuFunctions.PauseGame();
         MenuFunctions.Instance.ShowMenu("Pause");
      }
   }
}
