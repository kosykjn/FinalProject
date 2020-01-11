namespace Calendar.DatabaseManager
{
    public interface IDatabaseManager
    {
        DateInfo[] GetDataFromDatabase();
        void WriteDataToDatabase(DateInfo[] dateTimeData);
    }
}
