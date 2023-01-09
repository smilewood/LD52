using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesOnSlide : EffectOnSlideShown
{
   public ParticleSystem eyes;
   internal override void Trigger()
   {
      eyes.Play();
   }
}
