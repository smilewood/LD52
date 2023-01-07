using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
   public Transform Player;
   public float deadZone;
   public Vector2 offset;
   public float speed;

   // Update is called once per frame
   void Update()
   {
      if(Vector2.Distance(Player.position + (Vector3)offset, transform.position) > deadZone)
      {
         transform.position = Vector3.Lerp(transform.position, 
            new Vector3(Player.position.x, Player.position.y, this.transform.position.z), 
            speed);
      }
   }
}
