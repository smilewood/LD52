public class UpdateWheatCountOnMorning : UpdateTextOnMorning
{
   protected override string GetUpdateInfo(MorningReport report)
   {
      return report.Player.GetComponent<HarverstControlls>().count.ToString();
   }
}
