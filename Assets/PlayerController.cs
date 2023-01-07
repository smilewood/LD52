using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float Speed;
   
   private Vector2 direction;
   private SpriteRenderer sprite;
   private Rigidbody2D rb;

   // Start is called before the first frame update
   void Start()
   {
      sprite = GetComponent<SpriteRenderer>();
      rb = GetComponent<Rigidbody2D>();
   }

   // Update is called once per frame
   void Update()
   {
      float horiz = Input.GetAxis("Horizontal");
      direction = new Vector2(horiz, Input.GetAxis("Vertical"));
      if(horiz > 0)
      {
         sprite.flipX = false;
      }
      else if(horiz < 0)
      {
         sprite.flipX = true;
      }
      rb.velocity = (Vector3)direction * Time.fixedDeltaTime * Speed;
   }

}
