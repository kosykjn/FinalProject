namespace Calendar.CalendarManager
{
    public interface ICalendarManager
    {
        void SetDate(int year, int month);
        void CalculateLoadDays(out int busyDaysCount, out int freeDaysCount);
        void CalculateLoadHours(out double busyHoursCount, out double freeHoursCount);
        void Load();
        void Save();
    }
}