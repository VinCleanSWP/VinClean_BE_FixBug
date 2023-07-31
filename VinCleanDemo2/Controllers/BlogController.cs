using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Blog;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
       
            private readonly IBlogService _service;

            public BlogController(IBlogService service)
            {
                _service = service;
            }

      [HttpGet]
       public async Task<ActionResult<List<BlogDTO>>> Get()
            {
                return Ok(await _service.GetBlog());
            }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Blog>> GetById(int id)
        {

            if (id <= 0)
            {
                return BadRequest(id);
            }
            var blogFound = await _service.GetBlogByID(id);
            if (blogFound == null)
            {
                return NotFound();
            }
            return Ok(blogFound);
        }
        [HttpGet("{blogId}/comments")]
        public async Task<IActionResult> GetCommentsByBlogId(int blogId)
        {
            try
            {
                var comments = await _service.GetCommentsByBlogId(blogId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog(BlogDTO request)
        {
           

            var newBlog = await _service.CreateBlog(request);
            if (newBlog.Success == false && newBlog.Message == "Exist")
            {
                return Ok(newBlog);
            }

            if (newBlog.Success == false && newBlog.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newBlog.Success == false && newBlog.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newBlog.Data);
        }
        [HttpPut]
        //public async Task<ActionResult> UpdateBlog(BlogDTO request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest(ModelState);
        //    }


        //    var updateBlog = await _service.UpdateBlog(request);

        //    if (updateBlog.Success == false && updateBlog.Message == "NotFound")
        //    {
        //        return Ok(updateBlog);
        //    }

        //    if (updateBlog.Success == false && updateBlog.Message == "RepoError")
        //    {
        //        ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
        //        return StatusCode(500, ModelState);
        //    }

        //    if (updateBlog.Success == false && updateBlog.Message == "Error")
        //    {
        //        ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
        //        return StatusCode(500, ModelState);
        //    }


        //    return Ok(updateBlog);

        //}
        public async Task<ActionResult> UpdateBlog(BlogDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateBlog = await _service.UpdateBlog(request);

            if (updateBlog.Success == false && updateBlog.Message == "NotFound")
            {
                return Ok(updateBlog);
            }

            if (updateBlog.Success == false && updateBlog.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating blog {request}");
                return StatusCode(500, ModelState);
            }

            if (updateBlog.Success == false && updateBlog.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating blog {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateBlog);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlog(int id)
        {
            var deleteBlog = await _service.DeleteBlog(id);


            

            if (deleteBlog.Success == false && deleteBlog.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Blog");
                return StatusCode(500, ModelState);
            }

            if (deleteBlog.Success == false && deleteBlog.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Blog");
                return StatusCode(500, ModelState);
            }

            return Ok(deleteBlog);

        }

    }
}

