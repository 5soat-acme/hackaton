using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HT.WebApi.Commons.Users;

public interface IUserApp
{
    string Name { get; }
    Guid? GetUserId();
    string GetUserEmail();
    string GetUserToken();
    string GetUserRefreshToken();
    bool IsAuthenticated();
    bool IsInRole(string role);
    IEnumerable<Claim> GetClaims();
    HttpContext GetHttpContext();
    Guid GetSessionId();
    string GetUserCpf();
}