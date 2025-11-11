using Microsoft.EntityFrameworkCore;
using UserCrudRepo.Data;
using UserCrudRepo.Models;

namespace UserCrudRepo.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

public async Task<User> AddAsync(User user)
    {
        // Check if user with same ID already exists
        var existingUser = await _context.Users.FindAsync(user.Id);

        if (existingUser != null)
        {
            // Update existing user fields
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        // If not found, create a new one
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }


        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
