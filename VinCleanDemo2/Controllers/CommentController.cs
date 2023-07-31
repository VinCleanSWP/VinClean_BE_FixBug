using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Blog;
using VinClean.Service.DTO.Comment;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly ICommentService _service;
        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get()
        {
            return Ok(await _service.GetCommentList());
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountdDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Comment>> GetCommentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var commentFound = await _service.GetComment(id);
            if (commentFound == null)
            {
                return NotFound();
            }
            return Ok(commentFound);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(CommentDTO request)
        {

            var newComment = await _service.CreateComment(request);
            if (newComment.Success == false && newComment.Message == "Exist")
            {
                return Ok(newComment);
            }

            if (newComment.Success == false && newComment.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding comment {request}");
                return StatusCode(500, ModelState);
            }

            if (newComment.Success == false && newComment.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding comment {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newComment.Data);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateComment(CommentDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateComment = await _service.UpdateComment(request);

            if (updateComment.Success == false && updateComment.Message == "NotFound")
            {
                return Ok(updateComment);
            }

            if (updateComment.Success == false && updateComment.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating comment {request}");
                return StatusCode(500, ModelState);
            }

            if (updateComment.Success == false && updateComment.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating comment {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateComment);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var deleteComment = await _service.DeleteComment(id);


            if (deleteComment.Success == false && deleteComment.Message == "NotFound")
            {
                ModelState.AddModelError("", "Account Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteComment.Success == false && deleteComment.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting comment");
                return StatusCode(500, ModelState);
            }

            if (deleteComment.Success == false && deleteComment.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting comment");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
