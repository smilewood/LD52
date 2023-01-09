
public class UpdateMoneyOnMorning : UpdateTextOnMorning
{
   protected override string GetUpdateInfo(MorningReport report)
   {
      return report.upgrades.money.ToString();
   }
}
