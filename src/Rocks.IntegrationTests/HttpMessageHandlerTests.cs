using NUnit.Framework;
using System.Net;

namespace Rocks.IntegrationTests;

public static class HttpMessageHandlerTests
{
	[Test]
	public static async Task CreateAsync()
	{
		using var response = new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent("OK")
		};
		var expectations = new HttpMessageHandlerCreateExpectations();
		expectations.Methods.SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
			.ReturnValue(Task.FromResult(response));

		using var mock = expectations.Instance();
		using var client = new HttpClient(mock);
		var getResponse = await client.GetAsync(new Uri("https://localhost.com")).ConfigureAwait(false);

		Assert.Multiple(() =>
		{
			Assert.That(getResponse.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(getResponse.Content, Is.EqualTo(response.Content));
		});

		expectations.Verify();
	}
}