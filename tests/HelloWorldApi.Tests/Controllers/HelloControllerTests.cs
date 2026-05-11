// HelloController.cs
[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Salom Dunyo");
    }
}
```

## Testlar

```csharp
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class HelloControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HelloControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // 1. Status 200 qaytaradimi
    [Fact]
    public async Task Get_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/hello");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 2. Javob matni to'g'rimi
    [Fact]
    public async Task Get_ReturnsCorrectBody()
    {
        var response = await _client.GetAsync("/api/hello");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("\"Salom Dunyo\"", content); // JSON string
    }

    // 3. Content-Type tekshiruvi
    [Fact]
    public async Task Get_ReturnsJsonContentType()
    {
        var response = await _client.GetAsync("/api/hello");

        Assert.Equal("application/json", 
            response.Content.Headers.ContentType?.MediaType);
    }

    // 4. POST — ruxsat berilmasligi kerak
    [Fact]
    public async Task Post_ReturnsMethodNotAllowed()
    {
        var response = await _client.PostAsync("/api/hello", null);

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    // 5. Noto'g'ri URL — 404
    [Fact]
    public async Task Get_WrongRoute_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/hello/extra");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // 6. Javob bo'sh emasligini tekshirish
    [Fact]
    public async Task Get_ResponseBodyIsNotEmpty()
    {
        var response = await _client.GetAsync("/api/hello");
        var content = await response.Content.ReadAsStringAsync();

        Assert.False(string.IsNullOrWhiteSpace(content));
    }

    // 7. Bir necha parallel so'rov — barqarorlik
    [Fact]
    public async Task Get_MultipleRequests_AllReturnOk()
    {
        var tasks = new Task<HttpResponseMessage>[10];
        for (int i = 0; i < 10; i++)
            tasks[i] = _client.GetAsync("/api/hello");

        var results = await Task.WhenAll(tasks);

        Assert.All(results, r => Assert.Equal(HttpStatusCode.OK, r.StatusCode));
    }
}
```

**Paketlar (`*.csproj`):**

```xml
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.0" />
<PackageReference Include="xunit" Version="2.9.0" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />