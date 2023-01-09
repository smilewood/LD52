using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlideshowText : MonoBehaviour
{
   public TMP_Text[] text;
   public float fadeTime;
   public float delay;
   // Start is called before the first frame update
   void Start()
   {
      foreach (TMP_Text t in text)
      {
         t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
      }
   }

   public IEnumerator FadeInText()
   {
      foreach(TMP_Text t in text)
      {
         float time = 0;
         while(time < fadeTime)
         {
            t.color = new Color(t.color.r, t.color.g, t.color.b, time/fadeTime);
            time += Time.deltaTime;
            yield return null;
         }
         yield return new WaitForSeconds(delay);
      }
   }

   internal void SkipFade()
   {
      foreach (TMP_Text t in text)
      {
         t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
      }
   }
}
