using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Calendar.DatabaseManager;
using Calendar.Extensions;

namespace Calendar.CalendarManager
{
    public class CalendarManagerModel : ICalendarManager
    {
        private readonly double dayWorkTimeHours = 8.0;
        private readonly Label[,] calendarLabels;
        private readonly RichTextBox[,] calendarRichTextBoxes;

        private IDatabaseManager databaseManager;

        public DateTime Today => DateTime.Today;

        public CalendarManagerModel(Label[,] labels, RichTextBox[,] richTextBoxes)
        {
            calendarLabels = labels;
            calendarRichTextBoxes = richTextBoxes;

            var dbDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent;
            databaseManager = new DatabaseManagerModel($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbDirectory?.FullName}\CalendarDB.mdf;Integrated Security=True;Connect Timeout=30");
        }

        private void ClearCells()
        {
            foreach (var calendarLabel in calendarLabels)
            {
                calendarLabel.Visible = false;
                calendarLabel.Text = string.Empty;
                calendarLabel.Font = new Font(calendarLabel.Font.Name, 8, FontStyle.Bold);
            }

            foreach (var calendarRichTextBox in calendarRichTextBoxes)
            {
                calendarRichTextBox.Visible = false;
                calendarRichTextBox.BackColor = Color.DarkGray;
            }
        }

        private void SetCalendarCell(int weekIndex, int dayIndex, DateTime dateTime)
        {
            calendarLabels[weekIndex, dayIndex].Visible = calendarRichTextBoxes[weekIndex, dayIndex].Visible = true;

            calendarLabels[weekIndex, dayIndex].Text = dateTime.ToShortDateString();
            calendarRichTextBoxes[weekIndex, dayIndex].Tag = calendarLabels[weekIndex, dayIndex].Tag = dateTime;

            if (Today.Equals(dateTime))
            {
                calendarRichTextBoxes[weekIndex, dayIndex].BackColor = Color.CornflowerBlue;
            }
            else if (Today > dateTime) //Passed days
            {
                calendarRichTextBoxes[weekIndex, dayIndex].BackColor = Color.DarkGray;
            }
            else if (dayIndex > 4) //Saturday, Sunday
            {
                calendarRichTextBoxes[weekIndex, dayIndex].BackColor = Color.Red;
            }
            else
            {
                calendarRichTextBoxes[weekIndex, dayIndex].BackColor = Color.White;
            }
        }

        public void SetDate(int year, int month)
        {
            ClearCells();

            var currentDateTime = new DateTime(year, month, 1);

            while (currentDateTime.Month == month)
            {
                var dayOfWeekNumber = currentDateTime.GetDayOfWeekNumber();
                var currentWeekNumber = currentDateTime.GetWeekNumberOfMonth();

                SetCalendarCell(currentWeekNumber, dayOfWeekNumber, currentDateTime);

                //Increment date
                currentDateTime = currentDateTime.AddDays(1.0);
            }

            //Load data from DB
            Load();
        }

        public void CalculateLoadDays(out int busyDaysCount, out int freeDaysCount)
        {
            busyDaysCount = 0;
            freeDaysCount = 0;

            foreach (var calendarRichTextBox in calendarRichTextBoxes)
            {
                if (calendarRichTextBox.Visible)
                {
                    if (calendarRichTextBox.Text.Trim() == string.Empty)
                    {
                        freeDaysCount++;
                    }
                    else
                    {
                        busyDaysCount++;
                    }
                }
            }
        }

        public void CalculateLoadHours(out double busyHoursCount, out double freeHoursCount)
        {
            CalculateLoadDays(out var busyDaysCount, out var freeDaysCount);

            busyHoursCount = dayWorkTimeHours * busyDaysCount;
            freeHoursCount = dayWorkTimeHours * freeDaysCount;
        }

        public void Load()
        {
            var data = databaseManager.GetDataFromDatabase();

            foreach (var calendarRichTextBox in calendarRichTextBoxes)
            {
                if (calendarRichTextBox.Tag != null)
                {
                    foreach (var dateInfo in data)
                    {
                        var richTextBoxDateTime = (DateTime)calendarRichTextBox.Tag;

                        if (richTextBoxDateTime == dateInfo.DateTime)
                        {
                            calendarRichTextBox.Text = dateInfo.TasksDescription;
                        }
                    }
                }
            }
        }

        public void Save()
        {
            var data = new List<DateInfo>();

            foreach (var calendarRichTextBox in calendarRichTextBoxes)
            {
                if (calendarRichTextBox.Visible && calendarRichTextBox.Text != string.Empty)
                {
                    var t = (DateTime) calendarRichTextBox.Tag;
                    data.Add(new DateInfo((DateTime)calendarRichTextBox.Tag, calendarRichTextBox.Text));
                }
            }

            databaseManager.WriteDataToDatabase(data.ToArray());
        }
    }
}