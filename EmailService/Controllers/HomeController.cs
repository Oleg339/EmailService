using EmailService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Login(User guestResponse)
        {
            Repository.update();
            if (guestResponse.password != null &&
                Repository.returnPasswordByEmail(guestResponse.Email) == guestResponse.Password
                && guestResponse.Email != null)
            {
                guestResponse = Repository.findUser(guestResponse.Email);
                Repository.BubbleUserId = guestResponse.Id;
                return View("TaskList", guestResponse);
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
            var check = Repository.returnPasswordByEmail(guestResponse.Email);
            if (check != null)
                ModelState.AddModelError("Email", "Email already used");
            if (ModelState.IsValid)
            {
                Repository.addResponse(guestResponse);
                Repository.update();
                User u = Repository.FindUser(guestResponse.Email);
                Repository.BubbleUserId = u.Id;
                return View("TaskList", Repository.FindUser(guestResponse.Email));
            }
            else
            {
                return View();
            }
        }

        public ViewResult Admin()
        {
            return View(new StatisticsAndUsers());
        }
        public ViewResult EditTask(int TaskId)
        {
            Repository.bubbleId = TaskId;
            Repository.bubbleB = false;  //Edit task NOT add
            return View(Repository.FindTask(TaskId));
        }
        [HttpGet]
        public ViewResult AddTask(int UserId)
        {
            Models.Task task = new Models.Task();
            task.UserId = UserId;
            Repository.bubbleId = UserId;
            return View(task);
        }
        [HttpPost]
        public ViewResult AddTask(Models.Task task)
        {
            if(Repository.bubbleId < 0)
            {
                return View("TaskList", Repository.FindUser(Repository.BubbleUserId));
            }
            if(Repository.bubbleB)
            {   
                task.UserId = Repository.bubbleId;
                Repository.bubbleId = -1;
                User u = Repository.FindUser(task.UserId);
                Repository.addTask(task);
                u.Add(task);
                return View("TaskList", u);
            }
            else
            {
                task.TaskId = Repository.bubbleId;
                Models.Task i = Repository.FindTask(task.TaskId);
                User r = Repository.FindUser(i.UserId);
                task.UserId = r.Id;
                Database.editTask(task);
                r.editTask(task);
                Repository.bubbleId = -1;
                Repository.bubbleB = true;
                return View("TaskList", r);
            }
        }
        public ViewResult TasksList(int user)
        {
            return View(Repository.FindUser(user));
        }
        public ViewResult Delete(int TaskId)
        {
            Repository.bubbleB = true;
            Models.Task i = Repository.FindTask(TaskId);
            User r = Repository.FindUser(i.UserId);
            r.deleteTask(i);
            return View("TaskList", r);
        }
    }
}