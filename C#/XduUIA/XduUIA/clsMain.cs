using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XduUIA
{
    /// <summary>
    /// 提供登录统一身份认证系统通用方法的类。
    /// </summary>
    public class Ids
    {
        /// <summary>
        /// 获取或设置用于登录的学号。
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 获取或设置用于登录的密码。
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// <para>获取或设置回调地址。接受原始 URI 或编码后的 URI。</para>
        /// <para>如果提供编码后的 URI，仅接受经过一次编码的 URI。</para>
        /// </summary>
        public string RedirectUri { get; set; }
        /// <summary>
        /// 获取或设置登录使用的 <see cref="T:System.Net.Http.HttpClient"/> 类的 Referer 标头。
        /// </summary>
        public string Referrer { get; set; }

        /// <summary>
        /// 统一身份认证系统基础 URI。
        /// </summary>
        private readonly Uri _baseUri = new Uri("http://ids.xidian.edu.cn/authserver/login");

        /// <summary>
        /// 使用指定的登录信息初始化 <see cref="Ids"/> 类。
        /// </summary>
        /// <param name="id">用于登录的学号。</param>
        /// <param name="password">用于登录的密码。</param>
        /// <param name="redirectUri">回调地址。接受原始 URI 或编码后的 URI。</param>
        /// <param name="referrer">
        /// <para>登录使用的 <see cref="T:System.Net.Http.HttpClient"/> 类的 Referer 标头。</para>
        /// <para>如果不指定，则为统一身份认证系统默认标头。</para>
        /// </param>
        public Ids(string id, string password, string redirectUri, string referrer = "http://ids.xidian.edu.cn")
        {
            Id = id;
            Password = password;
            RedirectUri = redirectUri;
            Referrer = referrer;
        }

        /// <summary>
        /// <para>使用设置的学号和密码进行统一身份认证系统登录。如果提供验证码，则一并使用。</para>
        /// <para>返回包含 Cookies 等信息的 <see cref="T:System.Net.Http.HttpClient"/> 类。</para>
        /// </summary>
        /// <param name="verificationImage">
        /// 当此方法返回时，如果登录成功，则为 <see langword="null" />；
        /// 如果登录需要验证码，则为表示验证码图像的 <see cref="T:System.Drawing.Image" /> 类。
        /// </param>
        /// <param name="verificationCode">登录时使用的验证码。如果参数为空，则登录时不使用验证码。</param>
        /// <exception cref="FormatException">登录信息格式不正确。</exception>
        /// <exception cref="ArgumentException">密码不能为空。</exception>
        /// <exception cref="Exception">登录失败。</exception>
        /// <returns>包含 Cookies 等信息的 <see cref="T:System.Net.Http.HttpClient"/> 类。</returns>
        public HttpClient Login(out Image verificationImage, string verificationCode = "")
        {
            // Check format of id, password and redirect uri
            if (!long.TryParse(Id, out _) || Id.Length != 11)
                throw new FormatException("学号格式不正确。");
            if (Password == "")
                throw new ArgumentException("密码不能为空。");
            if (!CheckUri(HttpUtility.UrlDecode(RedirectUri)))
                throw new FormatException("回调地址格式不正确。");
            if (!CheckUri(Referrer))
                throw new FormatException("Referer 标头格式不正确。");
            HttpClient hc = new HttpClient();
            // Add HttpClient headers
            hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.92 Safari/537.36");
            hc.DefaultRequestHeaders.ExpectContinue = false;
            hc.DefaultRequestHeaders.Connection.Add("keep-alive");
            // Build get params
            UriBuilder builder = new UriBuilder(_baseUri);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["service"] = HttpUtility.UrlDecode(RedirectUri);
            builder.Query = query.ToString();
            string strLoginUri = builder.ToString();
            // Load login page
            string strHtmlContent = hc.GetStringAsync(builder.ToString()).Result;
            // Get extra params
            string strLt = Regex.Match(strHtmlContent, @"<input type=""hidden"" name=""lt"" value=""(.*?)""/>").Groups[1].Value;
            string strExecution = Regex.Match(strHtmlContent, @"<input type=""hidden"" name=""execution"" value=""(.*?)""/>").Groups[1].Value;
            string strEventId = Regex.Match(strHtmlContent, @"<input type=""hidden"" name=""_eventId"" value=""(.*?)""/>").Groups[1].Value;
            string strRmShown = Regex.Match(strHtmlContent, @"<input type=""hidden"" name=""rmShown"" value=""(.*?)"">").Groups[1].Value;
            // Set referer header
            hc.DefaultRequestHeaders.Referrer = new Uri(Referrer);
            // Build login params
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", Id),
                new KeyValuePair<string, string>("password", Password),
                new KeyValuePair<string, string>("submit", ""),
                new KeyValuePair<string, string>("lt", strLt),
                new KeyValuePair<string, string>("execution", strExecution),
                new KeyValuePair<string, string>("_eventId", strEventId),
                new KeyValuePair<string, string>("rmShown", strRmShown)
            };
            string strReturn = hc.PostAsync(strLoginUri, new FormUrlEncodedContent(paramList)).Result.Content.ReadAsStringAsync().Result;
            try
            {
                // Login successfully
                verificationImage = null;
                // Verification required
                if (hc.GetStringAsync("http://ids.xidian.edu.cn/authserver/needCaptcha.html?username=" + Id + @"&_=" + GetTimestamp()).Result == "true" || strReturn.Contains("请输入验证码"))
                {
                    verificationImage =
                        Image.FromStream(hc.GetStreamAsync("http://ids.xidian.edu.cn/authserver/captcha.html").Result);
                    return hc;
                }
                // Id or password is invalid
                if (strReturn.Contains("有误"))
                    throw new Exception("学号或密码不正确。");

                return hc;
            }
            catch (Exception e)
            {
                // Other exception
                throw new Exception($"登录失败。\n{e.Message}");
            }
        }

        /// <summary>
        /// 检查指定的 URI 是否合法。
        /// </summary>
        /// <param name="uri">要检查的 URI。</param>
        /// <returns>指示 URI 是否合法。</returns>
        private static bool CheckUri(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// 获取表示当前时间的 Unix 时间戳。
        /// </summary>
        /// <returns>表示 Unix 时间戳的字符串。</returns>
        public long GetTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }

    /// <summary>
    /// 提供登录 i 西电通用方法的类。
    /// </summary>
    public class XduApp
    {
        /// <summary>
        /// 获取或设置用于登录的学号。
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 获取或设置用于登录的密码。
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 获取 i 西电所有请求所需的 Uuid。
        /// </summary>
        public string Uuid { get; }
        /// <summary>
        /// 获取已登录用户的 userId。
        /// </summary>
        public int UserId { get; private set; }
        /// <summary>
        /// 获取本次登录会话 Token。
        /// </summary>
        public string Token { get; private set; }
        /// <summary>
        /// 获取应用密钥。
        /// </summary>
        public readonly string AppKey = "GiITvn";
        /// <summary>
        /// 获取学校 ID。
        /// </summary>
        public readonly int SchoolId = 190;

        /// <summary>
        /// 使用指定的登录信息初始化 <see cref="XduApp"/> 类。
        /// </summary>
        /// <param name="id">用于登录的学号。</param>
        /// <param name="password">用于登录的密码。</param>
        public XduApp(string id, string password)
        {
            Id = id;
            Password = password;
            Uuid = GetUuid();
        }

        /// <summary>
        /// <para>使用设置的学号和密码进行 i 西电系统登录。如果提供验证码，则一并使用。</para>
        /// <para>返回包含 Cookies 等信息的 <see cref="T:System.Net.Http.HttpClient"/> 类。</para>
        /// </summary>
        /// <exception cref="FormatException">登录信息格式不正确。</exception>
        /// <exception cref="ArgumentException">密码不能为空。</exception>
        /// <exception cref="Exception">登录失败。</exception>
        /// <returns>包含 Cookies 等信息的 <see cref="T:System.Net.Http.HttpClient"/> 类。</returns>
        public HttpClient Login()
        {
            // Check format of id, password and redirect uri
            if (!long.TryParse(Id, out _) || Id.Length != 11)
                throw new FormatException("学号格式不正确。");
            if (Password == "")
                throw new ArgumentException("密码不能为空。");
            HttpClient hc = new HttpClient();
            // Set HttpClient up
            hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 8.0; Pixel 2 Build/OPD3.170816.012) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Mobile Safari/537.36");
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            hc.DefaultRequestHeaders.ExpectContinue = false;
            hc.DefaultRequestHeaders.Referrer = new Uri("http://wx.xidian.edu.cn/wx_xdu/");
            hc.DefaultRequestHeaders.Add("Origin", "http://wx.xidian.edu.cn");
            hc.DefaultRequestHeaders.Add("token", "");
            hc.DefaultRequestHeaders.Host = "202.117.121.7:8080";
            hc.DefaultRequestHeaders.Connection.Add("keep-alive");
            if (!hc.DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json")))
                hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Build login params
            JObject param = new JObject
            {
                {"userName", Id},
                {"password", Password},
                {"uuId", Uuid},
                {"schoolId", SchoolId}
            };
            string loginParams = BuildQuery(param);
            string strReturn = hc.PostAsync("http://202.117.121.7:8080/baseCampus/login/login.do", new StringContent(loginParams, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
            try
            {
                JObject jRet = (JObject)JsonConvert.DeserializeObject(strReturn);
                // Login successfully
                if (jRet["msg"].ToString() != "登录成功")
                {
                    throw new Exception($"登录失败。\n{jRet["msg"]}");
                    //MessageBox.Show($"{jRet["userBaseInfo"]["realName"]}, {jRet["userLoginInfo"]["pid"]}, {txtID.Text}, {pass}");
                }
                // Id or password is invalid
                if (strReturn.Contains("有误"))
                    throw new Exception("学号或密码不正确。");

                UserId = jRet["userBaseInfo"]["userId"].ToObject<int>();
                Token = jRet["token"][0] + "_" + jRet["token"][1];
                return hc;
            }
            catch (Exception e)
            {
                // Other exception
                throw new Exception($"登录失败。\n{e.Message}");
            }
        }

        /// <summary>
        /// 获取表示当前时间的 Unix 时间戳。
        /// </summary>
        /// <returns>表示 Unix 时间戳的长整型。</returns>
        public long GetTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 生成用于 i 西电的随机 Uuid。
        /// </summary>
        /// <returns>表示 Uuid 的字符串。</returns>
        private string GetUuid()
        {
            string timestamp = GetTimestamp().ToString();
            string part1 = (Convert.ToString(long.Parse(new Random().NextDouble().ToString(CultureInfo.InvariantCulture).Substring(2, 8) + timestamp.Substring(timestamp.Length - 10, 10)), 16) + "").Substring(0, 8);
            timestamp = GetTimestamp().ToString();
            string part2 = (Convert.ToString(long.Parse(new Random().NextDouble().ToString(CultureInfo.InvariantCulture).Substring(2, 8) + timestamp.Substring(timestamp.Length - 10, 10)), 16) + "").Substring(0, 8);
            return "web" + part1 + part2;
        }

        /// <summary>
        /// 生成在 i 西电所有请求所需的签名。
        /// </summary>
        /// <param name="param">要签名的 param 对象。</param>
        /// <returns>表示签名的字符串。</returns>
        public string GetSign(JObject param)
        {
            return Md5Encrypt32(JsonToQuery(new JObject(param.Properties().OrderBy(p => p.Name))));
        }

        /// <summary>
        /// 生成 32 位大写 MD5。
        /// </summary>
        /// <param name="input">要加密的内容。</param>
        /// <returns>MD5 字符串。</returns>
        private static string Md5Encrypt32(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte t in data)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        /// 构造请求参数。
        /// </summary>
        /// <param name="param">请求参数中的 param 对象。</param>
        /// <param name="specificSchool">是否在请求参数中加入 schoolId。</param>
        /// <param name="acceptSecure">是否对请求参数中 param 对象进行加密。</param>
        /// <returns>构造的请求参数字符串。</returns>
        public string BuildQuery(JObject param, bool specificSchool = false, bool acceptSecure = false)
        {
            JObject queryPost = new JObject
            {
                {"appKey", AppKey},
                {"param",  param.ToString(Formatting.None)},
                {"time", GetTimestamp()},
                {"secure", specificSchool ? 1 : 0}
            };
            if (acceptSecure)
                queryPost.Add("acceptSecure", "aes");
            if (specificSchool)
                queryPost.Add("schoolId", SchoolId);
            string sign = GetSign(queryPost);
            queryPost.Add("sign", sign);
            // TODO: Add encryption method when acceptSecure it true
            return queryPost.ToString(Formatting.None);
        }

        /// <summary>
        /// 转换 <see cref="JObject"/> 对象为 GET 请求参数字符串。不进行 URL 编码。
        /// </summary>
        /// <param name="param">要转换的 <see cref="JObject"/> 对象</param>
        /// <returns>GET 请求参数字符串。</returns>
        public string JsonToQuery(JObject param)
        {
            return string.Join("&",
                param.Children().Cast<JProperty>()
                    .Select(jp => jp.Name + "=" + jp.Value.ToString()));
        }
    }
}
