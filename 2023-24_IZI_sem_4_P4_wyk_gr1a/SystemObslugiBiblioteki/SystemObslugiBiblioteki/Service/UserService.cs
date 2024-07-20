using System.Linq;
using System.Threading.Tasks;
using SystemObslugiBiblioteki.Model;
using SystemObslugiBiblioteki.Model.Dto;
using SystemObslugiBiblioteki.Persistence;

namespace SystemObslugiBiblioteki.Service
{
    public class UserService
    {
        private readonly IAppDbContext _appDbContext;

        public UserService(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User? GetByName(string name)
        {
            return _appDbContext.Users.FirstOrDefault(x => x.Name == name);
        }

        public async Task<UserDto> CreateUser(string name, string passHash, bool isAdmin)
        {
            var newUser = new User()
            {
                Name = name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(passHash),
                IsAdmin = isAdmin
            };

            var result = _appDbContext.Users.Add(newUser);
            await _appDbContext.SaveChangesAsync();

            return new UserDto
            {
                Id = result.Id,
                Name = result.Name,
                IsAdmin = result.IsAdmin
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var userToRemove = _appDbContext.Users.First(u => u.Id == id);
            var res = _appDbContext.Users.Remove(userToRemove);
            await _appDbContext.SaveChangesAsync();
            return res != null;
        }
    }
}
