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
using NUnit.Framework;
using Zeiss.PiWeb.Api.Rest.Dtos.Data;

#endregion

[TestFixture]
public class CompatibilityTests
{
	#region members

	private static readonly AttributeDefinitionDto AttributeDefinition = new()
	{
		Key = 13,
		Description = "Test",
		Length = 43,
		QueryEfficient = false,
		Type = AttributeTypeDto.AlphaNumeric
	};

	private static readonly CatalogAttributeDefinitionDto CatalogAttributeDefinition = new()
	{
		Key = 13,
		Description = "Test",
		QueryEfficient = false,
		Catalog = new Guid( "11D4115C-41A7-4D47-A353-AF5DF61503EA" )
	};

	private static object[] TestCases =
	{
		new object[] { AttributeDefinition, (AttributeDefinitionDto value) => Tuple.Create( value.Key, value.Description, value.Length, value.QueryEfficient, value.Type ) },
		new object[] { CatalogAttributeDefinition, (CatalogAttributeDefinitionDto value ) => Tuple.Create( value.Key, value.Description, value.QueryEfficient, value.Catalog ) },
	};

	#endregion

	#region methods

	[TestCaseSource( nameof( TestCases ) )]
	public void Backward_Compatible<T>( T value, Func<T, IStructuralEquatable> createEquatable )
	{
		var json = Newtonsoft.Json.JsonConvert.SerializeObject( value );

		var deserializedValue = System.Text.Json.JsonSerializer.Deserialize<T>( json );

		var expected = createEquatable( value );
		var actual = createEquatable( deserializedValue );

		Assert.AreEqual( expected, actual, $"{typeof( T ).Name}" );
	}

	[TestCaseSource( nameof( TestCases ) )]
	public void Forward_Compatible<T>( T value, Func<T, IStructuralEquatable> createEquatable )
	{
		var json = System.Text.Json.JsonSerializer.Serialize( value );

		var deserializedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>( json );

		var expected = createEquatable( value );
		var actual = createEquatable( deserializedValue );

		Assert.AreEqual( expected, actual, $"{typeof( T ).Name}" );
	}

	#endregion
}