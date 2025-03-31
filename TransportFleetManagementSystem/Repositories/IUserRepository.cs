using TransportFleetManagementSystem.Model;

namespace TransportFleetManagementSystem.Repositories
    {
    public interface IUserRepository
        {
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        void AddUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void DeleteUser(int id);
        void UpdateUser(User user);
        bool UsernameExists(string username);
        bool EmailExists(string email);
        }
    }
