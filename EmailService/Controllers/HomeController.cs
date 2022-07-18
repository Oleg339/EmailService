using EmailService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers
{
    public class HomeController : Controller
    {
        static User? user;
        static int bubbleTaskId;

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Login(User guestResponse)
        {
            user = Database.outputUser(guestResponse);
            if (user != null)
            {
                return View("TaskList", user);
            }
            else
            {
                ModelState.AddModelError("password", "Please enter correct email or password");
                return View();
            }
        }
        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View("RsvpForm");
        }
        [HttpPost]
        public ViewResult RsvpForm(User guestResponse)
        {
            if (ModelState.IsValid)
            {
                if (Database.inputUser(guestResponse))
                {
                    user = Database.outputUser(guestResponse);
                    return View("TaskList", user);
                }
                else
                {
                    ModelState.AddModelError("Email", "Email already used");
                }
            }
            
            return View();
        }

        public ViewResult Admin()
        {
            return View(new StatisticsAndUsers());
        }
        [HttpGet]
        public ViewResult EditTask(int TaskId)
        {
            bubbleTaskId = TaskId;
            Models.Task? t = user.findTask(TaskId);
            if (t == null)
            {
                return View("TaskList", user);
            }
            return View(t);
        }
        [HttpPost]
        public ViewResult EditTask(Models.Task task)
        {
            task.TaskId = bubbleTaskId;
            user.editTask(task);
            return View("TaskList", user);
        }

        [HttpGet]
        public ViewResult AddTask()
        {
            Models.Task task = new Models.Task();
            return View(task);
        }
        [HttpPost]
        public ViewResult AddTask(Models.Task task)
        {
            task.UserId = user.Id;
            user.Add(task);
            return View("TaskList", user);
        }

        public ViewResult TaskList()
        {
            return View(user);
        }

        public ViewResult Delete(int TaskId)
        {
            user.deleteTask(TaskId);
            return View("TaskList", user);
        }
    }
}