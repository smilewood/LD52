public class UpdatePriceOnMorning : UpdateTextOnMorning
{
   protected override string GetUpdateInfo(MorningReport report)
   {
      return report.upgrades.wheatPrice.ToString();
   }
}
