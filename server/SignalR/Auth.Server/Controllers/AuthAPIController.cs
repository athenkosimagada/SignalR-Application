using Auth.Server.Models;
using Auth.Server.Models.Dto;
using Auth.Server.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static Azure.Core.HttpHeader;

namespace Auth.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMessageService _messageService;
        private IMapper _mapper;
        private ResponseDto _response;

        public AuthAPIController(IAuthService authService, IMapper mapper, IMessageService messageService)
        {
            _authService = authService;
            _messageService = messageService;
            _response = new();
            _mapper = mapper;
        }

        [HttpGet("GetUsers")]
        public async Task<ResponseDto> GetUsers()
        {
            try
            {
                IEnumerable<ApplicationUser> objList = await _authService.GetUsers();
                _response.Result = _mapper.Map<IEnumerable<ApplicationUserDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpGet("GetChatUsers/{userId}")]
        public async Task<ResponseDto> GetChatUsers(string userId)
        {
            try
            {
                IEnumerable<ApplicationUser> objList = await _authService.GetUsers();

                var messages = await _messageService.GetMessages();

                var chatMessages = messages.Where(u => u.FromUserId == userId || u.ToUserId == userId).ToList();

                foreach (var user in objList)
                {
                    if(chatMessages.Count > 0 && user.Id != userId)
                    {
                        var messagesBetweenUsers = messages
                            .Where(u => u.FromUserId == userId && u.ToUserId == user.Id ||
                             u.FromUserId == user.Id && u.ToUserId == userId).ToList();

                        if(messagesBetweenUsers.Count > 0)
                        {
                            user.LatestMessage = messagesBetweenUsers[messagesBetweenUsers.Count - 1];
                        }
                    }
                }

                _response.Result = _mapper.Map<IEnumerable<ApplicationUserDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("register")]
        public async Task<ResponseDto> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var errorMessage = await _authService.RegisterAsync(registerRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
            }
            return _response;
        }

        [HttpPost("login")]
        public async Task<ResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.LoginAsync(loginRequestDto);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
            }
            _response.Result = loginResponse;
            return _response;
        }

        [HttpPost("AssignRole")]
        public async Task<ResponseDto> AssignRole(RegisterRequestDto registerRequestDto)
        {
            var assignRoleSuccessfull = await _authService
                .AssignRoleAsync(registerRequestDto.Email, registerRequestDto.Role.ToUpper());
            if (!assignRoleSuccessfull)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
            }
            _response.Result = assignRoleSuccessfull;
            return _response;
        }

        [HttpGet("GeyUserById/{userId}")]
        public ResponseDto GeyUserById(string userId)
        {
            var user = _authService.GetUserById(userId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User was not found";
            }
            _response.Result = user;
            return _response;
        }

        [HttpGet("GeyUserByEmail")]
        [Authorize]
        public ResponseDto GeyUserByEmail(string email)
        {
            var user = _authService.GetUserByEmail(email);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User was not found";
            }
            _response.Result = user;
            return _response;
        }

        [HttpGet("GeyUserByPhoneNumber")]
        [Authorize]
        public ResponseDto GeyUserByPhoneNumber(string phoneNumber)
        {
            var user = _authService.GetUserByPhoneNumber(phoneNumber);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User was not found";
            }
            _response.Result = user;
            return _response;
        }
    }
}
