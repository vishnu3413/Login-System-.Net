using Login_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;


namespace Login_System.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IMongoCollection<User> _usersCollection;
        public HomeController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("test");
            _usersCollection = database.GetCollection<User>("users");
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "Login System";
            ViewBag.Logout = TempData["LogoutMessage"];
            return View("Base");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("user")!=null)   
            {
                ViewBag.User = HttpContext.Session.GetString("user");
                return View("Dashboard");
            }
            else
            {
                return Content("Unauthorized Access");
            }
        }

        [HttpPost]

        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var filter = Builders<User>.Filter.Eq(u => u.Email, model.Email) & Builders<User>.Filter.Eq(u => u.Password, model.Password);
                var user = _usersCollection.Find(filter).SingleOrDefault();

                if (user != null)
                {
                    HttpContext.Session.SetString("user", user.Email);   
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.Title = "Login";
                    ViewBag.Logout = "Invalid Email or Password";
                    return View("Base");
                }
            }

            return View("Base",model);
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    Email = model.Email,
                    Password = model.Password,
                };
                _usersCollection.InsertOne(newUser);
                TempData["LogoutMessage"] = "Registered Successfully";
                 return RedirectToAction("Index");

            }
            
            return View("Register", model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewBag.Title = "Login System";
            ViewBag.Logout = "Successfully Logout";
            return View("Base");
        }
    }
}
