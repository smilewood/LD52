using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WheatInLight : MonoBehaviour
{
   private LanterRaycaster lightSource;
   private ShadowCaster2D shadow;
   private new Collider2D collider;
   private void Awake()
   {
      lightSource = GameObject.Find("Lantern").GetComponent<LanterRaycaster>();
      shadow = this.GetComponent<ShadowCaster2D>();
      collider = GetComponentInParent<Collider2D>();
   }


   public void FixedUpdate()
   {
      Vector2 pos = this.transform.position;
      Vector2 light = lightSource.transform.position;

      if (lightSource.CanSee(collider) || 
         (pos.x - .5f < light.x && light.x < pos.x + .5f && pos.y - .5f < light.y && light.y < pos.y + .5f)) //This relies on this being exactly 1x1 and centered
      {
         shadow.enabled = false;
      }
      else
      {
         shadow.enabled = true;
      }
   }
}
