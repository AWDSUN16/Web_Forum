using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_Forum.Entities;
using Microsoft.AspNetCore.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_Forum.Controllers
{
    [Route("contents")]
    public class ContentsController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private Web_ForumDbContext web_ForumDbContext;

        public ContentsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Web_ForumDbContext web_ForumDbContext)
        {
            this.web_ForumDbContext = web_ForumDbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost, Route("/contents")]
        async public Task<IActionResult> SaveThreadAndPostToDatabase(Thread UserCreatedThread, Post UserCreatdedOriginalPost)
        {
            if (User.Identity.IsAuthenticated)
            {
                UserCreatedThread.DateOfCreation = DateTime.Now.ToString();

                web_ForumDbContext.Add(UserCreatedThread);
                await web_ForumDbContext.SaveChangesAsync();

                UserCreatdedOriginalPost.CreatedBy = User.Identity.Name;
                UserCreatdedOriginalPost.DateOfCreation = DateTime.Now.ToString();
                UserCreatdedOriginalPost.ThreadId = UserCreatedThread.Id;

                web_ForumDbContext.Add(UserCreatdedOriginalPost);
                
                await web_ForumDbContext.SaveChangesAsync();

                return Ok(UserCreatedThread);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("/contents/threads")]
        public IActionResult GetThread(int id)
        {
            var result = web_ForumDbContext.Threads.SingleOrDefault(t => t.Id == id);

            return Ok(result);
        }

        [HttpGet, Route("/contents/threads/{id:int}/posts")]
        public IActionResult GetThreadPosts(int id)
        {
            var result = web_ForumDbContext.Posts.Where(p => p.ThreadId == id);

            return Ok(result);
        }

        //[HttpDelete, Route("/contents/threads/{id}")]
        //public IActionResult DeleteThread(int id)
        //{
        //    var threadToDelete = web_ForumDbContext.Threads.SingleOrDefault(t => t.Id == id);

        //    var result = web_ForumDbContext.Threads.Remove(threadToDelete);

        //    return Ok(result);
        //}

        //[HttpPut, Route("/contents/threads/{id}")]
        //public IActionResult UpdateThread(Thread ThreadToUpdate)
        //{
        //    var threadToUpdate = web_ForumDbContext.Threads.SingleOrDefault(t => t.Id == ThreadToUpdate.Id);

        //    threadToUpdate.Title = threadToUpdate.Title;
        //    threadToUpdate.DateOfLastUpdate = DateTime.Now.ToString();


        //    return Ok("");
        //}

        [HttpGet, Route("/contents/getAllThreads")]
        public IActionResult GetAllThreads()
        {
            List<Thread> result = web_ForumDbContext.Threads.ToList();

            result.Sort((a, b) => b.DateOfCreation.CompareTo(a.DateOfCreation));

            return Ok(result);
        }

        //[HttpGet, Route("")]
    }
}
