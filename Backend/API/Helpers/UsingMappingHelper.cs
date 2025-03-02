using API.Entities;
using API.DTO;

namespace API.Helpers
{
    public static class UserMappingHelper
    {
        public static UserDTO MapToUserDTO(AppUser user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
