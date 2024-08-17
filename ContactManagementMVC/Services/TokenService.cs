//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using ContactManagementMVC.Models;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace ContactManagementMVC.Services
//{
//    public class TokenService
//    {
//        private readonly IConfiguration _configuration;

//        public TokenService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public async Task<string> GenerateTokenAsync(ApplicationUser user)
//        {
//            var jwtSettings = _configuration.GetSection("JwtSettings");

//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(ClaimTypes.NameIdentifier, user.Id)
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryInMinutes"])),
//                signingCredentials: creds);

//            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
//        }
//    }
//}
