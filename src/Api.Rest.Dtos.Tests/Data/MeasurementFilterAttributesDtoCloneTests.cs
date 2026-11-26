#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System;
	using System.Linq;
	using System.Reflection;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class MeasurementFilterAttributesDtoCloneTests
	{
		#region methods

		[Test( Description = "Verifies that the clone method of the filter DTOs copies every property." )]
		public void Clone_MeasurementFilterAttributesDto_CopiesEveryProperty()
		{
			var filter = CreateFilter( new MeasurementFilterAttributesDto
			{
				RequestedMeasurementAttributes = new AttributeSelector( [10, 11] ),
				Statistics = MeasurementStatisticsDto.Detailed,
				MergeAttributes = [12, 13],
				MergeCondition = MeasurementMergeConditionDto.MeasurementsInAtLeastTwoParts,
				MergeMasterPart = Guid.NewGuid()
			} );

			var clone = filter.Clone();

			clone.Should().BeEquivalentTo( filter );
		}

		[Test( Description = "Verifies that the clone method of the filter DTOs copies every property." )]
		public void Clone_MeasurementValueFilterAttributesDto_CopiesEveryProperty()
		{
			var filter = CreateFilter( new MeasurementValueFilterAttributesDto
			{
				RequestedValueAttributes = new AttributeSelector( [14, 15] ),
				RequestedMeasurementAttributes = new AttributeSelector( AllAttributeSelectionDto.QueryEfficient ),
				CharacteristicsUuidList = [Guid.NewGuid(), Guid.NewGuid()],
				MergeAttributes = [16, 17],
				MergeCondition = MeasurementMergeConditionDto.None,
				MergeMasterPart = Guid.NewGuid()
			} );

			var clone = filter.Clone();

			clone.Should().BeEquivalentTo( filter );
		}

		[Test( Description = "Verifies that the clone method of the filter DTOs copies every property." )]
		public void Clone_DistinctMeasurementFilterAttributesDto_CopiesEveryProperty()
		{
			var filter = CreateFilter( new DistinctMeasurementFilterAttributesDto() );

			var clone = filter.Clone();

			clone.Should().BeEquivalentTo( filter );
		}

		[Test( Description = "Verifies the current set of writable properties of the filter DTOs. If this test fails, please check if the clone method needs to be updated." )]
		public void Clone_MeasurementFilterAttributesDto_HasExpectedWritableProperties()
		{
			AssertWritableProperties<MeasurementFilterAttributesDto>(
				nameof( AbstractMeasurementFilterAttributesDto.AggregationMeasurements ),
				nameof( AbstractMeasurementFilterAttributesDto.CaseSensitive ),
				nameof( AbstractMeasurementFilterAttributesDto.Deep ),
				nameof( AbstractMeasurementFilterAttributesDto.FromModificationDate ),
				nameof( AbstractMeasurementFilterAttributesDto.LimitResult ),
				nameof( AbstractMeasurementFilterAttributesDto.LimitResultPerPart ),
				nameof( MeasurementFilterAttributesDto.MergeAttributes ),
				nameof( MeasurementFilterAttributesDto.MergeCondition ),
				nameof( MeasurementFilterAttributesDto.MergeMasterPart ),
				nameof( AbstractMeasurementFilterAttributesDto.MeasurementUuids ),
				nameof( AbstractMeasurementFilterAttributesDto.OrderBy ),
				nameof( AbstractMeasurementFilterAttributesDto.PartUuids ),
				nameof( MeasurementFilterAttributesDto.RequestedMeasurementAttributes ),
				nameof( AbstractMeasurementFilterAttributesDto.SearchCondition ),
				nameof( MeasurementFilterAttributesDto.Statistics ),
				nameof( AbstractMeasurementFilterAttributesDto.ToModificationDate ) );
		}

		[Test( Description = "Verifies the current set of writable properties of the filter DTOs. If this test fails, please check if the clone method needs to be updated." )]
		public void Clone_MeasurementValueFilterAttributesDto_HasExpectedWritableProperties()
		{
			AssertWritableProperties<MeasurementValueFilterAttributesDto>(
				nameof( AbstractMeasurementFilterAttributesDto.AggregationMeasurements ),
				nameof( AbstractMeasurementFilterAttributesDto.CaseSensitive ),
				nameof( MeasurementValueFilterAttributesDto.CharacteristicsUuidList ),
				nameof( AbstractMeasurementFilterAttributesDto.Deep ),
				nameof( AbstractMeasurementFilterAttributesDto.FromModificationDate ),
				nameof( AbstractMeasurementFilterAttributesDto.LimitResult ),
				nameof( AbstractMeasurementFilterAttributesDto.LimitResultPerPart ),
				nameof( MeasurementValueFilterAttributesDto.MergeAttributes ),
				nameof( MeasurementValueFilterAttributesDto.MergeCondition ),
				nameof( MeasurementValueFilterAttributesDto.MergeMasterPart ),
				nameof( AbstractMeasurementFilterAttributesDto.MeasurementUuids ),
				nameof( AbstractMeasurementFilterAttributesDto.OrderBy ),
				nameof( AbstractMeasurementFilterAttributesDto.PartUuids ),
				nameof( MeasurementValueFilterAttributesDto.RequestedMeasurementAttributes ),
				nameof( MeasurementValueFilterAttributesDto.RequestedValueAttributes ),
				nameof( AbstractMeasurementFilterAttributesDto.SearchCondition ),
				nameof( AbstractMeasurementFilterAttributesDto.ToModificationDate ) );
		}

		[Test( Description = "Verifies the current set of writable properties of the filter DTOs. If this test fails, please check if the clone method needs to be updated." )]
		public void Clone_DistinctMeasurementFilterAttributesDto_HasExpectedWritableProperties()
		{
			AssertWritableProperties<DistinctMeasurementFilterAttributesDto>(
				nameof( AbstractMeasurementFilterAttributesDto.AggregationMeasurements ),
				nameof( AbstractMeasurementFilterAttributesDto.CaseSensitive ),
				nameof( AbstractMeasurementFilterAttributesDto.Deep ),
				nameof( AbstractMeasurementFilterAttributesDto.FromModificationDate ),
				nameof( AbstractMeasurementFilterAttributesDto.LimitResult ),
				nameof( AbstractMeasurementFilterAttributesDto.LimitResultPerPart ),
				nameof( AbstractMeasurementFilterAttributesDto.MeasurementUuids ),
				nameof( AbstractMeasurementFilterAttributesDto.OrderBy ),
				nameof( AbstractMeasurementFilterAttributesDto.PartUuids ),
				nameof( AbstractMeasurementFilterAttributesDto.SearchCondition ),
				nameof( AbstractMeasurementFilterAttributesDto.ToModificationDate ) );
		}

		private static void AssertWritableProperties<T>( params string[] expectedProperties )
		{
			var propertyNames = typeof( T ).GetProperties( BindingFlags.Instance | BindingFlags.Public )
				.Where( property => property.GetIndexParameters().Length == 0 && property.SetMethod?.IsPublic == true )
				.Select( property => property.Name );

			propertyNames.Should().BeEquivalentTo( expectedProperties );
		}

		private static T CreateFilter<T>( T filter ) where T : AbstractMeasurementFilterAttributesDto
		{
			filter.PartUuids = [Guid.NewGuid(), Guid.NewGuid()];
			filter.Deep = true;
			filter.LimitResult = 100;
			filter.LimitResultPerPart = 25;
			filter.OrderBy = [new OrderDto( 18, OrderDirectionDto.Asc, EntityDto.Measurement )];
			filter.SearchCondition = new GenericSearchAndDto( [
				new GenericSearchFieldConditionDto { FieldName = "testField", Value = "testValue" }
			] );
			filter.CaseSensitive = true;
			filter.MeasurementUuids = [Guid.NewGuid(), Guid.NewGuid()];
			filter.AggregationMeasurements = AggregationMeasurementSelectionDto.All;
			filter.FromModificationDate = new DateTime( 2020, 1, 2, 3, 4, 5, DateTimeKind.Utc );
			filter.ToModificationDate = new DateTime( 2021, 6, 7, 8, 9, 10, DateTimeKind.Utc );

			return filter;
		}

		#endregion
	}
}