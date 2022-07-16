using System.ComponentModel.DataAnnotations;
namespace EmailService.Models
{
    public class User
    {
        public string password;
        private string email;
        private int id = 1;

        [Required(ErrorMessage = "Something wrong")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter your email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email adress")]
        public string Email { get => email; set => email = value; }
        [Required(ErrorMessage = "Please Enter password")]
        public string Password { get => password; set => password = PasswordHashing.GetHash(value); }
        public bool Admin { get; set; }
        public int Id { get => id; set => id = value; }
        public List<Task> Tasks { get; set; }
        public IEnumerator<Task> GetEnumerator() => Tasks.GetEnumerator();

        public void Add(Task task)
        {
            task.TaskId = Database.inputTasks(task);
            Tasks.Add(task);
        }
        public Task? findTask(int taskId)
        {
            foreach(Task t in Tasks)
            {
                if(t.TaskId == taskId)
                {
                    return t;
                }
            }
            return null;
        }
        //public void TaskToUser()
        //{
        //    Tasks = Repository.UserToTasks(this);
        //}
        public User()
        {
            Tasks = new List<Task>();
        } 
        public void editTask(Task task)
        {
            for(int i = 0; i < Tasks.Count; i++)
            {
                if(Tasks[i].TaskId == task.TaskId)
                {
                    Tasks[i] = task;
                    Database.editTask(task);
                }
            }
        }
        public void deleteTask(int taskId)
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                if (Tasks[i].TaskId == taskId)
                {
                    Tasks.Remove(Tasks[i]);
                    Database.deleteTask(taskId);
                }
            }
            
        }
    }
}
