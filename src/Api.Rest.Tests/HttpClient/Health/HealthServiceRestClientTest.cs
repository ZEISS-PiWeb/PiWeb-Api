#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2025                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.HttpClient.Health
{
	#region usings

	using System;
	using System.Net;
	using System.Threading.Tasks;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Health;

	#endregion

	[TestFixture]
	public class HealthServiceRestClientTest
	{
		#region constants

		private const int Port = 8081;

		#endregion

		#region members

		private static readonly Uri Uri = new ( $"http://localhost:{Port}/" );

		#endregion

		#region methods

		[Test]
		public async Task GetReadiness_ApiResponseIs200Healthy_ReturnsHealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "Healthy" );

			var result = await client.GetReadiness();

			result.Should().Be( HealthCheckResultType.Healthy );
		}

		[Test]
		public async Task GetReadiness_ApiResponseIs200Degraded_ReturnsDegraded()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "Degraded" );

			var result = await client.GetReadiness();

			result.Should().Be( HealthCheckResultType.Degraded );
		}

		[Test]
		public async Task GetReadiness_ApiResponseIs503_ReturnsUnhealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "Unhealthy", HttpStatusCode.ServiceUnavailable );

			var result = await client.GetReadiness();

			result.Should().Be( HealthCheckResultType.Unhealthy );
		}

		[Test]
		public async Task GetReadiness_ApiResponseIs500_ThrowsException()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "", HttpStatusCode.InternalServerError );

			await client.Awaiting( svc => svc.GetReadiness() )
				.Should()
				.ThrowAsync<WrappedServerErrorException>();
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs200Healthy_ReturnsHealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "Healthy" );

			var result = await client.GetLiveness();

			result.Should().Be( HealthCheckResultType.Healthy );
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs200Degraded_ReturnsDegraded()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "Degraded" );

			var result = await client.GetLiveness();

			result.Should().Be( HealthCheckResultType.Degraded );
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs503_ReturnsUnhealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "Unhealthy", HttpStatusCode.ServiceUnavailable );

			var result = await client.GetLiveness();

			result.Should().Be( HealthCheckResultType.Unhealthy );
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs500_ThrowsException()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterDefaultResponse( "", HttpStatusCode.InternalServerError );

			await client.Awaiting( svc => svc.GetLiveness() )
				.Should()
				.ThrowAsync<WrappedServerErrorException>();
		}

		#endregion
	}
}