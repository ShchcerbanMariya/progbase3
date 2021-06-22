using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ServiceLib
{
    public class User
    {
        [XmlElement("Id")]
        public int id;
        public string name;
        public string password;
        public bool isModerator;
        public List<Question> questions = new List<Question>();
        public List<Answer> answers = new List<Answer>();
        public User()
        {
            this.id = 0;
            this.name = "";
            this.password = "";
            this.isModerator = false;
            this.questions = null;
            this.answers = null;
        }
        public User(int id, string name, string password, bool isModerator)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.isModerator = isModerator;
        }
        public User(int id, string name, string password, bool isModerator, List<Question> questions, List<Answer> answers)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.isModerator = isModerator;
            this.questions = new List<Question>(questions);
            this.answers = new List<Answer>(answers);
        }
        public override string ToString()
        {
            return $"{id}, {name}";
        }
    }
}