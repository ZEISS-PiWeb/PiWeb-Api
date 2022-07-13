#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Compatibility;

#region usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Zeiss.PiWeb.Api.Rest.Dtos.Data;

using static PiWeb.Api.Definitions.WellKnownKeys;

#endregion

[TestFixture]
public class CompatibilityTests
{
	#region members

	private static readonly object[] TestCases =
	{
		new AttributeDefinitionDto
		{
			Key = 13,
			Description = "Test",
			Length = 43,
			QueryEfficient = false,
			Type = AttributeTypeDto.AlphaNumeric
		},

		new CatalogAttributeDefinitionDto()
		{
			Key = 13,
			Description = "Test",
			QueryEfficient = false,
			Catalog = new Guid( "11D4115C-41A7-4D47-A353-AF5DF61503EA" )
		},

		new AttributeDto( Characteristic.MeasurementModule, "Abweichungen" ),
		new AttributeDto( Characteristic.X, 2919.041 ),
		new AttributeDto( Characteristic.AlignmentType, 0 ),
		new AttributeDto( Measurement.Time, new DateTime( 2010, 11, 04, 19, 44, 52, 8, DateTimeKind.Utc ) ),
		new AttributeDto( Measurement.Time, (object)null ),

		new CatalogDto
		{
			Name = "TestCatalog",
			Uuid = new Guid( "8B23C6BB-EE2A-4BB3-A627-512C911F6FE8" ),
			ValidAttributes = new [] { Catalog.OperatorNumber, Catalog.OperatorName },
			CatalogEntries = new []
			{
				new CatalogEntryDto
				{
					Key = 1,
					Attributes = new[]
					{
						new AttributeDto( Catalog.OperatorNumber, 13 ),
						new AttributeDto( Catalog.OperatorName, "Müller" ),
					}
				},
				new CatalogEntryDto
				{
					Key = 2,
					Attributes = new[]
					{
						new AttributeDto( Catalog.OperatorNumber, 42 ),
						new AttributeDto( Catalog.OperatorName, "Meier" ),
					}
				},
				new CatalogEntryDto
				{
					Key = 3,
					Attributes = new[]
					{
						new AttributeDto( Catalog.OperatorNumber, 73 ),
						new AttributeDto( Catalog.OperatorName, "Schulze" ),
					}
				}
			}
		}
	};

	private static readonly IReadOnlyDictionary<Type, Func<object, IStructuralEquatable>> EquatableFromType = new[]
	{
		EquatableFrom<AttributeDefinitionDto>(value => Tuple.Create( value.Key, value.Description, value.Length, value.QueryEfficient, value.Type )),

		EquatableFrom<CatalogAttributeDefinitionDto>(value => Tuple.Create( value.Key, value.Description, value.QueryEfficient, value.Catalog )),

		EquatableFrom<AttributeDto>(value => Tuple.Create( value.Key, value.Value )),

		EquatableFrom<CatalogDto>(value => Tuple.Create( value.Name, value.Uuid, value.ValidAttributes.ToArray(), EquatableFromMany( value.CatalogEntries ) ) ),

		EquatableFrom<CatalogEntryDto>(value => Tuple.Create( value.Key, EquatableFromMany( value.Attributes ) ) ),
	}
	.ToDictionary( pair => pair.Key, pair => pair.Value );

	#endregion

	#region methods

	[TestCaseSource( nameof( TestCases ) )]
	public void Backward_Compatible<T>( T value )
	{
		var json = Newtonsoft.Json.JsonConvert.SerializeObject( value );

		var deserializedValue = System.Text.Json.JsonSerializer.Deserialize<T>( json );

		var equatable = EquatableFromType[typeof( T )];

		var expected = equatable( value );
		var actual = equatable( deserializedValue );

		Assert.AreEqual( expected, actual, $"{typeof( T ).Name}" );
	}

	[TestCaseSource( nameof( TestCases ) )]
	public void Forward_Compatible<T>( T value )
	{
		var json = System.Text.Json.JsonSerializer.Serialize( value );

		var deserializedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>( json );

		var equatable = EquatableFromType[typeof( T )];

		var expected = equatable( value );
		var actual = equatable( deserializedValue );

		Assert.AreEqual( expected, actual, $"{typeof( T ).Name}" );
	}

	private static KeyValuePair<Type, Func<object, IStructuralEquatable>> EquatableFrom<T>( Func<T, IStructuralEquatable> func )
	{
		return new( typeof( T ), value => func( (T)value ) );
	}

	private static IStructuralEquatable EquatableFromMany<T>( IEnumerable<T> values )
	{
		return values.Select( value => EquatableFromType[typeof( T )]( value ) ).ToArray();
	}

	#endregion
}