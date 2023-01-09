using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public abstract class UpdateTextOnMorning : UpdateOnMorning
{
   [TextArea]
   public string FormatString;
   private TMP_Text text;
   private TMP_Text Text
   {
      get
      {
         if(text == null)
         {
            text = gameObject.GetComponent<TMP_Text>();
         }
         return text;
      }
   }

   public override void UpdateInfo(MorningReport report)
   {
      Text.text = string.Format(FormatString, GetUpdateInfo(report));
   }

   public void ManuallyUpdateText(string value)
   {
      Text.text = string.Format(FormatString, value);
   }
   protected abstract string GetUpdateInfo(MorningReport report);

}
