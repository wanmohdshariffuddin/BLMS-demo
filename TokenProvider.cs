using BLMS.Context;
using BLMS.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLMS
{
    public class TokenProvider
    {
        readonly LoginDBContext loginContext = new LoginDBContext();

        public string LoginUser(string STAFF_EMAIL, string PASSWORD)
        {
            //Get user details
            User user = loginContext.GetUserByEmail(STAFF_EMAIL);

            //Authenticate User, Check if its a registered user in DB
            if (user == null)
                return null;

            if (PASSWORD == user.PASSWORD)
            {
                //Authentication successful, Issue Token with user credentials
                //Provide the security key which was given in the JWToken configuration in Startup.cs
                var key = Encoding.ASCII.GetBytes("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
                //Generate Token for user
                var JWToken = new JwtSecurityToken(
                    issuer: "http://localhost:45092/",
                    audience: "http://localhost:45092/",
                    claims: GetUserClaims(user),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                    //Using HS256 Algorithm to encrypt Token
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                return token;
            }
            else
            {
                return null;
            }
        }

        //Using hard coded collection list as Data Store for demo. In reality, User data comes from Database or some other Data Source - JRozario
        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;

            _claim = new Claim(ClaimTypes.Name, user.STAFF_NAME);
            claims.Add(_claim);

            _claim = new Claim("STAFF_EMAIL", user.STAFF_EMAIL);
            claims.Add(_claim);

            _claim = new Claim("STAFF_NO", user.STAFF_NO);
            claims.Add(_claim);

            _claim = new Claim(user.ROLE, user.ROLE);
            claims.Add(_claim);

            _claim = new Claim(user.ACCESS_LEVEL, user.ACCESS_LEVEL);
            claims.Add(_claim);

            if (user.WRITE_ACCESS != "NO_WRITE_ACCESS")
            {
                _claim = new Claim(user.WRITE_ACCESS, user.WRITE_ACCESS);
                claims.Add(_claim);
            }
            return claims.AsEnumerable<Claim>();
        }
    }
}
