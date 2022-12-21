#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.HttpClient.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Core;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Data;
	using Attribute = Zeiss.PiWeb.Api.Core.Attribute;

	#endregion

	[TestFixture]
	public class DataServiceRestClientTest
	{
		#region constants

		private const int Port = 8081;

		#endregion

		#region members

		private static readonly Uri Uri = new Uri( $"http://localhost:{Port}/" );

		#endregion

		#region methods

		[Test]
		public async Task RequestSplit_OrderSpecified_OrderIsCorrect()
		{
			using var server = WebServer.StartNew( Port );
			//maxUriLength of 80 leads request splitting per part uuid
			using var client = new DataServiceRestClient( Uri, 80 );

			//attribute 999 is our reference order which we expect in our result
			var firstMeasurementSet = new[]
			{
				new SimpleMeasurementDto
					{ Attributes = new[] { new Attribute( 5, 3 ), new Attribute( 55, 10 ), new Attribute( 999, 2 ) } },
				new SimpleMeasurementDto
					{ Attributes = new[] { new Attribute( 5, 1 ), new Attribute( 55, 99 ), new Attribute( 999, 0 ) } },
				new SimpleMeasurementDto
					{ Attributes = new[] { new Attribute( 5, 6 ), new Attribute( 55, 99 ), new Attribute( 999, 5 ) } }
			};

			var secondMeasurementSet = new[]
			{
				new SimpleMeasurementDto
					{ Attributes = new[] { new Attribute( 5, 4 ), new Attribute( 55, 99 ), new Attribute( 999, 4 ) } },
				new SimpleMeasurementDto
					{ Attributes = new[] { new Attribute( 5, 2 ), new Attribute( 55, 99 ), new Attribute( 999, 1 ) } },
				new SimpleMeasurementDto
					{ Attributes = new[] { new Attribute( 5, 3 ), new Attribute( 55, 20 ), new Attribute( 999, 3 ) } }
			};

			server.RegisterResponse( "/DataServiceRest/measurements?partUuids=%7B11111111-1111-1111-1111-111111111111%7D&order=5%20Asc%2C55%20Asc", JsonConvert.SerializeObject( firstMeasurementSet ) );
			server.RegisterResponse( "/DataServiceRest/measurements?partUuids=%7B22222222-2222-2222-2222-222222222222%7D&order=5%20Asc%2C55%20Asc", JsonConvert.SerializeObject( secondMeasurementSet ) );

			var result = await client.GetMeasurements( filter: new MeasurementFilterAttributesDto
			{
				PartUuids = new[] { Guid.Parse( "11111111-1111-1111-1111-111111111111" ), Guid.Parse( "22222222-2222-2222-2222-222222222222" ) },
				OrderBy = new[] { new OrderDto( 5, OrderDirectionDto.Asc, EntityDto.Measurement ), new OrderDto( 55, OrderDirectionDto.Asc, EntityDto.Measurement ) }
			} );

			//check that the order is correct according to our attribute 999
			var index = 0;
			foreach( var measurement in result )
			{
				measurement.GetAttributeValue( 999 ).Should().Be( index.ToString() );
				index++;
			}
		}

		[Test]
		public async Task RequestSplit_LimitResultSpecified_LimitCorrect()
		{
			using var server = WebServer.StartNew( Port );
			//maxUriLength of 80 leads request splitting per part uuid
			using var client = new DataServiceRestClient( Uri, 80 );

			var firstMeasurementSet = new[] { new SimpleMeasurementDto(), new SimpleMeasurementDto(), new SimpleMeasurementDto() };

			var secondMeasurementSet = new[] { new SimpleMeasurementDto(), new SimpleMeasurementDto(), new SimpleMeasurementDto() };

			server.RegisterResponse( "/DataServiceRest/measurements?partUuids=%7B11111111-1111-1111-1111-111111111111%7D&limitResult=5&order=4%20Desc", JsonConvert.SerializeObject( firstMeasurementSet ) );
			server.RegisterResponse( "/DataServiceRest/measurements?partUuids=%7B22222222-2222-2222-2222-222222222222%7D&limitResult=5&order=4%20Desc", JsonConvert.SerializeObject( secondMeasurementSet ) );

			var result = await client.GetMeasurements( filter: new MeasurementFilterAttributesDto
			{
				PartUuids = new[] { Guid.Parse( "11111111-1111-1111-1111-111111111111" ), Guid.Parse( "22222222-2222-2222-2222-222222222222" ) },
				LimitResult = 5
			} );

			result.Count.Should().Be( 5 );
		}

		[Test]
		public async Task GetMeasurementValues_UriTooLong_RequestSplit()
		{
			using var server = WebServer.StartNew( Port );
			//maxUriLength of 200 leads to request splitting
			using var client = new DataServiceRestClient( Uri, 200 );

			server.RegisterDefaultResponse( "[]" );

			var uuids = new List<Guid>();

			for( var i = 0; i < 20; i++ )
				uuids.Add( Guid.NewGuid() );

			await client.GetMeasurementValues( filter: new MeasurementValueFilterAttributesDto { MeasurementUuids = uuids, CharacteristicsUuidList = uuids } );

			server.ReceivedRequests.Count().Should().BeGreaterThan( 5 );
		}

		[Test]
		public async Task GetMeasurementValues_UriTooLong_UriLengthCorrect()
		{
			using var server = WebServer.StartNew( Port );
			//maxUriLength of 200 leads to request splitting
			using var client = new DataServiceRestClient( Uri, 200 );

			server.RegisterDefaultResponse( "[]" );

			var uuids = new List<Guid>();

			for( var i = 0; i < 20; i++ )
				uuids.Add( Guid.NewGuid() );

			await client.GetMeasurementValues( filter: new MeasurementValueFilterAttributesDto { MeasurementUuids = uuids, CharacteristicsUuidList = uuids } );

			foreach( var request in server.ReceivedRequests )
				request.Length.Should().BeLessOrEqualTo( 200 );
		}

		[Test]
		public async Task GetMeasurements_UriTooLong_RequestSplit()
		{
			using var server = WebServer.StartNew( Port );
			//maxUriLength of 200 leads to request splitting
			using var client = new DataServiceRestClient( Uri, 200 );

			server.RegisterDefaultResponse( "[]" );

			var uuids = new List<Guid>();

			for( var i = 0; i < 20; i++ )
				uuids.Add( Guid.NewGuid() );

			await client.GetMeasurements( filter: new MeasurementFilterAttributesDto { MeasurementUuids = uuids } );

			server.ReceivedRequests.Count().Should().BeGreaterThan( 5 );
		}

		[Test]
		public async Task GetMeasurements_UriTooLong_UriLengthCorrect()
		{
			using var server = WebServer.StartNew( Port );
			//maxUriLength of 200 leads to request splitting
			using var client = new DataServiceRestClient( Uri, 200 );

			server.RegisterDefaultResponse( "[]" );

			var uuids = new List<Guid>();

			for( var i = 0; i < 20; i++ )
				uuids.Add( Guid.NewGuid() );

			await client.GetMeasurements( filter: new MeasurementFilterAttributesDto { MeasurementUuids = uuids } );

			foreach( var request in server.ReceivedRequests )
				request.Length.Should().BeLessOrEqualTo( 200 );
		}

		#endregion
	}
}