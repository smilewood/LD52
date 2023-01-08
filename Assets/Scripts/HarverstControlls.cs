using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarverstControlls : MonoBehaviour
{
   private WheatfieldController field;
   public bool harvesting;
   public float HarvestTime;
   private float harvestTimer;
   private ScarecrowAI scarecrow;
   public AudioSource SoundEffect;
   public int count = 0;
   public Text countText;
   // Start is called before the first frame update
   void Start()
   {
      field = GameObject.Find("Wheatfield").GetComponent<WheatfieldController>();
      scarecrow = GameObject.Find("Scarecrow").GetComponent<ScarecrowAI>();
      countText.text = $"Wheat: {count}";

   }

   // Update is called once per frame
   void Update()
   {
      if (Input.GetButtonDown("Jump"))
      {
         //Start harvesting
         if(field.IsWheatAtLocation(GetGridPos()))
         {
            harvesting = true;
            SoundEffect.Play();
            scarecrow.MadeASound(this.transform.position);
            harvestTimer = HarvestTime;
         }
      }
      if (Input.GetButtonUp("Jump"))
      {
         //Stop harvesting
         harvesting = false;
      }

      if (harvesting)
      {
         harvestTimer -= Time.deltaTime;
         if(harvestTimer < 0)
         {
            field.GetPlotAtLocation(GetGridPos()).Harvest();
            ++count;
            countText.text = $"Wheat: {count}";
            harvesting = false;
         }
      }
   }

   private Vector2Int GetGridPos()
   {
      return new Vector2Int(Mathf.FloorToInt(this.transform.position.x), Mathf.FloorToInt(this.transform.position.y));
   }
}
