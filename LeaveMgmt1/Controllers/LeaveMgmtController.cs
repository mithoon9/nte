using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LeaveMgmt1.Interface;
using System.Threading.Tasks;
using LeaveManagement.Models;
using System.Collections.Generic;
using LeaveMgmt1.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LeaveMgmt1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveMgmtController : ControllerBase
    {
        private readonly ILogger<LeaveMgmtController> _logger;
        private readonly ITbl_user_repository _tbl_user_repository;
        private readonly ITbl_leave_type_repository _tbl_leave_type_repository;
        private readonly ITbl_leave_repository _tbl_leave_repository;
        private readonly IConfiguration _configuration;

        public LeaveMgmtController(ILogger<LeaveMgmtController> logger, ITbl_user_repository tbl_User_Repository,
            ITbl_leave_type_repository tbl_Leave_Type_Repository, ITbl_leave_repository tbl_Leave_Repository,
            IConfiguration configuration)
        {
            _logger = logger;
            _tbl_user_repository = tbl_User_Repository;
            _tbl_leave_type_repository = tbl_Leave_Type_Repository;
            _tbl_leave_repository = tbl_Leave_Repository;
            _configuration = configuration; 
            
        }

        [HttpGet("countleavetype")]
        [Authorize]
        public IActionResult Get()
        {
            var count = _tbl_leave_type_repository.CountOfLeave();
            if(count == 0)
                return BadRequest("no data");
            return Ok(count);
        }

        [HttpPost("registeruser")]
        public IActionResult Register(Tbl_user user)
        {
            if (ModelState.IsValid)
            {
                if (_tbl_user_repository.IsExisting(user.email))
                {
                    return BadRequest("email in use");
                }
                Tbl_user tbl_user = new Tbl_user()
                {
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    password = user.password,
                    contact = user.contact,
                    created_date = System.DateTime.Today.Date,
                    updated_date = System.DateTime.Today.Date,
                };
                if (_tbl_user_repository.register(tbl_user))
                {
                    return Ok("added user");
                }
                return BadRequest("Not Successful");
            }
            return BadRequest("invalid request");
            
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = _tbl_user_repository.GetUser(user);
                if (loggedInUser == null)
                    return NotFound("User not found");

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, loggedInUser.email),
                    new Claim(ClaimTypes.GivenName, loggedInUser.first_name),
                    new Claim(ClaimTypes.Surname, loggedInUser.last_name),

                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]));
                var token = new JwtSecurityToken
                    (
                        
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                            
                    ) ; 
                var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenStr);

            }
            return BadRequest("invalid request");
        }

        [HttpGet("countleavetypewithuser")]
        [Authorize]
        public IActionResult Get(int user_id)
        {
            int count = _tbl_leave_type_repository.CountOfLeaveWithUser(user_id);
            if (count == 0)
                return BadRequest("no data");
            return Ok(count);
        }

        [HttpPost("addleavetype")]
        [Authorize]
        public IActionResult Post(Tbl_leavetype tbl_type)
        {
            if (ModelState.IsValid)
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (_tbl_leave_type_repository.FindLeaveTypeByName(tbl_type.name) != null)
                    return BadRequest("Tbl leave type name exists");
                Tbl_leavetype tbl_Leavetype = new Tbl_leavetype()
                {
                    user_id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    name = tbl_type.name,
                    created_date = DateTime.UtcNow,
                    updated_date = DateTime.UtcNow,
                };
                _tbl_leave_type_repository.AddLeaveType(tbl_Leavetype);

                return Ok("Added Successfully");
            }
            return BadRequest("invalid request");
        }

        [HttpGet("getleavelist")]
        [Authorize]
        
        public IActionResult GetAllLeave()
        {
            IEnumerable<Tbl_leave> tbl_Leave = _tbl_leave_repository.GetAllLeave();
            return Ok(tbl_Leave);
        }

        [HttpGet("countofleave")]
        [Authorize]
        public IActionResult GetCountOfLeave(int user_id)
        {
            int count = _tbl_leave_repository.CountOfLeveWithUser(user_id);
            if (count == 0)
                return BadRequest("No record found");
            return Ok(count);
        }

        [HttpPost("addtbl_leave")]
        [Authorize]
        public IActionResult AddLeave(Tbl_leave tbl_Leave, string tbl_Leavetype_name)
        {
            if (ModelState.IsValid)
            {
                Tbl_leave leave = new Tbl_leave()
                {
                    user_id = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    leave_title = tbl_Leave.leave_title,
                    leave_description = tbl_Leave.leave_description,
                    leave_type = _tbl_leave_type_repository.FindLeaveTypeByName(tbl_Leavetype_name).Id,
                    from_date = tbl_Leave.from_date,
                    to_date = tbl_Leave.to_date,
                    created_date = DateTime.Now,
                    updated_date = DateTime.Now,
                };
                if(leave.leave_type==null)
                    return BadRequest("Tbl_leave_type name doesnt exist");
                _tbl_leave_repository.AddLeave(leave);
                return Ok(leave);
            }
            return BadRequest("Invalid request");
        }
    }
}
