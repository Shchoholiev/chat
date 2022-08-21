using Chat.Application.Exceptions;
using Chat.Application.Interfaces.Repositories;
using Chat.Application.Interfaces.Services.Identity;
using Chat.Application.Models.Identity;
using Chat.Core.Entities.Identity;
using System.Security.Claims;

namespace Chat.Infrastructure.Services.Identity
{
    public class UserManager : IUserManager
    {
        private readonly IGenericRepository<User> _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly ITokensService _tokensService;

        public UserManager(IGenericRepository<User> userRepository, IPasswordHasher passwordHasher, 
            ITokensService tokensService)
        {
            _usersRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokensService = tokensService;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken)
        {
            if (await this._usersRepository.GetOneAsync(u => u.Email == register.Email, cancellationToken) != null)
            {
                throw new AlreadyExistsException("user email", register.Email);
            }

            var user = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = register.Name,
                Email = register.Email,
            };

            await this._usersRepository.AddAsync(user, cancellationToken);
            var tokens = this.GetUserTokens(user);

            return tokens;
        }

        public async Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetOneAsync(u => u.Email == login.Email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (!this._passwordHasher.Check(login.Password, user.PasswordHash))
            {
                throw new InvalidDataException("Invalid password!");
            }

            user.UserToken = this.GetRefreshToken();
            await this._usersRepository.UpdateAsync(user, cancellationToken);
            var tokens = this.GetUserTokens(user);

            return tokens;
        }

        private UserToken GetRefreshToken()
        {
            var refreshToken = this._tokensService.GenerateRefreshToken();
            var token = new UserToken
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
            };

            return token;
        }

        private TokensModel GetUserTokens(User user)
        {
            var claims = this.GetClaims(user);
            var accessToken = this._tokensService.GenerateAccessToken(claims);

            return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = user.UserToken.RefreshToken
            };
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            return claims;
        }
    }
}
