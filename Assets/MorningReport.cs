using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeMenu))]
public class MorningReport : MonoBehaviour
{
   public GameObject Player;
   public List<GameObject> ReportsToShow;
   public float delay;

   [HideInInspector]
   public int Income;
   [HideInInspector]
   public int DayNumber = 0;

   private HarverstControlls harvestCount;
   [HideInInspector]
   public UpgradeMenu upgrades;

   // Start is called before the first frame update
   void Awake()
   {
      harvestCount = Player.GetComponent<HarverstControlls>();
      upgrades = gameObject.GetComponent<UpgradeMenu>();
   }

   private void OnEnable()
   {
      Player.GetComponent<PlayerController>().Dead = true;
      foreach (GameObject go in ReportsToShow)
      {
         go.SetActive(false);
      }
      ++DayNumber;
      Income = harvestCount.count / upgrades.wheatPrice;
      upgrades.AddMoney(Income);
      StartCoroutine(PlayReport());
   }

   private void OnDisable()
   {
      foreach(GameObject go in ReportsToShow)
      {
         go.SetActive(false);
      }
   }

   private IEnumerator PlayReport()
   {
      foreach (GameObject go in ReportsToShow)
      {
         if(go.GetComponent<UpdateOnMorning>() is UpdateOnMorning update)
         {
            update.UpdateInfo(this);
         }
         go.SetActive(true);
         yield return new WaitForSeconds(delay);
      }
   }
}
