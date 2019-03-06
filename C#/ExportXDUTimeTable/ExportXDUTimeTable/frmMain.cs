using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Newtonsoft.Json;
using XduUIA;

namespace ExportXDUTimeTable
{
    public partial class frmMain : Form
    {
        Calendar timeTable = new Calendar();

        // Time A (summer)
        string[] sectionSummerStartTime = {"08:00", "08:30", "09:20", "10:25", "11:15", // morning
            "14:30", "15:20", "16:25", "17:15", // afternoon
            "19:30", "20:20"}; // evening
        string[] sectionSummerEndTime = {"08:30", "09:15", "10:05", "11:10", "12:00",
            "15:15", "16:05", "17:10", "18:00",
            "20:15", "21:05"};

        // Time B (spring, autumn and winter)
        string[] sectionWinterStartTime = {"08:00", "08:30", "09:20", "10:25", "11:15",
            "14:00", "14:50", "15:55", "16:45",
            "19:00", "19:50"};
        string[] sectionWinterEndTime = {"08:30", "09:15", "10:05", "11:10", "12:00",
            "14:45", "15:35", "16:40", "17:30",
            "19:45", "20:35"};

        private void btnLogin_Click(object sender, EventArgs e)
        {
            JArray courseList = new JArray();
            string term = "";
            if (rbCpdaily.Checked)
            {
                Ids ids = new Ids(txtStuID.Text, txtPwd.Text, "https%3A%2F%2Fxidian.cpdaily.com%2Fcomapp-timetable%2Fsys%2FschoolTimetable%2Fv2%2Fapi%2FweekTimetable");
                HttpClient hc = ids.Login(out Image _);
                try
                {
                    string json = hc
                        .GetStringAsync(
                            "https://xidian.cpdaily.com/comapp-timetable/sys/schoolTimetable/v2/api/weekTimetable").Result;
                    courseList = GetFromCpdaily(json);
                }
                catch (Exception)
                {
                    MessageBox.Show("获取课表失败！\n请检查学号或密码是否正确。\n如果确认无误，请尝试一站式服务大厅数据源。\n另外，假期时间无法获取课表。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (rbEhall.Checked)
            {
                Ids ids = new Ids(txtStuID.Text, txtPwd.Text, "http://ehall.xidian.edu.cn:80//appShow");
                HttpClient hc = ids.Login(out Image _);
                try
                {
                    string json = hc.GetStringAsync("http://ehall.xidian.edu.cn//appShow?appId=4770397878132218").Result;
                    hc.DefaultRequestHeaders.Remove("Accept");
                    hc.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    json = hc.PostAsync("http://ehall.xidian.edu.cn/jwapp/sys/wdkb/modules/jshkcb/dqxnxq.do", null)
                        .Result.Content.ReadAsStringAsync().Result;
                    term = ((JObject)JsonConvert.DeserializeObject(json))["datas"]["dqxnxq"]["rows"][0]["DM"].ToString();
                    json = hc.PostAsync("http://ehall.xidian.edu.cn/jwapp/sys/wdkb/modules/jshkcb/cxjcs.do", new StringContent($"XN={term.Split('-')[0]}-{term.Split('-')[1]}&XQ={term.Split('-')[2]}", Encoding.UTF8, "application/x-www-form-urlencoded")).Result.Content.ReadAsStringAsync().Result;
                    JToken termInfo = ((JObject)JsonConvert.DeserializeObject(json))["datas"]["cxjcs"]["rows"][0];
                    DateTime termStartDate = DateTime.Parse(termInfo["XQKSRQ"].ToString());
                    json = hc.PostAsync("http://ehall.xidian.edu.cn/jwapp/sys/wdkb/modules/xskcb/xskcb.do",
                        new StringContent($"XNXQDM={term}", Encoding.UTF8, "application/x-www-form-urlencoded")).Result.Content.ReadAsStringAsync().Result;
                    courseList = GetFromEhall(json, termStartDate);
                }
                catch (Exception)
                {
                   MessageBox.Show("获取课表失败！\n请检查学号或密码是否正确。\n如果确认无误，请尝试今日校园数据源。\n另外，假期时间无法获取课表。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                   return;
                }
            }

            foreach (var course in courseList)
            {
                AddToCalendar(
                    course["date"].ToString(),
                    course["name"].ToString(),
                    course["classroom"].ToString(),
                    int.Parse(course["sectionStart"].ToString()),
                    int.Parse(course["sectionEnd"].ToString())
                );
            }

            CalendarSerializer serializer = new CalendarSerializer();
            string serializedCalendar = serializer.SerializeToString(timeTable);
            // Write to iCalendar file
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
            System.IO.File.WriteAllText(desktopPath + txtStuID.Text + "_" + term + ".ics", serializedCalendar);
            MessageBox.Show("课表保存到: " + desktopPath + txtStuID.Text + "_" + term + ".ics！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public frmMain()
        {
            InitializeComponent();
        }

        public void AddToCalendar(string date, string courseName, string classroom, int sectionStart, int sectionEnd)
        {
            if (sectionStart > 10 || sectionEnd > 10)
                return;
            if (chkChangeTime.Checked)
            {
                if (DateTime.Compare(DateTime.Parse(date), dtpTimeB.Value) > 0 &&
                    DateTime.Compare(DateTime.Parse(date), dtpTimeA.Value) < 0)
                {
                    CalendarEvent e = new CalendarEvent
                    {
                        Start = new CalDateTime(DateTime.Parse(date + " " + sectionWinterStartTime[sectionStart])),
                        End = new CalDateTime(DateTime.Parse(date + " " + sectionWinterEndTime[sectionEnd])),
                        Description = courseName + " @ " + classroom,
                        Summary = courseName + " @ " + classroom
                    };
                    timeTable.Events.Add(e);
                }
                else
                {
                    CalendarEvent e = new CalendarEvent
                    {
                        Start = new CalDateTime(DateTime.Parse(date + " " + sectionSummerStartTime[sectionStart])),
                        End = new CalDateTime(DateTime.Parse(date + " " + sectionSummerEndTime[sectionEnd])),
                        Description = courseName + " @ " + classroom,
                        Summary = courseName + " @ " + classroom
                    };
                    timeTable.Events.Add(e);
                }
            }
            else
            {
                CalendarEvent e = new CalendarEvent
                {
                    Start = new CalDateTime(DateTime.Parse(date + " " + sectionSummerStartTime[sectionStart])),
                    End = new CalDateTime(DateTime.Parse(date + " " + sectionSummerEndTime[sectionEnd])),
                    Description = courseName + " @ " + classroom,
                    Summary = courseName + " @ " + classroom
                };
                timeTable.Events.Add(e);
            }
        }

        public JArray GetFromCpdaily(string json)
        {
            JObject jCourse = (JObject)JsonConvert.DeserializeObject(json);
            JArray courseList = new JArray();
            foreach (var week in jCourse["termWeeksCourse"])
            {
                foreach (var day in week["courses"])
                {
                    foreach (var course in day["sectionCourses"])
                    {
                        courseList.Add(new JObject
                        {
                            {"name", course["courseName"]},
                            {"classroom", course["classroom"]},
                            {"date", day["date"]},
                            {"sectionStart", course["sectionStart"]},
                            {"sectionEnd", course["sectionEnd"]}
                        });
                    }
                }
            }

            return courseList;
        }

        public JArray GetFromEhall(string json, DateTime termStartDate)
        {
            JObject jCourse = (JObject)JsonConvert.DeserializeObject(json);
            JArray courseList = new JArray();
            foreach (var course in jCourse["datas"]["xskcb"]["rows"])
            {
                for (int week = 0; week < course["SKZC"].ToString().Length; week++)
                {
                    if (course["SKZC"].ToString()[week] != '0')
                    {
                        courseList.Add(new JObject
                        {
                            {"name", course["KCM"]},
                            {"classroom", course["JASMC"]},
                            {"date", termStartDate.AddDays(week * 7 + int.Parse(course["SKXQ"].ToString()) - 1).ToString("yyyy-MM-dd")},
                            {"sectionStart", course["KSJC"]},
                            {"sectionEnd", course["JSJC"]}
                        });
                    }
                }
            }

            return courseList;
        }
    }
}
