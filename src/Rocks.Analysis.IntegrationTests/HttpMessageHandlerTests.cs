using NUnit.Framework;
using System.Net;

namespace Rocks.Analysis.IntegrationTests;

internal static class HttpMessageHandlerTests
{
	[Test]
	public static async Task CreateAsync()
	{
		using var response = new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent("OK")
		};
		using var context = new RockContext();
		var expectations = context.Create<HttpMessageHandlerCreateExpectations>();
#pragma warning disable CA2025 // Do not pass 'IDisposable' instances into unawaited tasks
		expectations.Setups.SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
			.ReturnValue(Task.FromResult(response));
#pragma warning restore CA2025 // Do not pass 'IDisposable' instances into unawaited tasks

		using var mock = expectations.Instance();
		using var client = new HttpClient(mock);
		var getResponse = await client.GetAsync(new Uri("https://localhost.com")).ConfigureAwait(false);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(getResponse.StatusCode, Is.EqualTo(response.StatusCode));
			Assert.That(getResponse.Content, Is.EqualTo(response.Content));
		}
	}
}