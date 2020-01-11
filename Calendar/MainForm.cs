using System;
using System.Windows.Forms;
using Calendar.CalendarManager;

namespace Calendar
{
    public partial class MainForm : Form
    {
        #region Fields

        private bool isCalendarInitialized;

        public ICalendarManager CalendarManager { get; protected set; }

        #endregion

        #region Methods

        private void InitializeCalendarControls()
        {
            nudCalendarYear.Value = DateTime.Now.Year;
            cbMonth.SelectedIndex = 0;
        }

        private void InitializeCalendarManager()
        {
            CalendarManager = new CalendarManagerModel(
                new[,]
                {
                    { label1, label2, label3, label4, label5, label6, label7 },
                    { label8, label9, label10, label11, label12, label13, label14 },
                    { label15, label16, label17, label18, label19, label20, label21 },
                    { label22, label23, label24, label25, label26, label27, label28 },
                    { label29, label30, label31, label32, label33, label34, label35 },
                    { label36, label37, label38, label39, label40, label41, label42 }
                },
                new[,]
                {
                    { richTextBox1, richTextBox2, richTextBox3, richTextBox4, richTextBox5, richTextBox6, richTextBox7 },
                    { richTextBox8, richTextBox9, richTextBox10, richTextBox11, richTextBox12, richTextBox13, richTextBox14 },
                    { richTextBox15, richTextBox16, richTextBox17, richTextBox18, richTextBox19, richTextBox20, richTextBox21 },
                    { richTextBox22, richTextBox23, richTextBox24, richTextBox25, richTextBox26, richTextBox27, richTextBox28 },
                    { richTextBox29, richTextBox30, richTextBox31, richTextBox32, richTextBox33, richTextBox34, richTextBox35 },
                    { richTextBox36, richTextBox37, richTextBox38, richTextBox39, richTextBox40, richTextBox41, richTextBox42 }
                });

            isCalendarInitialized = true;
        }

        private void SetCalendarDefault()
        {
            CalendarManager.SetDate(DateTime.Now.Year, 1);
        }

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            InitializeCalendarControls();
            InitializeCalendarManager();
            SetCalendarDefault();
        }

        #endregion

        #region Events

        private void nudCalendarYear_ValueChanged(object sender, EventArgs e)
        {
            if (isCalendarInitialized)
            {
                CalendarManager.SetDate((int)nudCalendarYear.Value, cbMonth.SelectedIndex + 1);
            }
            
        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isCalendarInitialized)
            {
                CalendarManager.SetDate((int)nudCalendarYear.Value, cbMonth.SelectedIndex + 1);
            }
        }

        private void btnCalculateLoad_Click(object sender, EventArgs e)
        {
            CalendarManager.CalculateLoadDays(out var busyDaysCount, out var freeDaysCount);
            CalendarManager.CalculateLoadHours(out var busyHoursCount, out var freeHoursCount);

            MessageBox.Show($"Кол-во свободных дней: {freeDaysCount}\n" +
                            $"Кол-во свободных часов: {freeHoursCount}\n" +
                            $"Кол-во занятых дней: {busyDaysCount}\n" +
                            $"Кол-во занятых часов: {busyHoursCount}",
                "Месячная загрузка");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CalendarManager.Save();
        }

        #endregion
    }
}
