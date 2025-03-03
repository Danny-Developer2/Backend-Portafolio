using API.DTO;
using API.Entities;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {

        Task<List<AppUser>> GetUsers();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> AddUser(UserDTO userDTO);
        Task<AppUser> UpdateUser(int id, UserDTO userDTO);
        Task<bool> DeleteUser(int id);
    }
}
