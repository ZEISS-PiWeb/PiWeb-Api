#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Compatibility
{
	#region usings

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Net.Mime;
	using System.Text;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Core;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters;
	using Zeiss.PiWeb.Api.Rest.Dtos.RawData;
	using static PiWeb.Api.Definitions.WellKnownKeys;
	using Attribute = Zeiss.PiWeb.Api.Core.Attribute;

	#endregion

	[TestFixture]
	public class CompatibilityTests
	{
		#region members

		private static readonly object[] TestCases =
		{
			new AttributeDefinitionDto { Key = 13, Description = "Test", Length = 43, QueryEfficient = false, Type = AttributeTypeDto.AlphaNumeric },

			new CatalogAttributeDefinitionDto { Key = 13, Description = "Test", QueryEfficient = false, Catalog = new Guid( "11D4115C-41A7-4D47-A353-AF5DF61503EA" ) },

			new Attribute( Characteristic.MeasurementModule, "Abweichungen" ),
			new Attribute( Characteristic.X, 2919.0 ),
			new Attribute( Characteristic.AlignmentType, 0 ),
			new Attribute( Measurement.Time, new DateTime( 2010, 11, 04, 19, 44, 52, 8, DateTimeKind.Utc ) ),
			new Attribute( Measurement.Time, (object)null ),

			new CatalogDto
			{
				Name = "TestCatalog",
				Uuid = new Guid( "8B23C6BB-EE2A-4BB3-A627-512C911F6FE8" ),
				ValidAttributes = new[] { Catalog.OperatorNumber, Catalog.OperatorName },
				CatalogEntries = new[]
				{
					new CatalogEntryDto
					{
						Key = 1,
						Attributes = new[]
						{
							new Attribute( Catalog.OperatorNumber, 13 ),
							new Attribute( Catalog.OperatorName, "Müller" ),
						}
					},
					new CatalogEntryDto
					{
						Key = 2,
						Attributes = new[]
						{
							new Attribute( Catalog.OperatorNumber, 42 ),
							new Attribute( Catalog.OperatorName, "Meier" ),
						}
					},
					new CatalogEntryDto
					{
						Key = 3,
						Attributes = new[]
						{
							new Attribute( Catalog.OperatorNumber, 73 ),
							new Attribute( Catalog.OperatorName, "Schulze" ),
						}
					}
				}
			},

			new ConfigurationDto
			{
				PartAttributes = new[]
				{
					new AttributeDefinitionDto { Key = Part.Number, Description = "Teilenummer", Length = 30, Type = AttributeTypeDto.AlphaNumeric },
					new AttributeDefinitionDto { Key = Part.VariantOfLine, Description = "Modell/Variante", Length = 40, Type = AttributeTypeDto.AlphaNumeric }
				},

				CharacteristicAttributes = new AbstractAttributeDefinitionDto[]
				{
					new AttributeDefinitionDto { Key = Characteristic.Number, Description = "Merkmalsnummer", Length = 20, Type = AttributeTypeDto.AlphaNumeric },
					new AttributeDefinitionDto { Key = Characteristic.ControlItem, Description = "Dokumentationspflicht", Type = AttributeTypeDto.Integer },
					new CatalogAttributeDefinitionDto { Key = 2004, Description = "Richtung", Catalog = new Guid( "d7291afb-0a67-4c1e-8bcc-6fc455bcc0e5" ) },
				},

				MeasurementAttributes = new[]
				{
					new AttributeDefinitionDto { Key = Measurement.Time, Description = "Zeit", Type = AttributeTypeDto.DateTime },
					new AttributeDefinitionDto { Key = 1, Description = "Messwert", Type = AttributeTypeDto.Float }
				},

				ValueAttributes = new AbstractAttributeDefinitionDto[]
				{
				},

				CatalogAttributes = new[]
				{
					new AttributeDefinitionDto { Key = 2009, Description = "Richtung", Length = 10, Type = AttributeTypeDto.AlphaNumeric }
				},

				VersioningType = VersioningTypeDto.Off
			},

			new DataValueDto
			{
				Attributes = new[]
				{
					new Attribute( Value.MeasuredValue, 0.15108030390438515 ),
					new Attribute( Value.AggregatedCp, 0.25108030390438873 ),
					new Attribute( Value.AggregatedMaximum, 0.551080303904385 )
				}
			},

			new SimpleMeasurementDto
			{
				Uuid = new Guid( "82DA1300-CC13-4920-A272-0AA33C4001A3" ),
				PartUuid = new Guid( "CF1938E5-C8D6-468D-9BC9-640A60A54105" ),
				LastModified = new DateTime( 2010, 11, 04, 19, 44, 52, 8, DateTimeKind.Utc ),
				Created = new DateTime( 2010, 12, 05, 22, 37, 49, 12, DateTimeKind.Utc ),
				Status = new[]
				{
					new SimpleMeasurementStatusDto { Id = "InTol", Count = 1, Uuid = new [] { new Guid( "575E6026-29EB-49D9-92D8-9E534BC04BD5" ) } },
					new SimpleMeasurementStatusDto { Id = "OutTol", Count = 2, Uuid = new [] { new Guid( "F45098BF-0C3B-42AC-BF20-E19907A9350C" ), new Guid( "D6DFBC39-D398-4311-BB3E-22BAD360F77E" ) } },
					new SimpleMeasurementStatusDto { Id = "OutWarn", Count = 0 },
				},
				Attributes = new[]
				{
					new Attribute( Measurement.Time, new DateTime( 2022, 01, 31, 19, 02, 59, 71, DateTimeKind.Utc) ),
					new Attribute( Measurement.BatchNumber, "5" ),
					new Attribute( Measurement.InspectorName, 2 ),
				}
			},

			new DataMeasurementDto
			{
				Uuid = new Guid( "82DA1300-CC13-4920-A272-0AA33C4001A3" ),
				PartUuid = new Guid( "CF1938E5-C8D6-468D-9BC9-640A60A54105" ),
				LastModified = new DateTime( 2010, 11, 04, 19, 44, 52, 8, DateTimeKind.Utc ),
				Created = new DateTime( 2010, 12, 05, 22, 37, 49, 12, DateTimeKind.Utc ),
				Attributes = new[]
				{
					new Attribute( Measurement.Time, new DateTime( 2022, 01, 31, 19, 02, 59, 71, DateTimeKind.Utc) ),
					new Attribute( Measurement.BatchNumber, "5" ),
					new Attribute( Measurement.InspectorName, 2 ),
				},
				Characteristics = new Dictionary<Guid, DataValueDto>
				{
					{ new Guid( "b71a5bd7-5406-46a3-a5b7-458ba1c0248d" ), new DataValueDto( 0.15108030390438515 ) },
					{ new Guid( "8c72afa6-fc67-4fbd-8606-e3727d79c8ff" ), new DataValueDto( -0.06273457511599848 ) },
					{ new Guid( "a5d13d7f-4029-4fb5-a7b5-3f40718df85a" ), new DataValueDto( 0.03185869918795966 ) }
				}
			},

			new InspectionPlanCharacteristicDto
			{
				Path = PathHelper.RoundtripString2PathInformation( "PC:/Blechteil/Abweichung_3/" ),
				Uuid = new Guid( "b71a5bd7-5406-46a3-a5b7-458ba1c0248d" ),
				Version = 0,
				Timestamp = new DateTime( 2022, 01, 31, 19, 02, 58, 683, DateTimeKind.Utc ),
				Attributes = new[]
				{
					new Attribute( Characteristic.NominalValue, 0.0 ),
					new Attribute( Characteristic.LowerSpecificationLimit, -0.5 ),
					new Attribute( Characteristic.UpperSpecificationLimit, 0.5 )
				}
			},

			new InspectionPlanPartDto
			{
				Path = PathHelper.RoundtripString2PathInformation( "P:/Blechteil/" ),
				CharChangeDate = new DateTime( 2022, 01, 31, 19, 2, 58, 767, DateTimeKind.Utc ),
				Uuid = new Guid( "fe85eefe-f08d-4e78-9f06-0e3b3cc9275e" ),
				Version = 0,
				Timestamp = new DateTime( 2022, 06, 30, 6, 25, 35, 46, DateTimeKind.Utc ),
				Attributes = new[]
				{
					new Attribute( Part.Number, 122345 ),
					new Attribute( Part.Abbreviation, "Blechteil" ),
					new Attribute( Part.Organisation, "Presswerk" )
				}
			},

			new OperationStatusDto { OperationUuid = new Guid( "7825FE38-1486-45CC-AA95-70658B83C7ED" ), ExecutionStatus = OperationExecutionStatusDto.Running, Exception = default },
			new OperationStatusDto { OperationUuid = new Guid( "7AE4BD0A-5EA4-4665-AB60-9D517E23BE6D" ), ExecutionStatus = OperationExecutionStatusDto.Exception, Exception = new Error("ErrorMessage") },

			new OrderDto { Entity = EntityDto.Characteristic, Attribute = Characteristic.LowerTolerance, Direction = OrderDirectionDto.Asc },
			new OrderDto { Entity = EntityDto.Measurement, Attribute = Measurement.MeasurementStatus, Direction = OrderDirectionDto.Desc },

			new Dtos.Data.ServiceInformationDto
			{
				EditionSpecified = true,
				ServerName = "TestServer",
				Version = "8.4.1.0",
				SecurityEnabled = true,
				Edition = "PiWebDB",
				FeatureList = new[] { "MeasurementAggregation", "JobEngineSupported" },
				PartCount = 1,
				CharacteristicCount = 13,
				MeasurementCount = 42,
				ValueCount = 73,
				InspectionPlanTimestamp = new DateTime( 2022, 06, 23, 10, 42, 0, 931, DateTimeKind.Utc ),
				MeasurementTimestamp = new DateTime( 2022, 03, 30, 15, 29, 43, 324, DateTimeKind.Utc ),
				ConfigurationTimestamp = new DateTime( 2022, 07, 13, 9, 35, 28, 858, DateTimeKind.Utc ),
				CatalogTimestamp = new DateTime( 2022, 03, 30, 15, 29, 42, 816, DateTimeKind.Utc ),
				StructureTimestamp = new DateTime( 2022, 03, 30, 15, 29, 41, 254, DateTimeKind.Utc ),
				RequestHeaderSize = 0
			},

			new InterfaceVersionRange { SupportedVersions = new[] { new Version(7, 8, 2), new Version(8, 0), new Version(8, 2, 8) } },

			new InspectionPlanDtoBase[]
			{
				new InspectionPlanCharacteristicDto
				{
					Path = PathHelper.RoundtripString2PathInformation( "PC:/Blechteil/Abweichung_3/" ),
					Uuid = new Guid( "b71a5bd7-5406-46a3-a5b7-458ba1c0248d" ),
					Version = 0,
					Timestamp = new DateTime( 2022, 01, 31, 19, 02, 58, 683, DateTimeKind.Utc ),
					Attributes = new[]
					{
						new Attribute( Characteristic.NominalValue, 0.0 ),
						new Attribute( Characteristic.LowerSpecificationLimit, -0.5 ),
						new Attribute( Characteristic.UpperSpecificationLimit, 0.5 )
					}
				},

				new InspectionPlanPartDto
				{
					Path = PathHelper.RoundtripString2PathInformation( "P:/Blechteil/" ),
					CharChangeDate = new DateTime( 2022, 01, 31, 19, 2, 58, 767, DateTimeKind.Utc ),
					Uuid = new Guid( "fe85eefe-f08d-4e78-9f06-0e3b3cc9275e" ),
					Version = 1,
					Timestamp = new DateTime( 2022, 06, 30, 6, 25, 35, 46, DateTimeKind.Utc ),
					Attributes = new[]
					{
						new Attribute( Part.Number, 122345 ),
						new Attribute( Part.Abbreviation, "Blechteil" ),
						new Attribute( Part.Organisation, "Presswerk" )
					},
					History = new[]
					{
						new InspectionPlanPartDto
						{
							Path = PathHelper.RoundtripString2PathInformation( "P:/Blechteil/" ),
							CharChangeDate = new DateTime( 2022, 01, 30, 18, 5, 46, 124, DateTimeKind.Utc ),
							Uuid = new Guid( "fe85eefe-f08d-4e78-9f06-0e3b3cc9275e" ),
							Version = 0,
							Timestamp = new DateTime( 2022, 06, 29, 7, 26, 12, 863, DateTimeKind.Utc ),
							Attributes = new[]
							{
								new Attribute( Part.Number, 122344 ),
								new Attribute( Part.Abbreviation, "Blechteil" ),
								new Attribute( Part.Organisation, "Presswerk" )
							}
						},
					},
				},

				new SimplePartDto
				{
					Path = PathHelper.RoundtripString2PathInformation( "P:/Blechteil/" ),
					CharChangeDate = new DateTime( 2022, 01, 31, 19, 2, 58, 767, DateTimeKind.Utc ),
					Uuid = new Guid( "fe85eefe-f08d-4e78-9f06-0e3b3cc9275e" ),
					Version = 0,
					Timestamp = new DateTime( 2022, 06, 30, 6, 25, 35, 46, DateTimeKind.Utc ),
					Attributes = new[]
					{
						new Attribute( Part.Number, 122345 ),
						new Attribute( Part.Abbreviation, "Blechteil" ),
						new Attribute( Part.Organisation, "Presswerk" )
					}
				},

				null
			},

			new RawDataInformationDto
			{
				Target = new RawDataTargetEntityDto { Entity = RawDataEntityDto.Part, Uuid = "15D4BFB5-563D-45CB-B6C2-26720E89582A" },
				Key = 7,
				FileName = "Text.txt",
				MimeType = MediaTypeNames.Text.Plain,
				LastModified = new DateTime( 2022, 07, 19, 16, 9, 40, 0, DateTimeKind.Utc ),
				Created = new DateTime( 2022, 07, 19, 16, 10, 5, 0, DateTimeKind.Utc ),
				Size = 13,
				MD5 = new Guid("c35a0b60-ee3f-2524-e89f-e2ea9fd6cb50")
			},

			new RawDataDto
			{
				Target = new RawDataTargetEntityDto { Entity = RawDataEntityDto.Part, Uuid = "15D4BFB5-563D-45CB-B6C2-26720E89582A" },
				Key = 7,
				FileName = "Text.txt",
				MimeType = MediaTypeNames.Text.Plain,
				LastModified = new DateTime( 2022, 07, 19, 16, 9, 40, 0, DateTimeKind.Utc ),
				Created = new DateTime( 2022, 07, 19, 16, 10, 5, 0, DateTimeKind.Utc ),
				Size = 13,
				MD5 = new Guid("c35a0b60-ee3f-2524-e89f-e2ea9fd6cb50"),
				Data = Encoding.ASCII.GetBytes("Hello, PiWeb!"),
			},

			new RawData.ServiceInformationDto
			{
				Version = "8.4.1.0",
				RequestHeaderSize = 0
			},
		};

		private static readonly IReadOnlyDictionary<Type, Func<object, IStructuralEquatable>> EquatableFromType = new[]
		{
			EquatableFrom<AttributeDefinitionDto>( value => Tuple.Create( value.Key, value.Description, value.Length, value.QueryEfficient, value.Type ) ),

			EquatableFrom<CatalogAttributeDefinitionDto>( value => Tuple.Create( value.Key, value.Description, value.QueryEfficient, value.Catalog ) ),

			EquatableFrom<AbstractAttributeDefinitionDto>( value => EquatableFromType[value is AttributeDefinitionDto ? typeof( AttributeDefinitionDto ) : typeof( CatalogAttributeDefinitionDto )]( value ) ),

			EquatableFrom<Attribute> (value => Tuple.Create( value.Key, EquatableFromAttributeValue(value.Value) ) ),

			EquatableFrom<CatalogDto>( value => Tuple.Create( value.Name, value.Uuid, value.ValidAttributes.ToArray(), EquatableFromMany( value.CatalogEntries ) ) ),

			EquatableFrom<CatalogEntryDto>( value => Tuple.Create( value.Key, EquatableFromMany( value.Attributes ) ) ),

			EquatableFrom<ConfigurationDto>( value => Tuple.Create( EquatableFromMany( value.PartAttributes ),
																	EquatableFromMany( value.CharacteristicAttributes ),
																	EquatableFromMany( value.MeasurementAttributes ),
																	EquatableFromMany( value.ValueAttributes ),
																	EquatableFromMany( value.CatalogAttributes ),
																	value.VersioningType ) ),

			EquatableFrom<KeyValuePair<Guid, DataValueDto>>( value => Tuple.Create( value.Key, EquatableFromMany( value.Value.Attributes ) ) ),

			EquatableFrom<DataValueDto>( value => EquatableFromMany( value.Attributes ) ),

			EquatableFrom<SimpleMeasurementStatusDto>( value => Tuple.Create( value.Id, value.Count, value.Uuid?.ToArray() ) ),

			EquatableFrom<SimpleMeasurementDto>( value => Tuple.Create( value.Uuid, value.PartUuid, value.LastModified, value.Created, EquatableFromMany( value.Status ), EquatableFromMany( value.Attributes ) ) ),

			EquatableFrom<DataMeasurementDto>( value => Tuple.Create( value.Uuid, value.PartUuid, value.LastModified, value.Created, EquatableFromMany( value.Attributes ), EquatableFromMany( value.Characteristics ) ) ),

			EquatableFrom<InspectionPlanCharacteristicDto>( value => Tuple.Create( value.Path, value.Uuid, value.Version, value.Timestamp, EquatableFromMany( value.Attributes ) ) ),

			EquatableFrom<InspectionPlanPartDto>( value => Tuple.Create( value.Path, value.Uuid, value.Version, value.Timestamp, value.CharChangeDate,
															EquatableFromMany( value.Attributes ), value.History is not null ?  EquatableFromMany( value.History ) : null  ) ),

			EquatableFrom<SimplePartDto>( value => Tuple.Create( value.Path, value.Uuid, value.Version, value.Timestamp, value.CharChangeDate, EquatableFromMany( value.Attributes ) ) ),

			EquatableFrom<OperationStatusDto>( value => Tuple.Create( value.OperationUuid, value.ExecutionStatus, value.Exception?.ExceptionMessage ?? string.Empty ) ),

			EquatableFrom<OrderDto>( value => Tuple.Create( value.Entity, value.Attribute, value.Direction ) ),

			EquatableFrom<Dtos.Data.ServiceInformationDto>( value => Tuple.Create( value.EditionSpecified, value.Edition, value.ServerName, value.SecurityEnabled, value.FeatureList.ToArray(), value.RequestHeaderSize,
															Tuple.Create( value.PartCount, value.CharacteristicCount, value.MeasurementCount, value.ValueCount ),
															Tuple.Create( value.InspectionPlanTimestamp, value.MeasurementTimestamp, value.ConfigurationTimestamp, value.CatalogTimestamp, value.StructureTimestamp )) ),

			EquatableFrom<InterfaceVersionRange>( value => value.SupportedVersions.ToArray() ),

			EquatableFrom<InspectionPlanDtoBase[]>( value => EquatableFromMany( value ) ),

			EquatableFrom<RawDataTargetEntityDto>( value => Tuple.Create( value.Entity, value.Uuid ) ),

			EquatableFrom<RawDataInformationDto>( value => Tuple.Create( EquatableFromType[typeof( RawDataTargetEntityDto )]( value.Target ), value.Key, value.FileName, value.MimeType, value.LastModified, value.Created, value.Size, value.MD5 ) ),

			EquatableFrom<RawDataDto>( value => Tuple.Create( EquatableFromType[typeof( RawDataInformationDto )]( value ), value.Data ) ),

			EquatableFrom<RawData.ServiceInformationDto>( value => Tuple.Create( value.Version, value.RequestHeaderSize ) ),
		}
		.ToDictionary( pair => pair.Key, pair => pair.Value );

		private static readonly Newtonsoft.Json.JsonSerializerSettings Settings = new()
		{
			NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
			Converters =
			{
				new Newtonsoft.Json.Converters.VersionConverter(),
				new InspectionPlanDtoBaseConverter(),
				new AttributeConverter()
			}
		};

		private static readonly System.Text.Json.JsonSerializerOptions Options = new()
		{
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			Converters =
			{
				new JsonInspectionPlanDtoBaseConverter(),
				new JsonAttributeConverter(),
			}
		};

		#endregion

		#region methods

		[TestCaseSource( nameof( TestCases ) )]
		public void Backward_Compatible<T>( T value )
		{
			var json = Newtonsoft.Json.JsonConvert.SerializeObject( value, Settings );

			var deserializedValue = System.Text.Json.JsonSerializer.Deserialize<T>( json, Options );

			var equatable = EquatableFromType[typeof( T )];

			var expected = equatable( value );
			var actual = equatable( deserializedValue );

			Assert.AreEqual( expected, actual, $"{typeof( T ).Name}" );
		}

		[TestCaseSource( nameof( TestCases ) )]
		public void Forward_Compatible<T>( T value )
		{
			var json = System.Text.Json.JsonSerializer.Serialize( value, Options );

			var deserializedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<T>( json, Settings );

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
			return values.Where( value => value != null ).Select( value => EquatableFromType[value.GetType()]( value ) ).ToArray();
		}

		private static string EquatableFromAttributeValue( string value )
		{
			if( double.TryParse( value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue ) )
			{
				return doubleValue.ToString( "G17", CultureInfo.InvariantCulture );
			}

			return value;
		}

		#endregion
	}
}