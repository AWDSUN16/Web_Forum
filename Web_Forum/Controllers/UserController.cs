using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Web_Forum.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_Forum.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private Web_ForumDbContext web_ForumDbContext;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Web_ForumDbContext web_ForumDbContext)
        {
            this.web_ForumDbContext = web_ForumDbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost]
        async public Task<IActionResult> CreateUser(ApplicationUser newUser, string Password)
        {
            ApplicationUser NewUser = new ApplicationUser { UserName = newUser.UserName, Email = newUser.Email };
            //NewUser.PasswordHash = userManager.PasswordHasher.HashPassword(newUser.PasswordHash);
            var result = await userManager.CreateAsync(NewUser, Password);


            return Ok(NewUser);

        }

        [HttpPost, Route("/user/login")]
        async public Task<IActionResult> SignIn(string email)
        {
            var userToSignIn = await userManager.FindByEmailAsync(email);

            await signInManager.SignInAsync(userToSignIn, true);

            return Ok(userToSignIn);
        }

        [HttpPost, Route("/user/logout")]
        async public Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();

            return Ok("");
        }



        [HttpPost, Route("/user/post")]
        async public Task<IActionResult> SavePostToDatabase(Post UserCreatedPost)
        {
            if (User.Identity.IsAuthenticated)
            {

                web_ForumDbContext.Add(UserCreatedPost);
            }
            else
            {
                return Ok("fail");
            }
            await web_ForumDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
