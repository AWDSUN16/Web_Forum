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

        [HttpPost, Route("/contents/threads/{id:int}/posts")]
        async public Task<IActionResult> SavePostToDatabase(int id, Post UserCreatedPostInThread)
        {
            if (User.Identity.IsAuthenticated)
            {
                Post UserCreatedPostToSave = new Post();
                UserCreatedPostToSave.Content = UserCreatedPostInThread.Content;
                UserCreatedPostToSave.CreatedBy = User.Identity.Name;
                UserCreatedPostToSave.DateOfCreation = DateTime.Now.ToString();
                UserCreatedPostToSave.ThreadId = id;

                web_ForumDbContext.Add(UserCreatedPostToSave);
                var result = await web_ForumDbContext.SaveChangesAsync();

                return Ok(UserCreatedPostToSave);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut, Route("/contents/posts")]
        async public Task<IActionResult> EditPost(Post UserEditedPost)
        {
            if(User.Identity.IsAuthenticated)
            {
                Post EditedPostToSaveToDatabase = web_ForumDbContext.Posts.SingleOrDefault(p => p.Id == UserEditedPost.Id);

                EditedPostToSaveToDatabase.Content = UserEditedPost.Content;

                var result = await web_ForumDbContext.SaveChangesAsync();

                return Ok(EditedPostToSaveToDatabase);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete, Route("/contents/posts")]
        public IActionResult DeletePost(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Post postToDelete = web_ForumDbContext.Posts.Find(id);
                web_ForumDbContext.Remove(postToDelete);

                var result = web_ForumDbContext.SaveChangesAsync();

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("/contents/posts")]
        public IActionResult GetPostById(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Post postToEdit = web_ForumDbContext.Posts.SingleOrDefault( p => p.Id == id);
                return Ok(postToEdit);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete, Route("/contents/threads")]
        async public Task<IActionResult> DeleteThreadAndPostsInIt(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Thread threadToDelete = web_ForumDbContext.Threads.SingleOrDefault(t => t.Id == id);
                var postsToDelete = web_ForumDbContext.Posts.Where(p => p.ThreadId == threadToDelete.Id);

                foreach (Post postToDeleteInThread in postsToDelete)
                {
                    web_ForumDbContext.Remove(postToDeleteInThread);
                }

                web_ForumDbContext.Threads.Remove(threadToDelete);

                var result = await web_ForumDbContext.SaveChangesAsync();

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
