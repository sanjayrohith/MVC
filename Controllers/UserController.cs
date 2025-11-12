using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserCrudRepo.Models;
using UserCrudRepo.Repository;

namespace UserCrudRepo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]  
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            try
            {
                var created = await _userRepository.AddAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Only Gmail"))
            {
                return BadRequest(new { message = "Only Gmail addresses are allowed." });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                return Conflict(new { message = "Email already exists." });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Email already exists." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            try
            {
                await _userRepository.UpdateAsync(user);
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Only Gmail"))
            {
                return BadRequest(new { message = "Only Gmail addresses are allowed." });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
            {
                return Conflict(new { message = "Email already exists." });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Email already exists." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}