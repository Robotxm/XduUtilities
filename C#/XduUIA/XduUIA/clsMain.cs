using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace XduUIA
{
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
        /// 获取或设置回调地址。接受原始 URI 或编码后的 URI。
        /// </summary>
        public string RedirectUri { get; set; }

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
        public Ids(string id, string password, string redirectUri)
        {
            Id = id;
            Password = password;
            RedirectUri = redirectUri;
        }

        /// <summary>
        /// 初始化新的空 <see cref="Ids"/> 类。
        /// </summary>
        public Ids()
        {
            Id = "";
            Password = "";
            RedirectUri = "";
        }
        /// <summary>
        /// <para>使用设置的学号和密码进行统一身份认证系统登录。如果提供验证码，则一并使用。</para>
        /// <para>返回包含 Cookies 等信息的 HttpClient 类。</para>
        /// </summary>
        /// <param name="verificationImage">当此方法返回时，如果登录成功，则为 <see langword="null" />；如果登录需要验证码，则为表示验证码图像的 <see cref="T:System.Drawing.Image" /> 类。</param>
        /// <param name="client">上一次登录返回的 <see langword="null" /> 类。如果参数为 <see langword="null" /></param>
        /// <param name="verificationCode">登录时使用的验证码。如果参数为空，则登录时不使用验证码。</param>
        /// <exception cref="FormatException">登录信息格式不正确。</exception>
        /// <exception cref="ArgumentException">密码不能为空。</exception>
        /// <exception cref="Exception">登录失败。</exception>
        /// <returns>包含 Cookies 等信息的 HttpClient 类。</returns>
        public HttpClient Login(out Image verificationImage, HttpClient client = null, string verificationCode = "")
        {
            // Check format of id, password and redirect uri
            if (!long.TryParse(Id, out _) || Id.Length != 11)
                throw new FormatException("学号格式不正确。");
            if (Password == "")
                throw new ArgumentException("密码不能为空。");
            if (!CheckUri(HttpUtility.UrlDecode(RedirectUri)))
                throw new FormatException("回调地址格式不正确。");
            HttpClient hc = new HttpClient();
            // Set HttpClient up
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
            hc.DefaultRequestHeaders.Referrer = new Uri("http://ids.xidian.edu.cn");
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
        /// <returns>返回 bool 指示 URI 是否合法。</returns>
        private static bool CheckUri(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// 检查指定的字符串是否包含中文。
        /// </summary>
        /// <param name="input">要检查的字符串。</param>
        /// <returns>返回 bool 指示字符串是否包含中文。</returns>
        private static bool ContainsChinese(string input)
        {
            return Regex.IsMatch(input, "[\u4e00-\u9fbb]");
        }

        /// <summary>
        /// 获取表示当前时间的 Unix 时间戳。
        /// </summary>
        /// <returns>返回 <see langword="long"/> 表示 Unix 时间戳。</returns>
        public long GetTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
