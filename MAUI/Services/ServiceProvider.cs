using System.Text;
using ChatAppTutorial.Services.Authenticate;
using Newtonsoft.Json;

namespace ChatAppTutorial.Services;

public class ServiceProvider
{
    private static ServiceProvider _instance;
    private string _serverRootUrl = "http://192.168.2.219/chatAPI";
    public string _accessToken = "";

    private ServiceProvider()
    {
    }

    public static ServiceProvider GetInstance()
    {
        return _instance ??= new ServiceProvider();
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
    {
        using (HttpClient client = new HttpClient())
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_serverRootUrl + "/Service/Authenticate");

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await client.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;
                if (result.StatusCode == 200)
                {
                    _accessToken = result.Id.ToString();
                }

                return result;
            }
            catch (Exception e)
            {
                var result = Activator.CreateInstance<AuthenticateResponse>();
                result.StatusCode = 500;
                result.StatusMessage = e.Message;
                return result;
            }
        }
    }

    public async Task<TResponse> CallWebApi<TRequest, TResponse>(
        string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse : BaseResponse
    {
        using (HttpClient client = new HttpClient())
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_serverRootUrl + apiUrl);
            httpRequestMessage.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await client.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception e)
            {
                var result = Activator.CreateInstance<TResponse>();
                result.StatusCode = 500;
                result.StatusMessage = e.Message;
                return result;
            }
        }
    }
}