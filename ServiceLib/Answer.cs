using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ServiceLib
{
    public class Answer
    {
        [XmlElement("Id")]
        public int id;
        [XmlElement("Title")]
        public string title;
        [XmlElement("Body")]
        public string body;
        [XmlElement("Author")]
        public User user;
        public Question question;
        public DateTime time;
        public Answer()
        {
            
        }
         public Answer(int id, string title, string body, DateTime time, int userId)
        {
            this.id = id;
            user = new User();
            user.id = userId;
            this.title = title;
            this.body = body;
            this.time = time;
        }
        public override string ToString()
        {
            return $"{id},{title}";
        }

    }
}