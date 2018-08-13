using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace WebCore_pc.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }


        [HttpPost]
        public JsonResult textsearch()
        {
            SearchHtml();
            return Json(new { f = false, mes = "" });
        }


        #region code_function

        public static bool SearchHtml()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string url = string.Empty;
            //IRequestSizePolicy requestSize;

            //注册gb2312 

            //反射
            WebClient wc = new WebClient();
            for (int i = 1; i<= 400; i++)
            { 
                string add = "";
                wc.BaseAddress = "http://www.fushu365.com/book/xddm/";
                wc.Encoding = System.Text.Encoding.GetEncoding("GB2312");
                if (i == 1)
                {
                    add = "";
                }
                else
                {
                    add = string.Format("index_{0}.html", i);
                }
                //HtmlDocument doc = new HtmlDocument(); 
            
                HtmlDocument doccc = new HtmlDocument();
                string html = wc.DownloadString(add);
                doccc.LoadHtml(html); 
                var nodes = doccc.DocumentNode.SelectNodes("//*[@id=\"p2\"]/div[3]/ul/li");  
                foreach(var n in nodes)
                {
                    try
                    {
                        string name = @"《(.*?)》";

                        string pattern = @"/book/(.*?).html";
                        var math = Regex.Match(n.OuterHtml, pattern);
                        var id = math.Groups[1].Value;

                        var math2 = Regex.Match(n.InnerText, name);

                        string urls = "http://www.fushu365.com/e/DownSys/doaction.php?enews=DownSoft&classid=1&id=" + id + "&pathid=0&pass=4f25921ec4d3d8b45bf82230156a474e&p=:::";
                        string d = math2.Groups[1].Value;
                        string p = Directory.GetCurrentDirectory() + "\\download\\";
                        DownFile(urls, p, d);
                    }
                    catch (Exception)
                    {

                    }

                }
                
            } 
            return true;
            //  HtmlNode node = doc.DocumentNode.SelectSingleNode("/html/body/div[4]/div[1]/div[2]/ul[1]"); 

        }

        public static bool writeToText(string path, string name, string text)
        {
            try
            {
                string fp = string.Format("{0}\\{1}.txt", path, name);
                if (!System.IO.File.Exists(fp))
                    {
                    FileStream fs1 = new FileStream(fp, FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(text);//开始写入值
                    sw.Close();
                    fs1.Close();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public static string HttpDownloadFile(string url, string param, string path)
        {
            byte[] bs = Encoding.UTF8.GetBytes(param);
            // 设置参数
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            System.Net.ServicePointManager.Expect100Continue = false;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            //发送请求并获取相应回应数据
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();

            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);

            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return path;
        }


        public static void DownFile(string url, string path, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2) AppleWebKit/525.13 (KHTML, like Gecko) Chrome/0.2.149.27 Safari/525.13";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            // request.Referer = "http://pinyin.sogou.com/dict/list.php?c=180";
            request.Referer = "http://pinyin.sogou.com/dict/cell.php?id=19431";

            request.KeepAlive = false;
            request.Timeout = 2000;
            //request.ContentType="text/plain";
            request.ProtocolVersion = HttpVersion.Version10;

            HttpWebResponse response;
            Stream resStream;
            response = (HttpWebResponse)request.GetResponse();
            resStream = response.GetResponseStream();

            int count = (int)response.ContentLength;
            int offset = 0;
            byte[] buf = new byte[count];
            while (count > 0)
            {
                int n = resStream.Read(buf, offset, count);
                if (n == 0)
                    break;
                count -= n;
                offset += n;
            }

            if (!System.IO.File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileStream fs = new FileStream(path + "\\" + fileName + ".txt", FileMode.Create, FileAccess.Write);
            fs.Write(buf, 0, buf.Length);
            fs.Flush();
            fs.Close();
            //Thread.Sleep(88000);
        }

        #endregion

        #region code_build
        
        public JsonResult textremove()
        {
            int x = 0;
            var u = double.TryParse(x.ToString(), out double y);
            int.TryParse(y.ToString(),out x);
            return Json(new { f = false, msg = ""});
        }


        #endregion

        #region code_test
      
        public static string search()
        {
            return "YhisIsvoid";
        }

        #endregion
    }
}