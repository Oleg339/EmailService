using Microsoft.Data.Sqlite;
namespace EmailService.Models
{
    public static class Database
    {
        public static void inputTasks(Task task)
        {

            using (var connection = new SqliteConnection(@"Data Source=EmailServiceDB.db; Mode=ReadWrite"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO Tasks(name, title, api, api_option, id, last_trigger, start, end, cron_period) VALUES ('{task.Name}', '{task.Title}', '{task.Api}', {task.ApiOption}, '{task.UserId}', '{task.LastTrigger}', '{task.Start}', '{task.End}','{task.CronPeriod}')";
                command.ExecuteNonQuery();
            }
        }
        public static void inputUsers(User user)
        {
            using (var connection = new SqliteConnection(@"Data Source=EmailServiceDB.db; Mode=ReadWrite"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO Users (name, email, password, isAdmin) VALUES ('{user.Name}', '{user.Email}', '{user.Password}', {user.Admin})";
                command.ExecuteNonQuery();
            }
       }
        private static void CreateIfNotExist()
        {
            var connection = new SqliteConnection(@"Data Source=EmailServiceDB.db; Mode=ReadWriteCreate");
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = "CREATE TABLE IF NOT EXISTS Users(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name VARCHAR(50) NOT NULL, email VARCHAR(50) NOT NULL, password VARCHAR(100) NOT NULL, isAdmin BOOL)";
            command.ExecuteNonQuery();
            command.CommandText = "CREATE TABLE IF NOT EXISTS Tasks(id INTEGER REFERENCES Users (id),name VARCHAR (50)  NOT NULL,title VARCHAR(200) NOT NULL,api INTEGER NOT NULL,api_option INTEGER NOT NULL, last_trigger VARCHAR(20)  DEFAULT 'xxx',start VARCHAR(20),end VARCHAR(20),cron_period VARCHAR(20),taskId INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE)";
            command.ExecuteNonQuery();
        }

        public static List<User> outputUsers()
        {
            CreateIfNotExist();
            string sqlExpression = "SELECT * FROM Users";
            using (var connection = new SqliteConnection("Data Source=EmailServiceDB.db; Mode=ReadWriteCreate"))
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
            var connection = new SqliteConnection(@"Data Source=EmailServiceDB.db; Mode=ReadWrite");
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"UPDATE tasks SET name = '{task.Name}',title = '{task.Title}', api = '{task.Api}', api_option = '{task.ApiOption}', start = '{task.Start}', cron_period = '{task.CronPeriod}' WHERE id = '{task.UserId}'";
            command.ExecuteNonQuery();
        }
        public static void deleteTask(Task task)
        {
            var connection = new SqliteConnection(@"Data Source=EmailServiceDB.db; Mode=ReadWrite");
            connection.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"DELETE FROM tasks WHERE taskId = '{task.TaskId}'";
            command.ExecuteNonQuery();
        }
        public static List<Task> outputTasks()
        {
            string sqlExpression = "SELECT * FROM Tasks";
            using (var connection = new SqliteConnection("Data Source=EmailServiceDB.db; Mode=ReadWrite"))
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
                            foreach(User r in Repository.Responses)
                            {
                                if(r.Id == task.UserId)
                                {
                                    r.Add(task);
                                }
                            }

                        }
                    }
                    return tasks;
                }
            }
        }

        
    }
}
