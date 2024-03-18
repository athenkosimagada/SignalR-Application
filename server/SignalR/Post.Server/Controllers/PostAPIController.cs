using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Post.Server.Data;
using Post.Server.Models;
using Post.Server.Models.Dto;
using Post.Server.Service.IService;
using System.Data;

namespace Post.Server.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly IUserService _userService;
        public PostAPIController(ApplicationDbContext db, IMapper mapper, 
            IUserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
            _response = new ResponseDto();
        }

        [HttpGet]
        [Route("all")]
        public  async Task<ResponseDto> GetAllPost()
        {
            try
            {
                IEnumerable<UserPost> objList = _db.Posts.ToList();

                objList = objList.Reverse();

                var userList = await _userService.GetUsers();
                foreach (var post in objList)
                {
                    post.ApplicationUser = userList.FirstOrDefault(u => u.Id == post.UserId)!;
                }
                _response.Result = _mapper.Map<IEnumerable<UserPostDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("get-user-posts/{userId}")]
        public async Task<ResponseDto> GetUserPosts(string userId)
        {
            try
            {
                IEnumerable<UserPost> objList = _db.Posts.Where(u => u.UserId == userId).ToList();

                var userList = await _userService.GetUsers();
                foreach (var post in objList)
                {
                    post.ApplicationUser = userList.FirstOrDefault(u => u.Id == userId)!;
                }

                _response.Result = _mapper.Map<IEnumerable<UserPostDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet]
        [Route("{postId:int}")]
        public async Task<ResponseDto> Get(int postId)
        {
            try
            {
                UserPost obj = _db.Posts.First(u => u.PostId == postId);

                var userList = await _userService.GetUsers();
                obj.ApplicationUser = userList.FirstOrDefault(u => u.Id == obj.UserId)!;

                _response.Result = _mapper.Map<UserPostDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Route("create-post")]
        public ResponseDto Post([FromBody] UserPostDto postDto)
        {
            try
            {
                UserPost obj = _mapper.Map<UserPost>(postDto);
                _db.Posts.Add(obj);
                _db.SaveChanges();

                _response.Result = postDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Route("update-post")]
        public ResponseDto Put([FromBody] UserPostDto postDto)
        {
            try
            {
                UserPost obj = _db.Posts.First(u => u.PostId == postDto.PostId);
                obj.LikesCount = postDto.LikesCount;
                obj.CommentsCount = postDto.CommentsCount;
                obj.Content = postDto.Content;

                _db.Posts.Update(obj);
                _db.SaveChanges();

                _response.Result = postDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("delete-post/{postId:int}")]
        public ResponseDto Delete(int postId)
        {
            try
            {
                UserPost obj = _db.Posts.First(u => u.PostId == postId);
                _db.Posts.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
