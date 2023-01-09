using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShowController : MonoBehaviour
{
   public List<GameObject> Slides;
   private Dictionary<GameObject, (SlideshowText, CanvasGroup)> slides;
   private int currentSlide = 0;

   public float fadeTime;

   // Start is called before the first frame update
   void Awake()
   {
      slides = Slides.ToDictionary((s) => s, (s) => (s.GetComponent<SlideshowText>(), s.GetComponent<CanvasGroup>()));
   }

   private bool playing;
   private bool started;
   private void Update()
   {
      if (!started)
      {
         return;
      }
      if (Input.GetButtonDown("Jump"))
      {
         if (currentSlide < slides.Count)
         {
            if (playing)
            {
               StopAllCoroutines();
               GameObject slide = Slides[currentSlide];
               slides[slide].Item1.SkipFade();
               slides[slide].Item2.alpha = 1;
               EffectOnSlideShown[] effects = slide.GetComponents<EffectOnSlideShown>();
               foreach (EffectOnSlideShown effect in effects)
               {
                  effect.Trigger();
               }
            }
            StartCoroutine(NextSlide());
         }
      }
   }
   public void StartShow()
   {
      started = true;
      foreach(CanvasGroup slide in slides.Values.Select(v => v.Item2))
      {
         slide.alpha = 0;
      }
      StartCoroutine(NextSlide());
   }

   private IEnumerator NextSlide()
   {
      playing = true;
      GameObject slide = Slides[currentSlide];
      ++currentSlide;
      yield return FadeInSlide(slides[slide].Item2);
      EffectOnSlideShown[] effects = slide.GetComponents<EffectOnSlideShown>();
      foreach (EffectOnSlideShown effect in effects)
      {
         effect.Trigger();
      }
      yield return slides[slide].Item1.FadeInText();
      playing = false;
   }

   private IEnumerator FadeInSlide(CanvasGroup slide)
   {
      float time = 0;
      while (time < fadeTime)
      {
         slide.alpha = time / fadeTime;
         time += Time.deltaTime;
         yield return null;
      }
   }
}
