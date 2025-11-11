using Microsoft.AspNetCore.Mvc;
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
        var existingUser = await _userRepository.GetByIdAsync(user.Id);

        if (existingUser != null)
        {
            // Update the existing user
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            await _userRepository.UpdateAsync(existingUser);
            return Ok(existingUser); // Return updated data
        }
        // Create a new user
        var created = await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            await _userRepository.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}