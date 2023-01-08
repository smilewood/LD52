using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float Speed;
   
   private Vector2 direction;
   private SpriteRenderer sprite;
   private Rigidbody2D rb;
   private HarverstControlls harverter;
   public AudioSource WalkingSound;
   public bool Dead = false;
   // Start is called before the first frame update
   void Start()
   {
      sprite = GetComponent<SpriteRenderer>();
      rb = GetComponent<Rigidbody2D>();
      harverter = GetComponent<HarverstControlls>();
   }

   // Update is called once per frame
   void Update()
   {
      float horiz = 0;
      float vert = 0;
      if (!harverter.harvesting && !Dead)
      {
         horiz = Input.GetAxisRaw("Horizontal");
         vert = Input.GetAxisRaw("Vertical");
      }
      direction = new Vector2(horiz, vert).normalized;
      if(direction != Vector2.zero && !WalkingSound.isPlaying)
      {
         WalkingSound.Play();
      }
      else if(direction == Vector2.zero && WalkingSound.isPlaying)
      {
         WalkingSound.Stop();
      }
      if (horiz > 0)
      {
         sprite.flipX = false;
      }
      else if (horiz < 0)
      {
         sprite.flipX = true;
      }
   }

   private void FixedUpdate()
   {
      rb.velocity = direction * Speed;
   }

}
