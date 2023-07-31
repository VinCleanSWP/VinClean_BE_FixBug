using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Blog;
using VinClean.Service.DTO.Comment;

namespace VinClean.Service.Service
{
    public interface IBlogService
    {
        Task<ServiceResponse<List<BlogDTO>>> GetBlog();
        Task<ServiceResponse<BlogDTO>> CreateBlog(BlogDTO request);
        Task<ServiceResponse<BlogDTO>> UpdateBlog(BlogDTO request);
        Task<ServiceResponse<BlogDTO>> DeleteBlog(int id);
        Task<ServiceResponse<BlogDTO>> GetBlogByID(int id);
        Task<ServiceResponse<List<CommentDTO>>> GetCommentsByBlogId(int blogId);

    }
    public class BlogService: IBlogService
    {
        private readonly IBlogRepository _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        public BlogService(IBlogRepository repository, IMapper mapper, ICommentRepository commentRepository, IAccountRepository accountRepository)
        {
            _repository = repository;
            _commentRepository = commentRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        async Task<ServiceResponse<List<CommentDTO>>> IBlogService.GetCommentsByBlogId(int blogId)
        {
            
            ServiceResponse<List<CommentDTO>> _response = new();
            try
            {
                var blog = await _repository.GetBlogById(blogId);
                var comments = await _commentRepository.GetCommentsByBlogId(blogId);
                var commentDtos = new List<CommentDTO>();
                foreach (var comment in comments)
                {
                    commentDtos.Add(_mapper.Map<CommentDTO>(comment));
                    _response.Success = true;
                    _response.Message = "OK";
                    _response.Data = commentDtos;
                }

                
            }catch(Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
           
            
        }

        async Task<ServiceResponse<BlogDTO>> IBlogService.CreateBlog(BlogDTO request)
        {
            ServiceResponse<BlogDTO> _response = new();
            try
            {
                Blog _newBlog = new Blog()
                {
                    Img = request.Img,
                    Title = request.Title,
                    Sumarry = request.Sumarry,
                    Content = request.Content,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                   

                };

                if (!await _repository.CreateBlog(_newBlog))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<BlogDTO>(_newBlog);
                _response.Message = "Created";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        async Task<ServiceResponse<BlogDTO>> IBlogService.DeleteBlog(int id)
        {
            ServiceResponse<BlogDTO> _response = new();
            //try
            //{
                var existingBlog = await _repository.GetBlogById(id);
            var existingComment = await _commentRepository.GetCommentsByBlogId(id);
                if (existingBlog == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.Deleteblog(existingBlog)&& !await _commentRepository.DeleteComment((Comment)existingComment))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _blogDTO = _mapper.Map<BlogDTO>(existingBlog);
                _response.Success = true;
                _response.Data = _blogDTO;
                _response.Message = "Deleted";

            //}
            //catch (Exception ex)
            //{
            //    _response.Success = false;
            //    _response.Data = null;
            //    _response.Message = "Error";
            //    _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            //}
            return _response;
        }

        async Task<ServiceResponse<List<BlogDTO>>> IBlogService.GetBlog()
        {
            ServiceResponse<List<BlogDTO>> _response = new();
            try
            {
                var ListBlog = await _repository.GetBlogs();
                var ListBlogDTO = new List<BlogDTO>();
                foreach (var blog in ListBlog)
                {
                    ListBlogDTO.Add(_mapper.Map<BlogDTO>(blog));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListBlogDTO;
            }
            catch (Exception ex)
            {

                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        async Task<ServiceResponse<BlogDTO>> IBlogService
            .GetBlogByID(int id)
        {
            ServiceResponse<BlogDTO> _response = new();
            try
            {
                var blog = await _repository.GetBlogById(id);
                if (blog == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var blogdto = _mapper.Map<BlogDTO>(blog);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = blogdto;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        //async Task<ServiceResponse<BlogDTO>> IBlogService.UpdateBlog(BlogDTO request)
        //{
        //    ServiceResponse<BlogDTO> _response = new();
        //    try
        //    {
        //        var existingBlog = await _repository.GetBlogById(request.BlogId);
        //        if (existingBlog == null)
        //        {
        //            _response.Success = false;
        //            _response.Message = "NotFound";
        //            _response.Data = null;
        //            return _response;
        //        }
        //        existingBlog.BlogId = request.BlogId;
        //        existingBlog.Title = request.Title;
        //        existingBlog.Sumarry = request.Sumarry;
        //        existingBlog.Content = request.Content;
        //        existingBlog.CreatedBy = request.CreatedBy;


        //        if (!await _repository.UpdateBlog(existingBlog))
        //        {
        //            _response.Success = false;
        //            _response.Message = "RepoError";
        //            _response.Data = null;
        //            return _response;
        //        }

        //        var _blogDTO = _mapper.Map<BlogDTO>(existingBlog);
        //        _response.Success = true;
        //        _response.Data = _blogDTO;
        //        _response.Message = "Updated";

        //    }
        //    catch (Exception ex)
        //    {
        //        _response.Success = false;
        //        _response.Data = null;
        //        _response.Message = "Error";
        //        _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
        //    }
        //    return _response;
        //}
     
        async Task<ServiceResponse<BlogDTO>> IBlogService.UpdateBlog(BlogDTO request)
        {
            ServiceResponse<BlogDTO> _response = new();
            try
            {
                var existingBlog = await _repository.GetBlogById(request.BlogId);
                if (existingBlog == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                
                existingBlog.BlogId = request.BlogId;
                existingBlog.Img = request.Img;
                existingBlog.Title = request.Title;
                existingBlog.Sumarry = request.Sumarry;
                existingBlog.Content = request.Content;
                existingBlog.CreatedDate = DateTime.Now;
                existingBlog.Img = request.Img;


                if (!await _repository.UpdateBlog(existingBlog))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _blogDTO = _mapper.Map<BlogDTO>(existingBlog);
                _response.Success = true;
                _response.Data = _blogDTO;
                _response.Message = "Updated";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }
    }
}
    


