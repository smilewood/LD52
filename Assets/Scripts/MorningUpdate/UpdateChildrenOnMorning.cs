using UnityEngine;

public class UpdateChildrenOnMorning : UpdateOnMorning
{
   public override void UpdateInfo(MorningReport Report)
   {
      foreach (Transform child in this.transform)
      {
         if(child.gameObject.GetComponent<UpdateOnMorning>() is UpdateOnMorning update)
         {
            update.UpdateInfo(Report);
         }
      }
   }
}
