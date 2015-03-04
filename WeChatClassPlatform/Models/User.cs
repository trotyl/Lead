using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WeChatClassPlatform.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                TryGetData();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                TryGetData();
            }
        }
        public bool IsChatState { get; set; }
        public List<Exam> ExamList { get; set; }
        public List<Lesson> LessonList { get; set; }

        private void TryGetData()
        {
            if (string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(Password))
            {
                return;
            }
            using (var client = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("IDToken0", ""), 
                    new KeyValuePair<string, string>("IDToken1", Number), 
                    new KeyValuePair<string, string>("IDToken2", Password), 
                    new KeyValuePair<string, string>("IDButton", "Submit"), 
                    new KeyValuePair<string, string>("goto", "aHR0cDovL3BvcnRhbC51ZXN0Yy5lZHUuY24vbG9naW4ucG9ydGFs"), 
                    new KeyValuePair<string, string>("encoded", "true"), 
                    new KeyValuePair<string, string>("gx_charset", "UTF-8"), 
                });
                var response = client.PostAsync("https://uis.uestc.edu.cn/amserver/UI/Login", formContent).Result;
                var stringContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(stringContent);
                var allScores =
                    client.GetStringAsync(
                        "http://eams.uestc.edu.cn/eams/teach/grade/course/person!historyCourseGrade.action?projectType=MAJOR").Result;
                Console.WriteLine(allScores);
            }
        }
    }
}