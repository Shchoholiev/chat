using Chat.Application.Descriptions;
using Chat.Application.DTO;
using Chat.Application.Interfaces;
using Chat.Application.IRepositories;
using Chat.Core.Entities.Identity;

namespace Chat.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        public UsersService(IGenericRepository<User> userRepository, IPasswordHasher passwordHasher)
        {
            this._usersRepository = userRepository;
            this._passwordHasher = passwordHasher;
        }

        public async Task<OperationDetails> RegisterAsync(UserDTO userDTO)
        {
            var operationDetails = new OperationDetails();
            if (await this._usersRepository.GetOneAsync(u => u.Email == userDTO.Email) != null)
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
                user.PasswordHash = this._passwordHasher.Hash(userDTO.Password);
                await this._usersRepository.AddAsync(user);
            }
            catch (Exception e)
            {
                operationDetails.AddError(e.Message);
            }

            return operationDetails;
        }

        public async Task<OperationDetails> LoginAsync(UserDTO userDTO)
        {
            var user = await this._usersRepository.GetOneAsync(u => u.Email == userDTO.Email);

            var operationDetails = new OperationDetails();
            if (user == null)
            {
                operationDetails.AddError("User with this email not found!");
                return operationDetails;
            }

            if (!this._passwordHasher.Check(userDTO.Password, user.PasswordHash))
            {
                operationDetails.AddError("Incorrect password!");
            }

            return operationDetails;
        }

        public async Task<OperationDetails> UpdateUserAsync(User user)
        {
            this._usersRepository.Attach(user);
            await this._usersRepository.UpdateAsync(user);
            return new OperationDetails();
        }

        public async Task DeleteAsync(string email)
        {
            var user = await this._usersRepository.GetOneAsync(u => u.Email == email);
            await this._usersRepository.DeleteAsync(user);
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await this._usersRepository.GetOneAsync(u => u.Email == email, u => u.UserToken, 
                                                           u => u.Connections, u => u.Rooms);
        }
    }
}
