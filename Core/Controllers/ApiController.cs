using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Core.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiController : ControllerBase
    { 

        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 获取文件路径内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string  GetFile(string fileName,bool isBase64 =true)
        {
            try
            { 
                string str = "";
                var path = Directory.GetCurrentDirectory();
                path = Path.Combine(path, fileName);
                if (!System.IO.File.Exists(path))
                    throw new Exception($"文件名{fileName}不存在");
                FileStream fileStream = new FileStream(path, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    str = reader.ReadToEnd();
                }
                
                if (isBase64)
                {
                    return Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(str));
                }
                else
                {
                    return str;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            } 
        }
        /// <summary>
        /// 获取网络路径内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetUrl(string url,bool isBase64=true)
        {
            try
            { 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close(); 
                if (isBase64)
                {
                    return Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(retString));
                }
                else
                {
                    return retString;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
