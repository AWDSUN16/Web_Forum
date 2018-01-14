using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_Forum.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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

        //[HttpGet, Route("/contents/getAllThreads")]
        //public IActionResult GetAllThreads()
        //{
        //    List<Thread> result = web_ForumDbContext.Threads.ToList();

        //    result.Sort((a, b) => b.DateOfCreation.CompareTo(a.DateOfCreation));

        //    return Ok(result);
        //}

        [HttpGet, Route("/contents/getAllThreads")]
        public IActionResult PolicyCheck()
        {
            List<Thread> result = web_ForumDbContext.Threads.ToList();

            result.Sort((a, b) => b.DateOfCreation.CompareTo(a.DateOfCreation));

            List<string> threadList = new List<string>();

            if (User.Identity.IsAuthenticated && User.HasClaim("role", "administrator"))
            {
                for (int i = 0; i < result.Count; i++)
                {
                    threadList.Add("html += '<tr>'");
                    threadList.Add("html += '<td style='border: 1px solid black;'><a href='#threadDataDiv' class='threadLink' thread-id=" + result[i].Id + ">" + result[i].Title + "</a></td>");
                    threadList.Add("html += '<td style='border: 1px solid black; '>" + result[i].Title + "</td>");
                    threadList.Add("html += '<td style='border: 1px solid black; '>" + result[i].DateOfCreation + "</td>");
                    threadList.Add("html += <td><button class='adminthreaddeleteButton' data-id='" + result[i].Id + "'>delete</button></td>");
                    threadList.Add("html += '<tr>'");
                }
                return Ok(threadList);
            }
            else
            {
                for (int i = 0; i < result.Count; i++)
                {
                    threadList.Add("html += '<tr>'");
                    threadList.Add("html += '<td style='border: 1px solid black;'><a href='#threadDataDiv' class='threadLink' thread-id=" + result[i].Id + ">" + result[i].Title + "</a></td>");
                    threadList.Add("html += '<td style='border: 1px solid black; '>" + result[i].Title + "</td>");
                    threadList.Add("html += '<td style='border: 1px solid black; '>" + result[i].DateOfCreation + "</td>");
                    threadList.Add("html += '<tr>'");
                }
                return Ok(threadList);
            }



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
            if (User.Identity.IsAuthenticated)
            {
                Post EditedPostToSaveToDatabase = web_ForumDbContext.Posts.SingleOrDefault(p => p.Id == UserEditedPost.Id);

                EditedPostToSaveToDatabase.Content = UserEditedPost.Content;

                var result = await web_ForumDbContext.SaveChangesAsync();

                return Ok("succeeded!");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet, Route("contents/posts")]
        public IActionResult GetPostById(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Post postToEdit = web_ForumDbContext.Posts.Find(id);
                return Ok(postToEdit);
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize(Policy = "AdminRights")]
        [HttpDelete, Route("/contents/adminthreaddelete")]     
        public IActionResult AdminThreadDelete(int clickedId)
        {
            string temp="";
            foreach (Thread thread in web_ForumDbContext.Threads)
            {
                if (thread.Id == clickedId)
                {
                    foreach(Post post in web_ForumDbContext.Posts)
                    {
                        if (thread.Id == post.ThreadId)
                        {
                            web_ForumDbContext.Remove(post);
                        }
                    }
                    temp= thread.Title;
                    web_ForumDbContext.Remove(thread);
                }

            }
            web_ForumDbContext.SaveChanges();

            return Ok("titel: "+temp + " borttagen");
        }


        //[HttpDelete, Route("/contents/userthreaddelete")]
        //public IActionResult UserThreadDelete(int clickedId)
        //{
        //   if(User.Identity.IsAuthenticated)
        //    {
        //        foreach (Thread thread in web_ForumDbContext.Threads)
        //        {
        //            if (User.Identity.Name == Thread[i].createdby)
        //            {
        //                remove thread
        //            }

        //        }
        //    }
           
        //    web_ForumDbContext.SaveChanges();

        //    return Ok("titel: " + temp + " borttagen");
        //}
    }
}
