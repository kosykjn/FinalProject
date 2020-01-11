using System;

namespace Calendar.DatabaseManager
{
    public class DateInfo
    {
        public DateTime DateTime { get; }
        public string TasksDescription { get; }

        public DateInfo(DateTime dateTime, string tasksDescription)
        {
            DateTime = dateTime;
            TasksDescription = tasksDescription;
        }
    }
}
