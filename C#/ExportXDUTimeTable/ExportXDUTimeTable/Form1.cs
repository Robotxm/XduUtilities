using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Newtonsoft.Json;

namespace ExportXDUTimeTable
{
    public partial class Form1 : Form
    {
        HttpClient hc = new HttpClient();
        string strLt, strExecution, strEventId, strRmShown;
        Calendar timeTable = new Calendar();

        // Time A (summer)
        string[] sectionStartTime_A = {"08:00", "08:30", "09:20", "10:25", "11:15", // morning
            "14:30", "15:20", "16:25", "17:15", // afternoon
            "19:30", "20:20"}; // evening
        string[] sectionEndTime_A   = {"08:30", "09:15", "10:05", "11:10", "12:00",
            "15:15", "16:05", "17:10", "18:00",
            "20:15", "21:05"};

        // Time B (spring, autumn and winter)
        string[] sectionStartTime_B = {"08:00", "08:30", "09:20", "10:25", "11:15",
            "14:00", "14:50", "15:55", "16:45",
            "19:00", "19:50"};
        string[] sectionEndTime_B   = {"08:30", "09:15", "10:05", "11:10", "12:00",
            "14:45", "15:35", "16:40", "17:30",
            "19:45", "20:35"};

        int allTeachWeeks;


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!hc.DefaultRequestHeaders.Contains("Referer"))
                hc.DefaultRequestHeaders.Add("Referer", "http://ids.xidian.edu.cn/authserver/login?service=https%3A%2F%2Fxidian.cpdaily.com%2Fcomapp-timetable%2Fsys%2FschoolTimetable%2Fv2%2Fapi%2FweekTimetable");
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", txtStuID.Text),
                new KeyValuePair<string, string>("password", txtPwd.Text),
                new KeyValuePair<string, string>("submit", ""),
                new KeyValuePair<string, string>("lt", strLt),
                new KeyValuePair<string, string>("execution", strExecution),
                new KeyValuePair<string, string>("_eventId", strEventId),
                new KeyValuePair<string, string>("rmShown", strRmShown)
            };
            // Get json which contains time table
            string strJson = hc.PostAsync("http://ids.xidian.edu.cn/authserver/login?service=https%3A%2F%2Fxidian.cpdaily.com%2Fcomapp-timetable%2Fsys%2FschoolTimetable%2Fv2%2Fapi%2FweekTimetable", new FormUrlEncodedContent(paramList)).Result.Content.ReadAsStringAsync().Result;
            if (!strJson.Contains("有误") || !strJson.Contains("放假中"))
            {
                JObject jCourse = (JObject)JsonConvert.DeserializeObject(strJson);

                allTeachWeeks = int.Parse(jCourse["allTeachWeeks"].ToString());

                // Start conversion
                // All weeks in term
                for (int curWeek = 0; curWeek < allTeachWeeks; curWeek++)
                {
                    // All days in a week
                    for (int curDay = 0; curDay < 7; curDay++)
                    {
                        // Check if there's any course in the day
                        if (jCourse["termWeeksCourse"][curWeek]["courses"][curDay]["sectionCourses"].Count() != 0)
                        {
                            // All courses in the day
                            for (int curCourse = 0; curCourse < jCourse["termWeeksCourse"][curWeek]["courses"][curDay]["sectionCourses"].Count(); curCourse++)
                            {
                                string courseDay = jCourse["termWeeksCourse"][curWeek]["courses"][curDay]["date"].ToString();
                                string courseName = jCourse["termWeeksCourse"][curWeek]["courses"][curDay]["sectionCourses"][curCourse]["courseName"].ToString();
                                string courseClassroom = jCourse["termWeeksCourse"][curWeek]["courses"][curDay]["sectionCourses"][curCourse]["classroom"].ToString();
                                int courseStart, courseEnd;
                                courseStart = int.Parse(jCourse["termWeeksCourse"][curWeek]["courses"][curDay]["sectionCourses"][curCourse]["sectionStart"].ToString());
                                courseEnd = int.Parse(jCourse["termWeeksCourse"][curWeek]["courses"][curDay]["sectionCourses"][curCourse]["sectionEnd"].ToString());
                                if (chkChangeTime.Checked)
                                {
                                    if (DateTime.Compare(DateTime.Parse(courseDay), dtpTimeB.Value) > 0 && DateTime.Compare(DateTime.Parse(courseDay), dtpTimeA.Value) < 0) // Time B
                                        AddToCalendar(courseDay, courseName, courseClassroom, courseStart, courseEnd, true);
                                    else
                                        AddToCalendar(courseDay, courseName, courseClassroom, courseStart, courseEnd, false);
                                }
                                else
                                    AddToCalendar(courseDay, courseName, courseClassroom, courseStart, courseEnd, false);
                            }
                        }
                    }
                }

                CalendarSerializer serializer = new CalendarSerializer();
                string serializedCalendar = serializer.SerializeToString(timeTable);
                // Write to iCalendar file
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).EndsWith("\\") ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
                System.IO.File.WriteAllText(desktopPath + "xdu_timetable_" + txtStuID.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".ics", serializedCalendar);
                MessageBox.Show(this, "课表保存到: " + desktopPath + "xdu_timetable_" + txtStuID.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"), "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(this, "获取课表失败！\n请检查学号或密码是否正确。\n如果确认无误，可能是服务器升级了或爆炸了，请与作者联系。\n另外，假期时间无法获取课表。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Prepare things that login process needs
            hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.92 Safari/537.36");
            string strHtml = hc.GetStringAsync("http://ids.xidian.edu.cn/authserver/login?service=https%3A%2F%2Fxidian.cpdaily.com%2Fcomapp-timetable%2Fsys%2FschoolTimetable%2Fv2%2Fapi%2FweekTimetable").Result.ToString();
            strLt = Regex.Match(strHtml, @"<input type=""hidden"" name=""lt"" value=""(.*?)""/>").Groups[1].Value;
            strExecution = Regex.Match(strHtml, @"<input type=""hidden"" name=""execution"" value=""(.*?)""/>").Groups[1].Value;
            strEventId = Regex.Match(strHtml, @"<input type=""hidden"" name=""_eventId"" value=""(.*?)""/>").Groups[1].Value;
            strRmShown = Regex.Match(strHtml, @"<input type=""hidden"" name=""rmShown"" value=""(.*?)"">").Groups[1].Value;
        }

        public void AddToCalendar(string date, string courseName, string classroom, int sectionStart, int sectionEnd, bool isTimeB)
        {
            if (isTimeB)
            {
                CalendarEvent e = new CalendarEvent
                {
                    Start = new CalDateTime(DateTime.Parse(date + " " + sectionStartTime_B[sectionStart])),
                    End = new CalDateTime(DateTime.Parse(date + " " + sectionEndTime_B[sectionEnd])),
                    Description = courseName + " @ " + classroom,
                    Summary = courseName + " @ " + classroom
                };
                timeTable.Events.Add(e);
            }
            else
            {
                CalendarEvent e = new CalendarEvent
                {
                    Start = new CalDateTime(DateTime.Parse(date + " " + sectionStartTime_A[sectionStart])),
                    End = new CalDateTime(DateTime.Parse(date + " " + sectionEndTime_A[sectionEnd])),
                    Description = courseName + " @ " + classroom,
                    Summary = courseName + " @ " + classroom
                };
                timeTable.Events.Add(e);
            }
        }
    }
}
