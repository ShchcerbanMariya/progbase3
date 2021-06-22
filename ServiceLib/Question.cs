using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ServiceLib
{
      public class Question
    {
        [XmlElement("Id")]
        public int id;
        [XmlElement("Title")]
        public string title;
        [XmlElement("Body")]
        public string body;
        [XmlElement("Author")]
        public User user;
        [XmlIgnore]
        public List<Answer> listAnswers = new List<Answer>();
        [XmlElement("MainAnswer")]
        public Answer mainAnswer = null;
        public DateTime start;
        public DateTime end = new DateTime();
        public Question()
        {

        }
        public override string ToString()
        {
            return $"{id},{title},{user}";
        }

    }
}