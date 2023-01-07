using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanterRaycaster : MonoBehaviour
{
   public bool CanSee(Collider2D target)
   {
      RaycastHit2D hit = Physics2D.Raycast(this.transform.position, 
         (target.ClosestPoint(this.transform.position) - (Vector2)this.transform.position).normalized);
      return hit.collider == target;
   }
}
