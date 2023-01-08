using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowAI : MonoBehaviour
{
   private enum AIState
   {
      Wander,
      Sound,
      Chase,
      Stop
   }
   public GameObject Player;
   public Transform RaycastSource;
   public GameObject[] waypoints;
   private AIState state;
   public Vector2 target;

   public float LOSRange;

   public float speed;
   [Range(0,1)]
   public float speedMod;

   private ParticleSystem sparks;

   private bool searching = false;
   private float searchCountdown;
   public float SearchTime;
   private Animator Animator;
   public AudioClip ChaseClip, EscapeClip, CaughtClip;
   private void Start()
   {
      state = AIState.Wander;
      target = waypoints[Random.Range(0, waypoints.Length)].transform.position;
      sparks = GetComponentInChildren<ParticleSystem>();
      Animator = GetComponent<Animator>();
   }

   private void Update()
   {
      if(state == AIState.Stop)
      {
         return;
      }
      UpdateMovement();

      CheckLOS();
   }

   private void UpdateMovement()
   {
      Vector3 direction = ((Vector3)target - transform.position).normalized;
      transform.position = transform.position + (direction * speed * speedMod * Time.deltaTime);

      switch (state)
      {
         case AIState.Wander:
         {
            //At the waypoint, just move to another
            if (Vector2.Distance(transform.position, target) < .01)
            {
               target = waypoints[Random.Range(0, waypoints.Length)].transform.position;
            }
            break;
         }
         case AIState.Sound:
         case AIState.Chase:
         {
            //For both sound and chace we want to go to the last known spot. 
            //If in chace mode it is likely that another part of update will be updating the target

            //Got to the target, hang around a moment as if searching
            if (Vector2.Distance(transform.position, target) < .01)
            {
               if (!searching)
               {
                  //Just got here, start searching
                  if(state == AIState.Chase)
                  {
                     AudioManager.Instance.PlayMusicThenBackground(EscapeClip);
                     sparks.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                  }
                  searching = true;
                  searchCountdown = SearchTime;
               }
               else
               {
                  searchCountdown -= Time.deltaTime;
                  if(searchCountdown <= 0)
                  {
                     ChangeAIState(AIState.Wander);
                  }
               }
            }
            break;
         }
      }
   }

   private void CheckLOS()
   {
      List<RaycastHit2D> results = new List<RaycastHit2D>();
      ContactFilter2D filter = new ContactFilter2D()
      {
         layerMask = LayerMask.GetMask("Crops", "Player"),
         useTriggers = true
      };
      _ = Physics2D.Raycast(RaycastSource.position, 
         (Player.transform.position - RaycastSource.position).normalized, 
         filter, results, LOSRange);

      //Debug.Log(string.Join(", ", results.Select(r => r.collider.gameObject.name)));
      for (int i = 0; i < 2 && i < results.Count; ++i)
      {
         //If the first or second thing hit is the player
         if (results[i].collider.gameObject.layer == LayerMask.NameToLayer("Player"))
         {
            ChangeAIState(AIState.Chase);
            target = results[i].collider.gameObject.transform.position;
         }
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.DrawWireSphere(RaycastSource.position, LOSRange);
      if (Application.isPlaying)
      {
         Gizmos.DrawRay(new Ray(RaycastSource.position, (Player.transform.position - RaycastSource.position).normalized));
      }
   }

   public void MadeASound(Vector2 location)
   {
      if (state != AIState.Chase)
      {
         ChangeAIState(AIState.Sound);
         target = location;
      }
   }

   private void ChangeAIState(AIState newState)
   {
      searching = false;
      switch (newState)
      {
         case AIState.Wander:
         {
            if(state == AIState.Chase)
            {
               //Chase will always end with wander
            }
            state = AIState.Wander;
            break;
         }
         case AIState.Sound:
         {
            state = AIState.Sound;
            break;
         }
         case AIState.Chase:
         {
            if(state != AIState.Chase)
            {
               Debug.Log("Starting Chase");
               state = AIState.Chase;

               //Make the sound
               AudioManager.Instance.PlayMusic(ChaseClip);

               sparks.Play();
            }
            break;
         }
      }
   }

   public GameObject GameOverUI;

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if(collision.gameObject == Player)
      {
         //Game Over
         Animator.SetTrigger("Stop");
         state = AIState.Stop;
         AudioManager.Instance.PlayMusicThenBackground(CaughtClip);
         Player.GetComponent<PlayerController>().Dead = true;
         GameOverUI.SetActive(true);
      }
   }
}