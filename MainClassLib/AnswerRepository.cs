using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Globalization;
using ServiceLib;
namespace MainClassLib
{
    public class AnswerRepository
    {
        private SqliteConnection connection;
        private string filePath;
        public AnswerRepository(SqliteConnection connection, string filePath)
        {
            this.connection = connection;
            this.filePath = filePath;
        }

        public Answer GetById(int id)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT * FROM answers WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            SqliteDataReader reader = command.ExecuteReader();
            Answer answer = new Answer();
            if (reader.Read())
            {
                answer.id = int.Parse(reader.GetString(0));
                answer.title = reader.GetString(1);
                answer.body = reader.GetString(2);
                //answer.user.id = int.Parse(reader.GetString(3));
                //answer.question.id = int.Parse(reader.GetString(4));
                answer.time = DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                // answer.time = DateTime.Parse(reader.GetString(5));
            }
            else
            {
                answer = null;
            }
            reader.Close();
            return answer;

        }
        public int DeleteById(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM answers WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
            command.CommandText = @"UPDATE questions SET main_answer_id = -1 WHERE main_answer_id = $id";
            //command.ExecuteNonQuery();
            int nChanged = command.ExecuteNonQuery();
            return nChanged;
        }
        public int Insert(Answer answer)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"INSERT INTO answers (title, body, user_id, question_id, creationTime)
            VALUES ($title, $body, $user_id,  $question_id, $creationTime);
            SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$body", answer.body);
            if (answer.title != null)
                command.Parameters.AddWithValue("$title", answer.title);
            else
                command.Parameters.AddWithValue("$title", "answer to q:" + answer.question.id);
            command.Parameters.AddWithValue("$user_id", answer.user.id);
            command.Parameters.AddWithValue("$question_id", answer.question.id);
            command.Parameters.AddWithValue("$creationTime", answer.time.ToString("yyyy-MM-dd"));
            long newId = (long)command.ExecuteScalar();
            return (int)newId;
        }
        public int DeleteAllAnswersOfQuestion(int q_id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM answers WHERE question_id = $id";
            command.Parameters.AddWithValue("$id", q_id);

            int nChanged = command.ExecuteNonQuery();
            return nChanged;
        }
        public int GetTotalPages()
        {
            const int pageSize = 10;
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM answers";
            long count = (long)command.ExecuteScalar();
            int pages = (int)count / pageSize;
            return pages;
        }
        public List<Answer> GetAllUserAns(int user_id)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT * 
        FROM answers 
        WHERE user_id = $valueX";
            command.Parameters.AddWithValue("$valueX", user_id);
            SqliteDataReader reader = command.ExecuteReader();
            List<Answer> posts = new List<Answer>();
            while (reader.Read())
            {
                Answer answer = new Answer();
                answer.id = int.Parse(reader.GetString(0));
                answer.title = reader.GetString(1);
                answer.body = reader.GetString(2);
                //answer.question = int.Parse(reader.GetString(4));
                if (!reader.IsDBNull(5))
                    if (reader.GetString(5) != "")
                        answer.time = DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                posts.Add(answer);
            }
            reader.Close();
            connection.Close();
            return posts;
        }
        public List<Answer> GetByQuestionId(int q_id)
        {
            connection.Open();
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT * 
        FROM answers 
        WHERE question_id = $question_id";
            command.Parameters.AddWithValue("$question_id", q_id);
            SqliteDataReader reader = command.ExecuteReader();
            List<Answer> posts = new List<Answer>();
            while (reader.Read())
            {
                Answer answer = new Answer();
                answer.id = int.Parse(reader.GetString(0));
                answer.title = reader.GetString(1);
                answer.body = reader.GetString(2);
                //answer.question = int.Parse(reader.GetString(4));
                if (!reader.IsDBNull(5))
                    answer.time = DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                posts.Add(answer);
            }
            reader.Close();
            return posts;
        }
        public Answer GetMainAnswer(int q_id)
        {
            SqliteCommand command = this.connection.CreateCommand();
            command.CommandText = @"SELECT * FROM answers CROSS JOIN questions WHERE questions.main_answer_id = answers.id AND questions.id = $id";
            command.Parameters.AddWithValue("$id", q_id);

            SqliteDataReader reader = command.ExecuteReader();
            Answer answer = new Answer();
            if (reader.Read())
            {
                answer.id = int.Parse(reader.GetString(0));
                answer.title = reader.GetString(1);
                answer.body = reader.GetString(2);
                //answer.user.id = int.Parse(reader.GetString(3));
                //answer.question.id = int.Parse(reader.GetString(4));
                answer.time = DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                answer = null;

            }
            reader.Close();
            return answer;
        }
        public int UpdateAnswer(Answer answer)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE answers SET body = $body WHERE id = $id;
 
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$body", answer.body);
            command.Parameters.AddWithValue("$id", answer.id);
            return (int)(long)command.ExecuteScalar();
        }
    }
}