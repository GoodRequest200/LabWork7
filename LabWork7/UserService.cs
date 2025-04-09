using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork7
{
    public class UserService
    {
        private AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<List<User>> GetUsersAsync() => await _context.Users.AsNoTracking().ToListAsync();

        public async Task<User> GetUSerByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task<int> CreateUserAsync(string login, string password)
        {
            User user = new User { Id = RandomInteger.GenerateRandomPositiveInt(), Login = login, Password = password };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task ChangeUserAsync(int id, string login, string password)
        {
            User selectedUser = await _context.Users.FindAsync(id);

            selectedUser.Login = login;
            selectedUser.Password = password;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            User selectedUser = await _context.Users.FindAsync(id);

            _context.Users.Remove(selectedUser);

            await _context.SaveChangesAsync();
        }
    }
}
