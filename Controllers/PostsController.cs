using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webAPIDevelopment.Models;
using webAPIDevelopment.Services;

namespace webAPIDevelopment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postsService;
        public PostsController(IPostService postService){
            _postsService = new PostsService();
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> GetPost(int id){
            var post = await _postsService.GetPost(id);
            if(post==null){
                return NotFound();
            }
            return Ok(post);
        }
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(Post post){
            Console.Write(post);
            await _postsService.CreatePost(post);
            return CreatedAtAction(nameof(GetPost),new {id = post.Id},post);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePost(int id, Post post){
            if(id !=post.Id){
                return BadRequest();
            }
            var updatePost = await _postsService.UpdatePost(id,post);
            if(updatePost == null){
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts(){
            var posts = await _postsService.GetAllPosts();
            return Ok(posts);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id){
            await _postsService.DeletePost(id);
            return Ok();

        }

    }
}
