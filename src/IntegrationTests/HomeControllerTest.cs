using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
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
    public async Task Divide10By2()
    {
        var factory = new WebApplicationFactory<HomeController>();
        var client = factory.CreateDefaultClient();
        var result = await client.GetAsync("/Home/Divide/10/By/2");
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var body = await result.Content.ReadAsStringAsync();
        Assert.Contains("5", body);
    }

    [Fact]
    public async Task DevelopmentEnvironmentShouldExposeErrorDetailsOnException()
    {
        var factory = new WebApplicationFactory<HomeController>().WithWebHostBuilder(b => b.UseEnvironment(Environments.Development));
        var client = factory.CreateDefaultClient();
        var result = await client.GetAsync("/Home/Divide/10/By/0");
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        var body = await result.Content.ReadAsStringAsync();
        Assert.Contains("DivideByZeroException", body);
        Assert.Contains("at WebApplication1.DummyCalculator.Div", body);
    }

    [Fact]
    public async Task ProductionEnvironmentShouldUseGenericErrorPageOnException()
    {
        var factory = new WebApplicationFactory<HomeController>().WithWebHostBuilder(b => b.UseEnvironment(Environments.Production));
        var client = factory.CreateDefaultClient();
        var result = await client.GetAsync("/Home/Divide/10/By/0");
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        var body = await result.Content.ReadAsStringAsync();
        Assert.Contains("An error occurred while processing your request.", body);
        Assert.DoesNotContain("DivideByZeroException", body);
        Assert.DoesNotContain("at WebApplication1.DummyCalculator.Div", body);
    }
}