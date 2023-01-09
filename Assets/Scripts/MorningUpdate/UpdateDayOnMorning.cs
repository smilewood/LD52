public class UpdateDayOnMorning : UpdateTextOnMorning
{
   protected override string GetUpdateInfo(MorningReport report)
   {
      return report.DayNumber.ToString();
   }
}
