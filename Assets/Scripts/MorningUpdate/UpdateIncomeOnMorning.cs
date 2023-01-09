public class UpdateIncomeOnMorning : UpdateTextOnMorning
{
   protected override string GetUpdateInfo(MorningReport report)
   {
      return report.Income.ToString();
   }
}
