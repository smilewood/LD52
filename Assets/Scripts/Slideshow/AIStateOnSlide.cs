using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateOnSlide : EffectOnSlideShown
{
   public ScarecrowAI AI;
   public bool stop;
   internal override void Trigger()
   {
      if (stop)
      {
         AI.PauseScarecrow();
      }
      else
      {
         AI.ResetScarecrow();
      }
   }
}
