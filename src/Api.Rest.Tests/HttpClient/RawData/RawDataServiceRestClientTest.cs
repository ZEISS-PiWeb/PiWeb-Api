#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.HttpClient.RawData
{
	#region usings

	using System;
	using System.Text;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.RawData;
	using Zeiss.PiWeb.Api.Rest.HttpClient.RawData;

	#endregion

	[TestFixture]
	public class RawDataServiceRestClientTest
	{
		#region constants

		private const int Port = 8081;
		private const int MaxUriLength = 80;

		#endregion

		#region members

		private static readonly Uri Uri = new Uri( $"http://localhost:{Port}/" );

		#endregion

		#region methods

		[Test]
		public async Task CreateRawData_SupportsResult_ReturnsResultWithKey()
		{
			var md5 = Guid.Parse( "FFC45AA8-7CFC-43D8-B159-D388B63AABD9" );
			var partUuid = Guid.Parse( "4F8C6FF6-9A94-4C54-A7E0-4B277758BAD3" );
			var data = Encoding.UTF8.GetBytes( "bar" );
			const int key = 42;

			var input = new RawDataInformationDto
			{
				Created = DateTime.Now,
				FileName = "foo.txt",
				Key = null,
				LastModified = DateTime.Now,
				MD5 = md5,
				MimeType = "text/plain",
				Size = data.Length,
				Target = new RawDataTargetEntityDto { Entity = RawDataEntityDto.Part, Uuid = partUuid.ToString() }
			};
			var expected = new RawDataInformationDto( input ) { Key = key };

			using var webServer = WebServer.StartNew( Port );
			RegisterRawDataServiceVersionResponse( webServer, "1.7.0" );
			RegisterCreateRawDataResponse( webServer, partUuid, expected );

			using var sut = new RawDataServiceRestClient( Uri, MaxUriLength );
			var actual = await sut.CreateRawData( input, data );

			Assert.That( actual.Key, Is.EqualTo( key ) );
		}

		[Test]
		public async Task CreateRawData_NotSupportsResult_ReturnsResultWithKey()
		{
			var md5 = Guid.Parse( "FFC45AA8-7CFC-43D8-B159-D388B63AABD9" );
			var partUuid = Guid.Parse( "4F8C6FF6-9A94-4C54-A7E0-4B277758BAD3" );
			var data = Encoding.UTF8.GetBytes( "bar" );
			const int key = 42;

			var input = new RawDataInformationDto
			{
				Created = DateTime.Now,
				FileName = "foo.txt",
				Key = null,
				LastModified = DateTime.Now,
				MD5 = md5,
				MimeType = "text/plain",
				Size = data.Length,
				Target = new RawDataTargetEntityDto { Entity = RawDataEntityDto.Part, Uuid = partUuid.ToString() }
			};
			var expected = new RawDataInformationDto( input ) { Key = key };

			using var webServer = WebServer.StartNew( Port );
			RegisterRawDataServiceVersionResponse( webServer, "1.6.0" );
			RegisterListRawDataResponse( webServer, partUuid, new[] { expected } );

			using var sut = new RawDataServiceRestClient( Uri, MaxUriLength );
			var actual = await sut.CreateRawData( input, data );

			Assert.That( actual.Key, Is.EqualTo( key ) );
		}

		private static void RegisterRawDataServiceVersionResponse( WebServer webServer, string version )
		{
			var interfaceVersionRange = new InterfaceVersionRange { SupportedVersions = new[] { new Version( version ) } };
			var responseJson = JsonConvert.SerializeObject( interfaceVersionRange, new VersionConverter() );
			webServer.RegisterResponse( "/RawDataServiceRest/", responseJson );
		}

		private static void RegisterCreateRawDataResponse( WebServer webServer, Guid partUuid, RawDataInformationDto responseObject )
		{
			var url = $"/RawDataServiceRest/rawData/Part/{partUuid}";
			var responseJson = JsonConvert.SerializeObject( responseObject );
			webServer.RegisterResponse( url, responseJson );
		}

		private static void RegisterListRawDataResponse( WebServer webServer, Guid partUuid, RawDataInformationDto[] responseObject )
		{
			var url = $"/RawDataServiceRest/rawData/Part?uuids=%7B{partUuid}%7D";
			var responseJson = JsonConvert.SerializeObject( responseObject );
			webServer.RegisterResponse( url, responseJson );
		}

		#endregion
	}
}