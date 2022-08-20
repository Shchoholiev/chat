using Chat.Application.Descriptions;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Interfaces.Services.Identity;
using Chat.Application.Models.Dtos;
using Chat.Core.Entities.Identity;

namespace Chat.Infrastructure.Services.Identity
{
    public class UsersService : IUserManager
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        public UsersService(IGenericRepository<User> userRepository, IPasswordHasher passwordHasher)
        {
            _usersRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationDetails> RegisterAsync(UserDto userDTO)
        {
            var operationDetails = new OperationDetails();
            if (await _usersRepository.GetOneAsync(u => u.Email == userDTO.Email) != null)
            {
                operationDetails.AddError("This email is already used!");
                return operationDetails;
            }

            var user = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = userDTO.Name,
                Email = userDTO.Email,
            };

            try
            {
                user.PasswordHash = _passwordHasher.Hash(userDTO.Password);
                await _usersRepository.AddAsync(user);
            }
            catch (Exception e)
            {
                operationDetails.AddError(e.Message);
            }

            return operationDetails;
        }

        public async Task<OperationDetails> LoginAsync(UserDto userDTO)
        {
            var user = await _usersRepository.GetOneAsync(u => u.Email == userDTO.Email);

            var operationDetails = new OperationDetails();
            if (user == null)
            {
                operationDetails.AddError("User with this email not found!");
                return operationDetails;
            }

            if (!_passwordHasher.Check(userDTO.Password, user.PasswordHash))
            {
                operationDetails.AddError("Incorrect password!");
            }

            return operationDetails;
        }

        public async Task<OperationDetails> UpdateUserAsync(User user)
        {
            _usersRepository.Attach(user);
            await _usersRepository.UpdateAsync(user);
            return new OperationDetails();
        }

        public async Task DeleteAsync(string email)
        {
            var user = await _usersRepository.GetOneAsync(u => u.Email == email);
            await _usersRepository.DeleteAsync(user);
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await _usersRepository.GetOneAsync(u => u.Email == email, u => u.UserToken,
                                                           u => u.Connections, u => u.Rooms);
        }

        public async Task SaveAsync()
        {
            await _usersRepository.SaveAsync();
        }
    }
}
