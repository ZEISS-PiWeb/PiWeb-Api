#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System.Linq;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class GenericSearchConditionSerializationTests
	{
		#region methods

		[Test]
		public void GenericSearchConditionConverter_SerializesNotConditionProperly()
		{
			var original = (GenericSearchConditionDto)new GenericSearchNotDto
			{
				Condition = new GenericSearchFieldConditionDto
				{
					FieldName = "TestName",
					Operation = OperationDto.GreaterThan,
					Value = "TestValue"
				}
			};


			var serialized = JsonConvert.SerializeObject( original, Formatting.None, new GenericSearchConditionConverter() );
			var deserialized = JsonConvert.DeserializeObject<GenericSearchConditionDto>( serialized, new GenericSearchConditionConverter() );
			var typedDeserialized = deserialized as GenericSearchNotDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchNotDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.Condition, Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		[Test]
		public void GenericSearchConditionConverter_SerializesAndConditionProperly()
		{
			var original = (GenericSearchConditionDto)new GenericSearchAndDto
			{
				Conditions = new[]
				{
					new GenericSearchFieldConditionDto
					{
						FieldName = "TestName",
						Operation = OperationDto.GreaterThan,
						Value = "TestValue"
					},
					new GenericSearchFieldConditionDto
					{
						FieldName = "Test2Name",
						Operation = OperationDto.GreaterThan,
						Value = "Test2Value"
					}
				}
			};

			var serialized = JsonConvert.SerializeObject( original, Formatting.None, new GenericSearchConditionConverter() );
			var deserialized = JsonConvert.DeserializeObject<GenericSearchConditionDto>( serialized, new GenericSearchConditionConverter() );
			var typedDeserialized = deserialized as GenericSearchAndDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchAndDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.Conditions, Has.Exactly( 2 ).Items );
			Assert.That( typedDeserialized.Conditions.ElementAt( 0 ), Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( typedDeserialized.Conditions.ElementAt( 1 ), Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		[Test]
		public void GenericSearchConditionConverter_SerializesAttributeConditionProperly()
		{
			var original = new GenericSearchAttributeConditionDto
			{
				Attribute = 123,
				Operation = OperationDto.GreaterThan,
				Value = "TestValue"
			};

			var serialized = JsonConvert.SerializeObject( original, Formatting.None, new GenericSearchConditionConverter() );
			var deserialized = JsonConvert.DeserializeObject<GenericSearchConditionDto>( serialized, new GenericSearchConditionConverter() );
			var typedDeserialized = deserialized as GenericSearchAttributeConditionDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchAttributeConditionDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.Attribute, Is.EqualTo( 123 ) );
			Assert.That( typedDeserialized.Operation, Is.EqualTo( OperationDto.GreaterThan ) );
			Assert.That( typedDeserialized.Value, Is.EqualTo( "TestValue" ) );
			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		[Test]
		public void GenericSearchConditionConverter_SerializesFieldConditionProperly()
		{
			var original = new GenericSearchFieldConditionDto
			{
				FieldName = "TestName",
				Operation = OperationDto.GreaterThan,
				Value = "TestValue"
			};

			var serialized = JsonConvert.SerializeObject( original, Formatting.None, new GenericSearchConditionConverter() );
			var deserialized = JsonConvert.DeserializeObject<GenericSearchConditionDto>( serialized, new GenericSearchConditionConverter() );
			var typedDeserialized = deserialized as GenericSearchFieldConditionDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.FieldName, Is.EqualTo( "TestName" ) );
			Assert.That( typedDeserialized.Operation, Is.EqualTo( OperationDto.GreaterThan ) );
			Assert.That( typedDeserialized.Value, Is.EqualTo( "TestValue" ) );
			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		#endregion
	}
}