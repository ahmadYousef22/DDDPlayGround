﻿
namespace DDDPlayGround.Application.Authentication.JwtToken
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
