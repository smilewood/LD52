using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
   private static AudioManager instance;
   public static AudioManager Instance
   {
      get { return instance; }
   }

   private void Awake()
   {
      if(instance != null)
      {
         Destroy(this.gameObject);
      }
      else
      {
         DontDestroyOnLoad(this);
         instance = this;
      }
   }

   public AudioSource BackgroundSource;
   public AudioSource S1, S2;

   public AudioMixer Mixer;
   public void PlayMusic(AudioClip Music, bool fadeIn = true)
   {
      StopAllCoroutines();
      if (BackgroundSource.isPlaying)
      {
         StartCoroutine(FadeAudio("BackgroundVolume", .25f, 0, BackgroundSource));
      }

      if (S1.isPlaying)
      {
         StartCoroutine(FadeAudio("S1Volume", .25f, 0, S1, stopAtEnd: S1));
         S2.clip = Music;
         S2.Play();
         StartCoroutine(FadeAudio("S2Volume", fadeIn ? .25f : 0.001f, 1));
      }
      else
      {
         StartCoroutine(FadeAudio("S2Volume", .25f, 0, S2, stopAtEnd: S2));
         S1.clip = Music;
         S1.Play();
         StartCoroutine(FadeAudio("S1Volume", .25f, 1));
      }
   }

   public void PlayMusicThenBackground(AudioClip Music)
   {
      PlayMusic(Music);
      StartCoroutine(WaitThenPlay(Music.length));

      IEnumerator WaitThenPlay(float time)
      {
         yield return new WaitForSeconds(time - .5f);
         BackgroundSource.UnPause();
         StartCoroutine(FadeAudio("BackgroundVolume", .25f, 1));
      }
   }

   private IEnumerator FadeAudio(string group, float duration, float targetVolume, AudioSource pauseAtEnd = null, AudioSource stopAtEnd = null)
   {
      float currentTime = 0;
      Mixer.GetFloat(group, out float currentVol);
      currentVol = Mathf.Pow(10, currentVol / 20);
      float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
      while (currentTime < duration)
      {
         currentTime += Time.deltaTime;
         float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
         Mixer.SetFloat(group, Mathf.Log10(newVol) * 20);
         yield return null;
      }
      if(pauseAtEnd != null)
      {
         pauseAtEnd.Pause();
      }
      if (stopAtEnd != null)
      {
         pauseAtEnd.Stop();
      }
   }

   public void PlayBackground()
   {
      StopAllCoroutines();

      StartCoroutine(FadeAudio("S1Volume", .25f, 0, S1, stopAtEnd: S1));
      StartCoroutine(FadeAudio("S2Volume", .25f, 0, S2, stopAtEnd: S2));
      StartCoroutine(FadeAudio("BackgroundVolume", .25f, 1));
   }
}
