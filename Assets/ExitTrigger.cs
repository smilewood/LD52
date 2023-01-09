using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
   private bool inZone;
   private GameObject text;
   public ScarecrowAI scarecrow;

   private void Start()
   {
      text = transform.Find("Text").gameObject;
      text.SetActive(false);
      inZone = false;
   }

   // Update is called once per frame
   void Update()
   {
      if (inZone)
      {
         if (Input.GetButtonDown("Jump"))
         {
            scarecrow.PauseScarecrow();

            MenuFunctions.Instance.ShowMenu("MorningMenu");
         }
      }
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         //show exit text and listen for spacebar
         inZone = true;
         text.SetActive(true);
      }
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
      if (collision.gameObject.CompareTag("Player"))
      {
         //hide exit text and stop listening for spacebar
         inZone = false;
         text.SetActive(false);
      }
   }
}
