using Microsoft.Data.Sqlite;
namespace EmailService.Models
{
    public static class Database
    {
        public static int inputTasks(Task task)
        {

            using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO Tasks(name, title, api, api_option, id, last_trigger, start, end, cron_period) VALUES ('{task.Name}', '{task.Title}', '{task.Api}', {task.ApiOption}, '{task.UserId}', '{task.LastTrigger}', '{task.Start}', '{task.End}','{task.CronPeriod}')";
                command.ExecuteNonQuery();
                command.CommandText = "SELECT MAX(taskId) FROM Tasks";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    return Convert.ToInt32(reader.GetValue(0));
                }
            }
        }

        private static void CreateIfNotExist()
        {
            var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate");
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = "CREATE TABLE IF NOT EXISTS Triggers(time VARCHAR(20))";
            command.ExecuteNonQuery();
            command.CommandText = "CREATE TABLE IF NOT EXISTS Users(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name VARCHAR(50) NOT NULL, email VARCHAR(50) NOT NULL UNIQUE, password VARCHAR(100) NOT NULL, isAdmin BOOL)";
            command.ExecuteNonQuery();
            command.CommandText = "CREATE TABLE IF NOT EXISTS Tasks(id INTEGER REFERENCES Users (id),name VARCHAR (50)  NOT NULL,title VARCHAR(200) NOT NULL,api INTEGER NOT NULL,api_option INTEGER NOT NULL, last_trigger VARCHAR(20)  DEFAULT 'xxx',start VARCHAR(20),end VARCHAR(20),cron_period VARCHAR(20),taskId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE)";
            command.ExecuteNonQuery();
        }

        public static List<DateTime> outputDates()
        {
            string sqlExpression = "SELECT * FROM Triggers";
            using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
            {
                connection.Open();
                List<DateTime> dates = new List<DateTime>();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime date = new DateTime();
                            date = DateTime.Parse(reader.GetString(0));
                            dates.Add(date);
                        }
                    }
                    return dates;
                }
            }
        }

        public static int getLastTaskId()
        {
            int t = 0;
            string sqlExpression = "SELECT MAX(taskId) FROM Tasks";
            using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        t = Convert.ToInt32(reader.GetValue(0));       
                    }
                    return t;
                }
            }
        }
        public static User? outputUser(User user) {
            CreateIfNotExist();
            string sqlExpression = $"SELECT * FROM Users WHERE email = '{user.Email}' AND password = '{user.Password}'";
            using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user.Id = Convert.ToInt32(reader.GetValue(0));
                        user.Name = reader.GetString(1);
                        user.Email = reader.GetString(2);
                        user.password = reader.GetString(3);
                        user.Admin = Convert.ToBoolean(reader.GetValue(4));
                        //BubbleUser = user;
                        user.Tasks = outputTask(user.Id);
                        return user;
                    }
                    else
                    {
                        return null;
                    } 
                }
            }
        }
        public static List<User> outputUsers()
        {
            CreateIfNotExist();
            string sqlExpression = "SELECT * FROM Users";
            using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
            {
                connection.Open();
                List <User> users = new List<User>();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.Id = Convert.ToInt32(reader.GetValue(0));
                            user.Name = reader.GetString(1);
                            user.Email = reader.GetString(2);
                            user.password = reader.GetString(3);
                            user.Admin = Convert.ToBoolean(reader.GetValue(4));
                            users.Add(user);
                        }  
                    }
                    return users;
                }
            }
        }
        public static void editTask(Task task)
        {
            var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate");
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"UPDATE Tasks SET name = '{task.Name}',title = '{task.Title}', api = '{task.Api}', api_option = '{task.ApiOption}', start = '{task.Start}', cron_period = '{task.CronPeriod}' WHERE TaskId = '{task.TaskId}'";
            command.ExecuteNonQuery();
        }
        public static void deleteTask(int taskId)
        {
            var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate");
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"DELETE FROM Tasks WHERE taskId = '{taskId}'";
            command.ExecuteNonQuery();
        }
        public static List<Task> outputTask(int userId)
        {
            string sqlExpression = $"SELECT * FROM Tasks WHERE id = '{userId}'";
            using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
            {
                connection.Open();
                List<Task> tasks = new List<Task>();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            Task task = new Task();
                            task.UserId = Convert.ToInt32(reader.GetValue(0));
                            task.Name = reader.GetString(1);
                            task.Title = reader.GetString(2);
                            task.Api = Convert.ToInt32(reader.GetValue(3));
                            task.ApiOption = Convert.ToInt32(reader.GetValue(4));
                            task.LastTrigger = reader.GetString(5);
                            task.Start = reader.GetString(6);
                            task.CronPeriod = reader.GetString(8);
                            task.TaskId = Convert.ToInt32(reader.GetValue(9));
                            tasks.Add(task);
                        }
                    }
                    return tasks;
                }
            }
        }
        public static bool inputUser(User user)
        {
            using (var connection = new SqliteConnection("Data Source=C:\\papka\\EmailServiceDB.db; Mode=ReadWriteCreate"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT EXISTS(SELECT * FROM Users WHERE email = '{user.Email}')";
                command.ExecuteNonQuery();
                bool u = false;

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    u = reader.GetBoolean(0);
                }
                if (!u)
                {
                    command.CommandText = $"INSERT INTO Users (name, email, password, isAdmin) VALUES ('{user.Name}', '{user.Email}', '{user.Password}', {user.Admin})";
                    command.ExecuteNonQuery();
                    
                }
                return !u;
            }
        }   
    }
}
