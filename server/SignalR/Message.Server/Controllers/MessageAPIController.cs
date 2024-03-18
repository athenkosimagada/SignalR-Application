using AutoMapper;
using Message.Server.Data;
using Message.Server.Models;
using Message.Server.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Message.Server.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        public MessageAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpPost("GetMessagesBetweenTwoUsers")]
        [Authorize]
        public ResponseDto Get([FromBody] MessageRequest message)
        {
            try
            {
                IEnumerable<UserMessage> objList = _db.Messages
                    .Where(u => u.FromUserId == message.FromUserId && u.ToUserId == message.ToUserId).ToList();
                _response.Result = _mapper.Map<IEnumerable<UserMessageDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("AddMessage")]
        [Authorize]
        public ResponseDto Post([FromBody] UserMessageDto userMessageDto)
        {
            try
            {
                userMessageDto.SentOn = DateTime.Now;
                _db.Messages.Add(_mapper.Map<UserMessage>(userMessageDto));
                _db.SaveChanges();

                _response.Result = userMessageDto;
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
