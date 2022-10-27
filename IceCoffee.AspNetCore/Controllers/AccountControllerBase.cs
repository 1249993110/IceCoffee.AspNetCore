using IceCoffee.AspNetCore.Models;
using IceCoffee.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IceCoffee.AspNetCore.Controllers
{
    public class AccountControllerBase : ApiControllerBase
    {
        /// <summary>
        /// 通过 Cookie 登录
        /// </summary>
        protected virtual async Task SignInWithCookie(UserInfo userInfo, bool rememberMe = true)
        {
            var utcNow = DateTimeOffset.UtcNow;

            var properties = new AuthenticationProperties()
            {
                IssuedUtc = utcNow,
                AllowRefresh = true,
                IsPersistent = rememberMe,
                ExpiresUtc = utcNow.AddDays(7),
            };

            await SignInWithCookie(userInfo.ToClaims(), properties);
        }

        /// <summary>
        /// 通过 Cookie 登录, 默认开启滑动过期, 窗口期7天
        /// </summary>
        protected virtual async Task SignInWithCookie(IEnumerable<Claim> claims, AuthenticationProperties authenticationProperties)
        {
            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                RegisteredClaimNames.UserName,
                RegisteredClaimNames.RoleNames);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
        }

        /// <summary>
        /// 通过 Cookie 注销
        /// </summary>
        protected virtual async Task SignOutWithCookie()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// 通过 JWT 登录
        /// </summary>
        /// <returns></returns>
        protected virtual JwtToken SignInWithJwt(UserInfo userInfo)
        {
            return GenerateJwtToken(userInfo.ToClaims());
        }

        /// <summary>
        /// 通过 JWT 登录
        /// </summary>
        /// <returns></returns>
        protected virtual JwtToken SignInWithJwt(IEnumerable<Claim> claims)
        {
            return GenerateJwtToken(claims);
        }

        /// <summary>
        /// 通过 JWT 注销
        /// </summary>
        /// <returns>jwtId</returns>
        protected virtual string SignOutWithJwt()
        {
            return User.Claims.First(p => p.Type == JwtRegisteredClaimNames.Jti).Value;
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="accessToken">已过期的访问令牌</param>
        /// <param name="storedRefreshToken">存储的刷新令牌</param>
        /// <remarks>
        /// 令牌验证失败将抛出相应异常
        /// </remarks>
        /// <returns></returns>
        protected virtual JwtToken RefreshToken(string accessToken, StoredRefreshToken? storedRefreshToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken? validatedToken = null;

            var tokenValidationParams = HttpContext.RequestServices.GetRequiredService<TokenValidationParameters>();

            try
            {
                // Validation 1 - Validation JWT token format
                // 此验证功能将确保 Token 满足验证参数, 并且它是一个真正的 token 而不仅仅是随机字符串

                var tokenInVerification = jwtTokenHandler.ValidateToken(accessToken, tokenValidationParams, out validatedToken);

                throw new CustomExceptionBase("令牌尚未过期");
            }
            catch (SecurityTokenExpiredException)
            {
                if (validatedToken is not JwtSecurityToken jwtSecurityToken)
                {
                    throw new Exception("无效的令牌");
                }
                else
                {
                    // Validation 2 - Validate encryption alg
                    // 检查 token 是否有有效的安全算法
                    if (validatedToken == null
                        || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) == false)
                    {
                        throw new Exception("无效的令牌");
                    }

                    // validation 3 - validate existence of the token
                    // 验证 refresh token 是否存在, 是否是保存在数据库的 refresh token
                    if (storedRefreshToken == null)
                    {
                        throw new Exception("刷新令牌不存在");
                    }

                    // Validation 5 - 检查存储的 RefreshToken 是否已过期
                    // Check the date of the saved refresh token if it has expired
                    if (DateTime.Now > storedRefreshToken.ExpiryDate)
                    {
                        throw new Exception("刷新令牌已过期, 用户需要重新登录");
                    }

                    // Validation 6 - validate if revoked
                    // 检查 refresh token 是否被撤销
                    if (storedRefreshToken.IsRevorked)
                    {
                        throw new Exception("刷新令牌已被撤销");
                    }

                    // Validation 7 - validate the id
                    // 根据数据库中保存的 Id 验证收到的 token 的 Id
                    if (storedRefreshToken.JwtId != validatedToken.Id)
                    {
                        throw new Exception("令牌与保存的令牌不匹配");
                    }

                    // 生成一个新的 token
                    return GenerateJwtToken(jwtSecurityToken.Claims);
                }
            }
            catch (CustomExceptionBase)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("签名验证失败", ex);
            }
        }

        /// <summary>
        /// 生成 JwtToken, 覆盖此方法以更改到期时间跨度
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="accessTokenExpiryTimeSpan">默认 10 分钟</param>
        /// <param name="refreshTokenExpiryTimeSpan">默认 30 天</param>
        /// <returns></returns>
        protected virtual JwtToken GenerateJwtToken(
            IEnumerable<Claim> claims,
            TimeSpan? accessTokenExpiryTimeSpan = null,
            TimeSpan? refreshTokenExpiryTimeSpan = null)
        {
            if (claims is not List<Claim> _claims)
            {
                _claims = claims.ToList();
            }

            var tokenValidationParams = HttpContext.RequestServices.GetRequiredService<TokenValidationParameters>();

            string jwtId = Guid.NewGuid().ToString();

            _claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Aud, tokenValidationParams.ValidAudience),
                new Claim(JwtRegisteredClaimNames.Iss, tokenValidationParams.ValidIssuer),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId)
            });

            var now = DateTime.Now;
            var utcNow = DateTime.UtcNow;
            var accessTokenExpiryDate = utcNow.Add(accessTokenExpiryTimeSpan ?? TimeSpan.FromMinutes(10));
            var refreshTokenExpiryDate = now.Add(refreshTokenExpiryTimeSpan ?? TimeSpan.FromDays(30));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(_claims),
                // 比较合理的值为 5~10 分钟, 需要一个UTC时间
                Expires = accessTokenExpiryDate,
                SigningCredentials = new SigningCredentials(tokenValidationParams.IssuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var accessToken = jwtTokenHandler.WriteToken(token);

            // 生成长度为 64 的随机字符串
            string refreshToken = Guid.NewGuid().ToString("N") + Utils.GetRandomString(24);

            return new JwtToken()
            {
                Id = jwtId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiryDate = accessTokenExpiryDate.Add(TimeZoneInfo.Local.BaseUtcOffset),
                RefreshTokenExpiryDate = refreshTokenExpiryDate
            };
        }
    }
}