using Auth.Server.Data;
using Auth.Server.Models;
using Auth.Server.Models.Dto;
using Auth.Server.Service.IService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Server.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private IMapper _mapper;

        public AuthService(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }
        public async Task<bool> AssignRoleAsync(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }

        public ApplicationUserDto? GetUserByEmail(string email)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                return _mapper.Map<ApplicationUserDto>(user);
            }

            return null;
        }

        public ApplicationUserDto? GetUserById(string Id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                return _mapper.Map<ApplicationUserDto>(user);
            }

            return null;
        }

        public ApplicationUserDto? GetUserByPhoneNumber(string phoneNumber)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user != null)
            {
                return _mapper.Map<ApplicationUserDto>(user);
            }

            return null;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var users = await _db.ApplicationUsers.ToListAsync();
            return users;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.Email!.ToLower() == loginRequestDto.UserName.ToLower() ||
                u.UserName!.ToLower() == loginRequestDto.UserName.ToLower() ||
                u.PhoneNumber!.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found, Generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateTokenAsync(user, roles);

            ApplicationUserDto applicationUserDto = new()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = applicationUserDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                FullName = registerRequestDto.FullName,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registerRequestDto.Email);

                    ApplicationUserDto applicationUserDto = new()
                    {
                        Id = userToReturn.Id,
                        FullName = userToReturn.FullName,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                } 
                else
                {
                    return result.Errors.FirstOrDefault()!.Description;
                }
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
