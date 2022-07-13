using System.Xml.Linq;
namespace EmailService.Models;

    public class Repository
    {



        private static List<User> responses = Database.outputUsers();

        private static List<Task> taskList = new List<Task>();

        public Repository()
        {
            responses = Database.outputUsers();
            taskList = Database.outputTasks();
        }
        public static void update()
        {
            responses = Database.outputUsers();
            taskList = Database.outputTasks();
        }

        public static void addTask(Task task)
        {
            taskList.Add(task);
            Database.inputTasks(task);
        }

        public static void addResponse(User response)
        {
            responses.Add(response);
            Database.inputUsers(response);
        }

        public static List<Task> TaskList
        {
            get
            {
                return Database.outputTasks();
            }
        }

        public static List<User> Responses
            {
                get
                {
                    return responses;
                }
            }
    
        public static Task? FindTask(int taskId)
        {
            foreach(Task t in taskList)
            {
                if(t.TaskId == taskId)
                {
                    return t;
                }
            }
            return null;
        }

        public static User? FindUser(int id)
        {
            foreach(User u in Responses)
            {
                if(u.Id == id)
                {
                    return u;
                }
            }
            return null;
        }

        public static User? FindUser(string Email)
        {
            foreach (User u in Responses)
            {
                if (u.Email == Email)
                {
                    return u;
                }
            }
            return null;
        }

        public static object deleteTask(int taskId)
        {
            foreach(Task t in TaskList)
            {
                if(t.TaskId == taskId)
                {
                    TaskList.Remove(t);
                }
            }
            return null;
        }


        public static string? returnPasswordByEmail(string email)
        {
            string? password = null;
            List<User> users = new List<User>();
            users = Repository.Responses;
            foreach (User user in users)
            {
                if (user.Email == email)
                    password = user.Password;
                return password;
            }
            return null;
        }
        public static User? findUser(string email)
        {
            List<User> users = new List<User>();
            users = Repository.Responses;
            foreach (User user in users)
            {
                if (user.Email == email)
                    return user;
            }
            return null;
        }




        public static int bubbleId = -1;
        public static bool bubbleB = true;
}

