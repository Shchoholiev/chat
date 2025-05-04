using Chat.Application.Exceptions;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Interfaces.Services;
using Chat.Application.Interfaces.Services.Identity;
using Chat.Application.Mapping;
using Chat.Application.Models.Dtos;
using Chat.Application.Models.Identity;
using Chat.Core.Entities.Identity;

namespace Chat.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IUserManager _userManager;

        private readonly Mapper _mapper = new();

        public AccountService(IGenericRepository<User> userRepository, IUserManager userManager)
        {
            this._usersRepository = userRepository;
            this._userManager = userManager;
        }

        public async Task<UserDto> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetOneAsync(u => u.Email == email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }
            var dto = this._mapper.Map(user);

            return dto;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken)
        {
            return await this._userManager.RegisterAsync(register, cancellationToken);
        }

        public async Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken)
        {
            return await this._userManager.LoginAsync(login, cancellationToken);
        }
    }
}
