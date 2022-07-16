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
            if(task.TaskId == 0)
            {
                task.TaskId = Database.getLastTaskId();
            }
            //task.TaskId = 0;// Tasks[Tasks.Count - 1].TaskId + 1;
            Tasks.Add(task);
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
                }
            }
        }
        public void deleteTask(Task task)
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                if (Tasks[i].TaskId == task.TaskId)
                {
                    Tasks.Remove(Tasks[i]);
                }
            }
            Database.deleteTask(task);
        }
    }
}
