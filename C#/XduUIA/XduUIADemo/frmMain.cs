using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XduUIA;

namespace XduUIADemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnLoginIds_Click(object sender, EventArgs e)
        {
            Ids ids = new Ids
            (
                txtStuID.Text,
                txtPwd.Text,
                "http%3A%2F%2Fehall.xidian.edu.cn%2Flogin%3Fservice%3Dhttp%3A%2F%2Fehall.xidian.edu.cn%2Fnew%2Findex.html"
            );
            HttpClient hc = ids.Login(out Image veriImg);
            if (veriImg != null)
                pbVerify.Image = veriImg;
            else
            {
                hc.DefaultRequestHeaders.Referrer = new Uri("http://ehall.xidian.edu.cn/new/index.html");
                hc.DefaultRequestHeaders.Connection.Add("keep-alive");
                hc.DefaultRequestHeaders.ExpectContinue = false;
                hc.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                hc.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                string strRetJson = hc.GetStringAsync("http://ehall.xidian.edu.cn/jsonp/userDesktopInfo.json?type=&_=" +
                                  ids.GetTimestamp()).Result;
                JObject jRet = (JObject)JsonConvert.DeserializeObject(strRetJson);
                MessageBox.Show(
                    $"登录成功。\n\n姓名: {jRet["userName"]}\n性别: {jRet["userSex"]}\n学院: {jRet["userDepartment"]}");
            }
        }

        private void btnLoginApp_Click(object sender, EventArgs e)
        {
            XduApp ixd = new XduApp
            (
                txtStuID.Text,
                txtPwd.Text
            );
            HttpClient hc = ixd.Login();
            JObject param = new JObject
            {
                {"userId", ixd.UserId}
            };
            string strParam = ixd.BuildQuery(param);
            hc.DefaultRequestHeaders.TryAddWithoutValidation("token", ixd.Token);
            hc.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            hc.DefaultRequestHeaders.Add("Accept-Language", "cn");
            hc.DefaultRequestHeaders.Connection.Add("keep-alive");
            string strRetJson = hc.PostAsync("http://202.117.121.7:8080/baseCampus/user/getUserInfo.do", new StringContent(strParam, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
            JObject jRet = (JObject)JsonConvert.DeserializeObject(strRetJson);
            MessageBox.Show($"登录成功。\n\n姓名: {jRet["userBaseInfo"]["realName"]}\n\n学院: {jRet["userBaseInfo"]["collegeName"]}");
        }
    }
}
