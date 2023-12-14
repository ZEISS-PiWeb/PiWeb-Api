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
	using System.Text.Json;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters;
	using JsonSerializer = System.Text.Json.JsonSerializer;

	#endregion

	[TestFixture]
	public class GenericSearchConditionSerializationTests
	{
		#region members

		private readonly JsonSerializerOptions _Options = new() { Converters = { new JsonGenericSearchConditionDtoConverter() } };

		#endregion

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
		public void GenericSearchConditionConverter_SerializesAndConditionWithDifferentConditionsProperly()
		{
			var original = (GenericSearchConditionDto)new GenericSearchAndDto
			{
				Conditions = new GenericSearchConditionDto[]
				{
					new GenericSearchFieldConditionDto
					{
						FieldName = "TestName",
						Operation = OperationDto.GreaterThan,
						Value = "TestValue"
					},
					new GenericSearchAttributeConditionDto
					{
						Attribute = 4,
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
			Assert.That( typedDeserialized.Conditions.ElementAt( 1 ), Is.TypeOf<GenericSearchAttributeConditionDto>() );
			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		[Test]
		public void GenericSearchConditionConverter_SerializesNestedConditionProperly()
		{
			var original = (GenericSearchConditionDto)new GenericSearchAndDto
			{
				Conditions = new GenericSearchConditionDto[]
				{
					new GenericSearchFieldConditionDto
					{
						FieldName = "TestName",
						Operation = OperationDto.GreaterThan,
						Value = "TestValue"
					},
					new GenericSearchAttributeConditionDto
					{
						Attribute = 4,
						Operation = OperationDto.GreaterThan,
						Value = "Test2Value"
					},
					new GenericSearchAndDto
					{
						Conditions = new GenericSearchConditionDto[]
						{
							new GenericSearchFieldConditionDto
							{
								FieldName = "NestedTestName",
								Operation = OperationDto.GreaterThan,
								Value = "NestedTestValue"
							},
							new GenericSearchAttributeConditionDto
							{
								Attribute = 8,
								Operation = OperationDto.GreaterThan,
								Value = "NestedTestValue2"
							}
						}
					}
				}
			};

			var serialized = JsonConvert.SerializeObject( original, Formatting.None, new GenericSearchConditionConverter() );
			var deserialized = JsonConvert.DeserializeObject<GenericSearchConditionDto>( serialized, new GenericSearchConditionConverter() );
			var typedDeserialized = deserialized as GenericSearchAndDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchAndDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.Conditions, Has.Exactly( 3 ).Items );
			Assert.That( typedDeserialized.Conditions.ElementAt( 0 ), Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( typedDeserialized.Conditions.ElementAt( 1 ), Is.TypeOf<GenericSearchAttributeConditionDto>() );
			Assert.That( typedDeserialized.Conditions.ElementAt( 2 ), Is.TypeOf<GenericSearchAndDto>() );

			var nestedDeserialized = typedDeserialized.Conditions.Last() as GenericSearchAndDto;
			Assert.That( nestedDeserialized, Is.Not.Null );

			var nestedFieldCondition = nestedDeserialized.Conditions.First() as GenericSearchFieldConditionDto;
			Assert.That( nestedFieldCondition, Is.Not.Null );
			Assert.That( nestedFieldCondition.FieldName, Is.EqualTo( "NestedTestName" ) );
			Assert.That( nestedFieldCondition.Value, Is.EqualTo( "NestedTestValue" ) );
			Assert.That( nestedFieldCondition.Operation, Is.EqualTo( OperationDto.GreaterThan ) );

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

		[Test]
		public void JsonGenericSearchConditionConverter_SerializesNotConditionProperly()
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

			var serialized = JsonSerializer.Serialize( original, _Options );
			var deserialized = JsonSerializer.Deserialize<GenericSearchConditionDto>( serialized, _Options );
			var typedDeserialized = deserialized as GenericSearchNotDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchNotDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.Condition, Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		[Test]
		public void JsonGenericSearchConditionConverter_SerializesAndConditionProperly()
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

			var serialized = JsonSerializer.Serialize( original, _Options );
			var deserialized = JsonSerializer.Deserialize<GenericSearchConditionDto>( serialized, _Options );
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
		public void JsonGenericSearchConditionConverter_SerializesAndConditionWithDifferentConditionsProperly()
		{
			var original = (GenericSearchConditionDto)new GenericSearchAndDto
			{
				Conditions = new GenericSearchConditionDto[]
				{
					new GenericSearchFieldConditionDto
					{
						FieldName = "TestName",
						Operation = OperationDto.GreaterThan,
						Value = "TestValue"
					},
					new GenericSearchAttributeConditionDto
					{
						Attribute = 4,
						Operation = OperationDto.GreaterThan,
						Value = "Test2Value"
					}
				}
			};

			var serialized = JsonSerializer.Serialize( original, _Options );
			var deserialized = JsonSerializer.Deserialize<GenericSearchConditionDto>( serialized, _Options );
			var typedDeserialized = deserialized as GenericSearchAndDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchAndDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.Conditions, Has.Exactly( 2 ).Items );
			Assert.That( typedDeserialized.Conditions.ElementAt( 0 ), Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( typedDeserialized.Conditions.ElementAt( 1 ), Is.TypeOf<GenericSearchAttributeConditionDto>() );
			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		[Test]
		public void JsonGenericSearchConditionConverter_SerializesNestedConditionProperly()
		{
			var original = (GenericSearchConditionDto)new GenericSearchAndDto
			{
				Conditions = new GenericSearchConditionDto[]
				{
					new GenericSearchFieldConditionDto
					{
						FieldName = "TestName",
						Operation = OperationDto.GreaterThan,
						Value = "TestValue"
					},
					new GenericSearchAttributeConditionDto
					{
						Attribute = 4,
						Operation = OperationDto.GreaterThan,
						Value = "Test2Value"
					},
					new GenericSearchAndDto
					{
						Conditions = new GenericSearchConditionDto[]
						{
							new GenericSearchFieldConditionDto
							{
								FieldName = "NestedTestName",
								Operation = OperationDto.GreaterThan,
								Value = "NestedTestValue"
							},
							new GenericSearchAttributeConditionDto
							{
								Attribute = 8,
								Operation = OperationDto.GreaterThan,
								Value = "NestedTestValue2"
							}
						}
					}
				}
			};

			var serialized = JsonSerializer.Serialize( original, _Options );
			var deserialized = JsonSerializer.Deserialize<GenericSearchConditionDto>( serialized, _Options );
			var typedDeserialized = deserialized as GenericSearchAndDto;
			var serializedAgain = JsonConvert.SerializeObject( deserialized, Formatting.None, new GenericSearchConditionConverter() );

			Assert.That( deserialized, Is.TypeOf<GenericSearchAndDto>() );
			Assert.That( typedDeserialized, Is.Not.Null );
			Assert.That( typedDeserialized.Conditions, Has.Exactly( 3 ).Items );
			Assert.That( typedDeserialized.Conditions.ElementAt( 0 ), Is.TypeOf<GenericSearchFieldConditionDto>() );
			Assert.That( typedDeserialized.Conditions.ElementAt( 1 ), Is.TypeOf<GenericSearchAttributeConditionDto>() );
			Assert.That( typedDeserialized.Conditions.ElementAt( 2 ), Is.TypeOf<GenericSearchAndDto>() );

			var nestedDeserialized = typedDeserialized.Conditions.Last() as GenericSearchAndDto;
			Assert.That( nestedDeserialized, Is.Not.Null );

			var nestedFieldCondition = nestedDeserialized.Conditions.First() as GenericSearchFieldConditionDto;
			Assert.That( nestedFieldCondition, Is.Not.Null );
			Assert.That( nestedFieldCondition.FieldName, Is.EqualTo( "NestedTestName" ) );
			Assert.That( nestedFieldCondition.Value, Is.EqualTo( "NestedTestValue" ) );
			Assert.That( nestedFieldCondition.Operation, Is.EqualTo( OperationDto.GreaterThan ) );

			Assert.That( serialized, Is.EqualTo( serializedAgain ) );
		}

		[Test]
		public void JsonGenericSearchConditionConverter_SerializesAttributeConditionProperly()
		{
			var original = new GenericSearchAttributeConditionDto
			{
				Attribute = 123,
				Operation = OperationDto.GreaterThan,
				Value = "TestValue"
			};

			var serialized = JsonSerializer.Serialize( original, _Options );
			var deserialized = JsonSerializer.Deserialize<GenericSearchConditionDto>( serialized, _Options );
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
		public void JsonGenericSearchConditionConverter_SerializesFieldConditionProperly()
		{
			var original = new GenericSearchFieldConditionDto
			{
				FieldName = "TestName",
				Operation = OperationDto.GreaterThan,
				Value = "TestValue"
			};

			var serialized = JsonSerializer.Serialize( original, _Options );
			var deserialized = JsonSerializer.Deserialize<GenericSearchConditionDto>( serialized, _Options );
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