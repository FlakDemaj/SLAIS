using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Xunit;

namespace Presentation.Tests.Common;

public abstract class TestBase : IClassFixture<TestWebApplicationFactory>
{
    protected readonly HttpClient _client;
    protected readonly TestWebApplicationFactory _factory;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected TestBase(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    protected static StringContent BuildJsonContent(object payload)
    {
        var json = JsonSerializer.Serialize(payload);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected static async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }

    protected void SetBearerToken(string token)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}
