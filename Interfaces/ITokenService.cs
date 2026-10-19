using ClinicApiLearn.Models;
using System.Security.Cryptography;

namespace ClinicApiLearn.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(User user);
    string CreateRefreshToken();
    string HashToken(string token);
}