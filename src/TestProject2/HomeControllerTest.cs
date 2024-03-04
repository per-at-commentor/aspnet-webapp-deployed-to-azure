using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using WebApplication1.Controllers;

namespace TestProject2;

public class HomeControllerTest
{
    [Fact]
    public async Task HomePage()
    {
        var factory = new WebApplicationFactory<HomeController>();
        var client = factory.CreateDefaultClient();
        var result = await client.GetAsync("/Home");
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var body = await result.Content.ReadAsStringAsync();
        Assert.Contains("Hello world!", body);
    }

    [Fact]
    public async Task PrivacyPage()
    {
        var factory = new WebApplicationFactory<HomeController>();
        var client = factory.CreateDefaultClient();
        var result = await client.GetAsync("/Home/Privacy");
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var body = await result.Content.ReadAsStringAsync();
        Assert.Contains("Use this page to detail your site's privacy policy.", body);
    }

    [Fact]
    public async Task ErrorPage()
    {
        var factory = new WebApplicationFactory<HomeController>();
        var client = factory.CreateDefaultClient();
        var result = await client.GetAsync("/Home/Error");
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var body = await result.Content.ReadAsStringAsync();
        Assert.Contains("An error occurred while processing your request.", body);
    }

    [Fact]
    public async Task InternalServerError()
    {
        var factory = new WebApplicationFactory<HomeController>();
        var client = factory.CreateDefaultClient();
        var result = await client.GetAsync("/Home/DivideByZero");
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        var body = await result.Content.ReadAsStringAsync();
        Assert.Contains("DivideByZeroException", body);
    }
}