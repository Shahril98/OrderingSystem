using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Infrastructure;
using OrderingSystem.Infrastructure.Dtos;
using System.Security.Cryptography.X509Certificates;

namespace OrderingSystem.WebApi.Controllers
{
    [Route("api/usermanager")]
    [ApiController]
    public class UserManagerController(RoleManager<IdentityRole<Guid>> roleManager, UserManager<IdentityUser<Guid>> userManager) : ControllerBase
    {
        [HttpPost]
        [Route("role/add")]
        [ProducesResponseType(typeof(IdentityRole<Guid>), 200)]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var role = new IdentityRole<Guid> { Name = roleName };
            var result = await roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new NoDataUpdatedException(error.Description);
                }
            }
            return Ok(role);
        }
        [HttpGet]
        [Route("role/all")]
        [ProducesResponseType(typeof(List<IdentityRole<Guid>>), 200)]
        public  IActionResult GetAllRole()
        {
           return Ok(roleManager.Roles.ToList());
        }
        [HttpDelete]
        [Route("role/{roleId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
          var result =  await roleManager.DeleteAsync(new IdentityRole<Guid> { Id = roleId });
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new NoDataUpdatedException(error.Description);
                }
            }
            return NoContent();
        }
        [HttpPatch]
        [Route("role/assign/{userId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AssignRolesToUser(Guid userId, [FromBody] List<string> roleName)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if(user == null)
            {
                throw new RecordNotFoundException("user not exist");
            }
            var roles = await userManager.GetRolesAsync(user);
            if(roles != null &&  roles.Count > 0)
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, roles);
                if (!removeResult.Succeeded)
                {
                    throw new NoDataUpdatedException("cannot remove existing role");
                }
            }
            var assignResult = await userManager.AddToRolesAsync(user, roleName);
            if (!assignResult.Succeeded)
            {
                throw new NoDataUpdatedException("cannot add roles");
            }
            return NoContent();
        }
        [HttpPost]
        [Authorize]
        [Route("user/add")]
        [ProducesResponseType(typeof(UserCreatedDto), 200)]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto addUser)
        {
            var userExists = await userManager.FindByEmailAsync(addUser.Email);
            if (userExists != null)
                throw new RecordAlreadyExistException($"user with emai {addUser.Email} already exist.");
            var toSave = new IdentityUser<Guid>
            {
                Email = addUser.Email,
                UserName = addUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = addUser.Email.ToUpper()
            };
            var result = await userManager.CreateAsync(toSave, addUser.Password);
            if (!result.Succeeded)
                throw new CustomException("Failed to create user");

            var roles = await userManager.GetRolesAsync(toSave);
            if (roles != null && roles.Count > 0)
            {
                var removeResult = await userManager.RemoveFromRolesAsync(toSave, roles);
                if (!removeResult.Succeeded)
                {
                    throw new NoDataUpdatedException("cannot remove existing role");
                }
            }
            var assignResult = await userManager.AddToRolesAsync(toSave, addUser.Roles);
            if (!assignResult.Succeeded)
            {
                throw new NoDataUpdatedException("cannot add roles");
            }
            return Ok(new UserCreatedDto { Email = toSave.Email, Id = toSave.Id });
        }
        [HttpDelete]
        [Authorize]
        [Route("user/delete/{userId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new RecordNotFoundException("User not exist");
          var result =  await userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new CustomException("Failed to deleted user");
            return NoContent();
        }
    }
}
