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
	};

	private static readonly IReadOnlyDictionary<Type, Func<object, IStructuralEquatable>> Equatables = new[]
	{
		CreateEquatable<AttributeDefinitionDto>(value => Tuple.Create( value.Key, value.Description, value.Length, value.QueryEfficient, value.Type )),

		CreateEquatable<CatalogAttributeDefinitionDto>(value => Tuple.Create( value.Key, value.Description, value.QueryEfficient, value.Catalog )),

		CreateEquatable<AttributeDto>(value => Tuple.Create( value.Key, value.Value ))
	}
	.ToDictionary( pair => pair.Key, pair => pair.Value );

	#endregion

	#region methods

	[TestCaseSource( nameof( TestCases ) )]
	public void Backward_Compatible<T>( T value )
	{
		var json = Newtonsoft.Json.JsonConvert.SerializeObject( value );

		var deserializedValue = System.Text.Json.JsonSerializer.Deserialize<T>( json );

		var equatable = Equatables[typeof( T )];

		var expected = equatable( value );
		var actual = equatable( deserializedValue );

		Assert.AreEqual( expected, actual, $"{typeof( T ).Name}" );
	}

	[TestCaseSource( nameof( TestCases ) )]
	public void Forward_Compatible<T>( T value )
	{
		var json = System.Text.Json.JsonSerializer.Serialize( value );

		var deserializedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>( json );

		var equatable = Equatables[typeof( T )];

		var expected = equatable( value );
		var actual = equatable( deserializedValue );

		Assert.AreEqual( expected, actual, $"{typeof( T ).Name}" );
	}

	private static KeyValuePair<Type, Func<object, IStructuralEquatable>> CreateEquatable<T>( Func<T, IStructuralEquatable> func )
	{
		return new( typeof( T ), value => func( (T)value ) );
	}

	#endregion
}