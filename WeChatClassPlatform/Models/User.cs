using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatClassPlatform.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public bool IsChatState { get; set; }
        public string Password { get; set; }
        public List<Exam> ExamList { get; set; }
        public List<Lesson> LessonList { get; set; }
    }
}