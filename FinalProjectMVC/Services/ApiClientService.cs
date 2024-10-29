using System.Net.Http.Headers;

namespace FinalProjectMVC.Services
{
    public class ApiClientService
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly HttpClient _httpClient;

        public ApiClientService(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7255/api/") };
        }

        public HttpClient GetAuthenticatedClient()
        {
            var request = _httpContextAccesor.HttpContext.Request;

            if(request.Cookies.TryGetValue("JwtToken", out string token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return _httpClient;
        }
    }
}
