using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BankAccountCoDo1.Models;

namespace BankAccountCoDo1.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get;set;}
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(dbUser => dbUser.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("activate")]
        public IActionResult Activate(LoginUser userInfo)
        {
            if(ModelState.IsValid)
            {
                var dbUser = _context.Users.FirstOrDefault(user => user.Email == userInfo.Email);
                if(dbUser == null)
                {
                    ModelState.AddModelError("Email", "Email is not registered. Please use a different email or register.");
                    return View("Login");
                }
                else
                {
                    var Hasher = new PasswordHasher<LoginUser>();
                    var result = Hasher.VerifyHashedPassword(userInfo, dbUser.Password, userInfo.Password);
                    if(result == 0)
                    {
                        ModelState.AddModelError("Password", "Password is incorrect.");
                        return View("Login");
                    }
                    else
                    {
                        User loggedInUser = _context.Users.FirstOrDefault(user => user.Email == userInfo.Email);
                        HttpContext.Session.SetString("UserFirstName", loggedInUser.FirstName);
                        HttpContext.Session.SetString("UserLastName", loggedInUser.LastName);
                        HttpContext.Session.SetInt32("UserId", loggedInUser.UserId);
                        return Redirect($"account/{loggedInUser.UserId}");
                    }
                }
            }
            else
            {
                return View("Login");
            }
        }
        [HttpGet("account/{UserId}")]
        public IActionResult Account()
        {
            int? userLogged = HttpContext.Session.GetInt32("UserId");
            if(userLogged == null)
            {
                return Redirect("/");
            }
            else
            {
                ViewBag.UserFirstName = HttpContext.Session.GetString("UserFirstName");
                ViewBag.UserLastName = HttpContext.Session.GetString("UserLastName");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Transactions = _context.Users.Include(user => user.MyTransactions).FirstOrDefault(user => user.UserId == userLogged).MyTransactions.OrderByDescending(t => t.CreatedAt).ToList();
                return View("Account");
            }
            
        }
        [HttpPost("update")]
        public IActionResult Update(Transaction newTransaction)
        {
            if(ModelState.IsValid)
            {
                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();
                int? userId = HttpContext.Session.GetInt32("UserId");
                return Redirect($"/account/{userId}");
            }
            else
            {
                return View("Account");
            }
        }
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
