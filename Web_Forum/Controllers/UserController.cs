using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Web_Forum.Entities;
using Microsoft.AspNetCore.Authorization;

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
            if (!User.Identity.IsAuthenticated)
            {
                var userToSignIn = await userManager.FindByEmailAsync(email);

                await signInManager.SignInAsync(userToSignIn, true);

                return Ok(userToSignIn);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost, Route("/user/logout")]
        async public Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
                return Ok(User.Identity.Name);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost, Route("/user/post")]
        async public Task<IActionResult> SavePostToDatabase(Post UserCreatedPost)
        {
            if (User.Identity.IsAuthenticated)
            {
                UserCreatedPost.CreatedBy=User.Identity.Name;
                UserCreatedPost.DateOfCreation = DateTime.Now.ToString();
                web_ForumDbContext.Add(UserCreatedPost);
                await web_ForumDbContext.SaveChangesAsync();
                return Ok(User.Identity.Name);
            }
            else
            {
                return BadRequest();
            }


        }

        [HttpGet, Route("/user/showallposts")]
         public IActionResult ShowAllPosts()
        {
            //List<Post> myList = new List<Post>();
            //foreach (var post in web_ForumDbContext.Posts)
            //{
            //    myList.Add(post);

            //}
            return Ok(web_ForumDbContext.Posts);
        }

        [HttpGet, Route("/user/checkIfUserIsAuthenticated")]
        public IActionResult CheckIfUserIsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = userManager.GetUserAsync(User);

                return Ok(User.Identity.Name);
            }
            else
            {
                return Ok("Logga in");
            }
        }

        [HttpPost, Route("/user/createadmin")]
       async public Task <IActionResult> CreateAdmin()
        {
            ApplicationUser admin = new ApplicationUser { UserName = "admin", Email = "admin@admin.com"};
            await userManager.CreateAsync(admin,"123456aA!");
            await userManager.AddClaimAsync(admin, new System.Security.Claims.Claim("role", "administrator"));
            return Ok();
        }
        [Authorize(Policy = "AdminRights")]
        [HttpPost, Route("/user/test")]
        public IActionResult Test()
        {
        
            return Ok();
        }

    }
}
