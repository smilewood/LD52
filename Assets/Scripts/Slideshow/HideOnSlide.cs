using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnSlide : EffectOnSlideShown
{
   public GameObject target;

   internal override void Trigger()
   {
      target.SetActive(false);
   }
}
