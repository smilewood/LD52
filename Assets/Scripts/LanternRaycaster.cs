using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternRaycaster : MonoBehaviour
{
   public bool CanSee(Collider2D target, float distance, int layerMask)
   {
      RaycastHit2D hit = Physics2D.Raycast(this.transform.position, 
         (target.ClosestPoint(this.transform.position) - (Vector2)this.transform.position).normalized, distance, layerMask);
      return hit.collider == target;
   }
}
