using System.ComponentModel.DataAnnotations;
namespace EmailService.Models
{
    public class Task
    {

        [Required(ErrorMessage = "Please enter task name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Task title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please choose api")]
        public int Api { get; set; }
        [Required(ErrorMessage = "Please choose api-option")]
        public int ApiOption { get; set; }
        public int UserId { get; set; }
        public string LastTrigger { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string CronPeriod { get; set; }
        public int TaskId { get; set; }
    }
}
