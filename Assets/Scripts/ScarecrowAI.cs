using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowAI : MonoBehaviour
{
   private enum AIState
   {
      Wander,
      Sound,
      Search,
      Chase,
      Stop
   }
   public GameObject Player;
   public Transform RaycastSource;
   public GameObject[] waypoints;
   private AIState state;
   public Vector2 target;

   public float LOSRange;
   public int LOSPierce;
   public float speed;
   [Range(0,1)]
   public float speedMod;

   private ParticleSystem sparks;

   private float searchCountdown;
   public float SearchTime;
   private Animator Animator;
   public AudioClip ChaseClip, EscapeClip, CaughtClip;

   private Vector3 startPos;
   private void Start()
   {
      state = AIState.Wander;
      target = waypoints[Random.Range(0, waypoints.Length)].transform.position;
      sparks = GetComponentInChildren<ParticleSystem>();
      Animator = GetComponent<Animator>();
      startPos = transform.position;
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
      transform.position = Vector3.MoveTowards(transform.position, target, speed * speedMod * Time.deltaTime);

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

            if (Vector2.Distance(transform.position, target) < .01)
            {
               ChangeAIState(AIState.Search);
            }
            break;
         }
         case AIState.Search:
         {
            searchCountdown -= Time.deltaTime;
            if (searchCountdown <= 0)
            {
               ChangeAIState(AIState.Wander);
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
      //bool hasLOS = false;
      for (int i = 0; i < LOSPierce && i < results.Count; ++i)
      {
         //If the first or second thing hit is the player
         if (results[i].collider.gameObject.layer == LayerMask.NameToLayer("Player"))
         {
            ChangeAIState(AIState.Chase);
            target = results[i].collider.gameObject.transform.position;
            //hasLOS = true;
         }
      }
      //if(state == AIState.Chase && !hasLOS)
      //{
      //   //Debug.Break();
      //   //We lost them, go find the place
      //   Debug.Log("LOS Lost");
      //   ChangeAIState(AIState.Sound);
      //}
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
      switch (newState)
      {
         case AIState.Wander:
         {
            state = AIState.Wander;
            break;
         }
         case AIState.Sound:
         {
            
            state = AIState.Sound;
            break;
         }
         case AIState.Search:
         {
            //A chase ends with losing LOS, so sound mode, or death, so deal with chase ending here.
            if (state == AIState.Chase)
            {
               Debug.Log("EndChase");
               AudioManager.Instance.PlayMusicThenBackground(EscapeClip);
               sparks.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            searchCountdown = SearchTime;
            state = AIState.Search;
            break;
         }
         case AIState.Chase:
         {
            if(state != AIState.Chase)
            {
               Debug.Log("Starting Chase");
               state = AIState.Chase;

               //Make the sound
               AudioManager.Instance.PlayMusic(ChaseClip, false);

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

   public void PauseScarecrow()
   {
      state = AIState.Stop;
      Animator.SetTrigger("Stop");
   }

   public void ResetScarecrow()
   {
      state = AIState.Wander;
      target = waypoints[Random.Range(0, waypoints.Length)].transform.position;
      Animator.SetTrigger("Resume");
      transform.position = startPos;
   }
}