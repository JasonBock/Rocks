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
		var handlerMock = Rock.Create<HttpMessageHandler>();
		handlerMock.Methods().SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
			.Returns(Task.FromResult(response));

		using var client = new HttpClient(handlerMock.Instance());
		await client.GetAsync(new Uri("https://localhost.com")).ConfigureAwait(false);

		handlerMock.Verify();
	}
}