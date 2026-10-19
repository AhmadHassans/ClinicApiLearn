using ClinicApiLearn.Models;

namespace ClinicApiLearn.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(User user);
}