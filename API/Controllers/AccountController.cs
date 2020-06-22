using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
         SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {

            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);

            return new UserDTO
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {

            return await _userManager.FindByEmailAsync(email) != null;
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {

            var user = await _userManager.FindByEmailAddressAsync(HttpContext?.User);

            return _mapper.Map<Address,AddressDTO>(user.Address);

        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO address){

           var user = await _userManager.FindByEmailAddressAsync(HttpContext?.User);

           user.Address = _mapper.Map<AddressDTO,Address>(address);

           var result = await _userManager.UpdateAsync(user);

           if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDTO>(user.Address));

           return BadRequest("Problem in updating user address");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDTO
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };

        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if(CheckEmailExists(registerDTO.Email).Result.Value){
                return BadRequest("Email alredy exists.");
            }

            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest();

            return new UserDTO
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };

        }

        [HttpGet("testAuth")]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "My secret key";
        }


    }
}