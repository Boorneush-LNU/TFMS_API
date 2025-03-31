using TransportFleetManagementSystem.Data;
using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public class UserRepository : IUserRepository
        {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
            {
            _context = context;
            }

        public User GetUserByUsername(string username)
            {
            return _context.Users.FirstOrDefault(u => u.Username == username);
            }

        public User GetUserByEmail(string email)
            {
            return _context.Users.FirstOrDefault(u => u.Email == email);
            }

        public void AddUser(User user)
            {
            _context.Users.Add(user);
            _context.SaveChanges();
            }

        public IEnumerable<User> GetAllUsers()
            {
            return _context.Users.ToList();
            }

        public User GetUserById(int id)
            {
            return _context.Users.Find(id);
            }

        public void DeleteUser(int id)
            {
            var user = _context.Users.Find(id);
            if (user != null)
                {
                _context.Users.Remove(user);
                _context.SaveChanges();
                }
            }

        public void UpdateUser(User user)
            {
            _context.Users.Update(user);
            _context.SaveChanges();
            }

        public bool UsernameExists(string username)
            {
            return _context.Users.Any(u => u.Username == username);
            }

        public bool EmailExists(string email)
            {
            return _context.Users.Any(u => u.Email == email);
            }
        }
    }
