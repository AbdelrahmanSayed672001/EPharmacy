using E_Pharmacy.Helpers;
using E_Pharmacy.Models;
using E_Pharmacy.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Pharmacy.Sefvices.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly JWT jwt;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.jwt = jwt.Value;
            this.roleManager = roleManager;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = $"{model.Email} is already existed" };
            
            if (await userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = $"{model.Username} is already existed" };

            var user = new ApplicationUser
            {
                UserName=model.Username,
                Email=model.Email,
                PhoneNumber=model.Phone,
                Address=model.Address,
            };

            var result= await userManager.CreateAsync(user,model.Password);
            if(! result.Succeeded)
            {
                var errors = string.Empty;
                foreach(var e in result.Errors)
                {
                    errors += $"{ e.Description} , ";
                }
                return new AuthModel { Message = errors };
            }

            await userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJWTToken(user);

            return new AuthModel {
                Email = user.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Role = new List<string> { "User" },
                Username = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var authModel = new AuthModel();
            var user = await userManager.FindByEmailAsync(model.Email);
            
            if(user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect";
                return authModel;
            }
            
            var jwtSecurityToken = await CreateJWTToken(user);
            var roles=await userManager.GetRolesAsync(user);

            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.Email = user.Email;
            authModel.IsAuthenticated = true;
            authModel.Token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Role = roles.ToList();
            authModel.Username = user.UserName;



            return authModel;
        }
        
        public async Task<string> AddRoleAsync(AddToRole model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user is null || ! await roleManager.RoleExistsAsync(model.Role))
                return "Invalid Username or Role";


            if (await userManager.IsInRoleAsync(user, model.Role))
                return $"{user.UserName} is already assigned to {model.Role}";

            var result = await userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? String.Empty : "Something went wrong";
        }

        private async Task<JwtSecurityToken> CreateJWTToken(ApplicationUser user)
        {
            var userclaims=await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleclaims = new List<Claim>();

            foreach(var role in roles)
            {
                roleclaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            }.Union(userclaims).Union(roleclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                    issuer:jwt.Issuer,
                    audience:jwt.Audience,
                    claims:claim,
                    expires:DateTime.Now.AddDays(jwt.DurationInDays),
                    signingCredentials:signInCredentials
                );

            return jwtSecurityToken;
        }

       
    }
}
