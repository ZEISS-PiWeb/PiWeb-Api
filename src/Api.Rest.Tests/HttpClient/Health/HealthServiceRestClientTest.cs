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
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Health;

	#endregion

	[TestFixture]
	public class HealthServiceRestClientTest
	{
		#region constants

		private const int Port = 8081;

		#endregion

		#region members

		private static readonly Uri Uri = new($"http://localhost:{Port}/");

		#endregion

		#region methods

		[Test]
		public async Task GetInterfaceInformation_HappyPath_ReturnsResult()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var interfaceVersionResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 2, 3, 4 )] };
			var responseJson = JsonConvert.SerializeObject( interfaceVersionResponse );
			server.RegisterDefaultResponse( responseJson );

			var result = await client.GetInterfaceInformation();

			result.SupportedVersions.Count().Should().Be( 1 );
			result.SupportedVersions.First().Major.Should().Be( 1 );
			result.SupportedVersions.First().Minor.Should().Be( 2 );
			result.SupportedVersions.First().Build.Should().Be( 3 );
			result.SupportedVersions.First().Revision.Should().Be( 4 );
		}

		[Test]
		public async Task GetReadiness_ApiResponseIs200Healthy_ReturnsHealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var healthCheckResponse = new HealthCheckStatus { Status = HealthCheckResultType.Healthy };
			var responseJson = JsonConvert.SerializeObject( healthCheckResponse );
			server.RegisterResponse( "/HealthServiceRest/ready", responseJson );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			var result = await client.GetReadiness();

			result.Status.Should().Be( HealthCheckResultType.Healthy );
		}

		[Test]
		public async Task GetReadiness_ApiResponseIs200Degraded_ReturnsDegraded()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var healthCheckResponse = new HealthCheckStatus { Status = HealthCheckResultType.Degraded };
			var responseJson = JsonConvert.SerializeObject( healthCheckResponse );
			server.RegisterResponse( "/HealthServiceRest/ready", responseJson );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			var result = await client.GetReadiness();

			result.Status.Should().Be( HealthCheckResultType.Degraded );
		}

		[Test]
		public async Task GetReadiness_ApiResponseIs503_ReturnsUnhealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var healthCheckResponse = new HealthCheckStatus { Status = HealthCheckResultType.Unhealthy };
			var responseJson = JsonConvert.SerializeObject( healthCheckResponse );
			server.RegisterResponse( "/HealthServiceRest/ready", responseJson, HttpStatusCode.ServiceUnavailable );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			var result = await client.GetReadiness();

			result.Status.Should().Be( HealthCheckResultType.Unhealthy );
		}

		[Test]
		public async Task GetReadiness_ApiResponseIs500_ThrowsException()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterResponse( "/HealthServiceRest/ready", "", HttpStatusCode.InternalServerError );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			await client.Awaiting( svc => svc.GetReadiness() )
				.Should()
				.ThrowAsync<WrappedServerErrorException>();
		}

		[Test]
		public async Task GetReadiness_ApiNotSupported_ThrowsException()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterResponse( "/HealthServiceRest", "{}", HttpStatusCode.NotFound );

			await client.Awaiting( svc => svc.GetReadiness() )
				.Should()
				.ThrowAsync<OperationNotSupportedOnServerException>();
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs200Healthy_ReturnsHealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var healthCheckResponse = new HealthCheckStatus { Status = HealthCheckResultType.Healthy };
			var responseJson = JsonConvert.SerializeObject( healthCheckResponse );
			server.RegisterResponse( "/HealthServiceRest/live", responseJson );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			var result = await client.GetLiveness();

			result.Status.Should().Be( HealthCheckResultType.Healthy );
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs200Degraded_ReturnsDegraded()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var healthCheckResponse = new HealthCheckStatus { Status = HealthCheckResultType.Degraded };
			var responseJson = JsonConvert.SerializeObject( healthCheckResponse );
			server.RegisterResponse( "/HealthServiceRest/live", responseJson );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			var result = await client.GetLiveness();

			result.Status.Should().Be( HealthCheckResultType.Degraded );
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs503_ReturnsUnhealthy()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var healthCheckResponse = new HealthCheckStatus { Status = HealthCheckResultType.Unhealthy };
			var responseJson = JsonConvert.SerializeObject( healthCheckResponse );
			server.RegisterResponse( "/HealthServiceRest/live", responseJson, HttpStatusCode.ServiceUnavailable );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			var result = await client.GetLiveness();

			result.Status.Should().Be( HealthCheckResultType.Unhealthy );
		}

		[Test]
		public async Task GetLiveness_ApiResponseIs500_ThrowsException()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterResponse( "/HealthServiceRest/live", "", HttpStatusCode.InternalServerError );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 0, 0 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			await client.Awaiting( svc => svc.GetLiveness() )
				.Should()
				.ThrowAsync<WrappedServerErrorException>();
		}

		[Test]
		public async Task GetLiveness_ApiNotSupported_ThrowsException()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterResponse( "/HealthServiceRest", "{}", HttpStatusCode.NotFound );

			await client.Awaiting( svc => svc.GetLiveness() )
				.Should()
				.ThrowAsync<OperationNotSupportedOnServerException>();
		}

		[Test]
		public async Task GetInterfaceInformation_ApiResponseIs200_ReturnsInterfaceVersion()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			var interfaceInformationResponse = new InterfaceVersionRange { SupportedVersions = [new Version( 1, 2, 3 )] };
			var interfaceResponseJson = JsonConvert.SerializeObject( interfaceInformationResponse );
			server.RegisterResponse( "/HealthServiceRest", interfaceResponseJson );

			var result = await client.GetInterfaceInformation();

			result.SupportedVersions.Should().HaveCount( 1 );
			result.SupportedVersions.First().Should().BeEquivalentTo( interfaceInformationResponse.SupportedVersions.First() );
		}

		[Test]
		public async Task GetInterfaceInformation_ApiResponseIs404_ThrowsException()
		{
			using var server = WebServer.StartNew( Port );
			using var client = new HealthServiceRestClient( Uri );
			server.RegisterResponse( "/HealthServiceRest", "", HttpStatusCode.NotFound );

			await client.Awaiting( svc => svc.GetInterfaceInformation() )
				.Should()
				.ThrowAsync<WrappedServerErrorException>();
		}

		#endregion
	}
}