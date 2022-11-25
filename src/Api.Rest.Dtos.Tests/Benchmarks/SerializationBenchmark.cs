namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Benchmarks
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using BenchmarkDotNet.Attributes;
	using BenchmarkDotNet.Configs;
	using BenchmarkDotNet.Filters;
	using BenchmarkDotNet.Running;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data;

	#endregion

	/// <summary>
	/// This class contains various benchmarks for testing the inspection plan serialization/deseialization.
	/// </summary>
	/// <remarks>
	/// Please note: Run these benchmarks in Release mode for reliable and realistic performance numbers.
	/// </remarks>
	[TestFixture]
	[SuppressMessage( "Performance", "CA1822:Member als statisch markieren" )]
	[MemoryDiagnoser, CategoriesColumn]
	public class SerializationBenchmark
	{
		#region constants

		private const string InspectionPlanCategory = "InspectionPlan";
		private const string MeasurementCategory = "Measurements";
		private const string ValueCategory = "Values";

		private const string SerializationCategory = "Serialization";
		private const string DeserializationCategory = "Deserialization";

		private const string NewtonsoftJsonCategory = "NewtonsoftJson";
		private const string SystemTextJsonCategory = "SystemTextJson";

		#endregion

		#region members

		private static readonly string CharacteristicJson = SerializationTestHelper.ReadResourceString( "Samples.characteristics.json" );
		private static readonly string MeasurementsJson = SerializationTestHelper.ReadResourceString( "Samples.measurements.json" );
		private static readonly string ValuesJson = SerializationTestHelper.ReadResourceString( "Samples.values.json" );

		private static readonly IReadOnlyList<InspectionPlanCharacteristicDto> Characteristics = Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		private static readonly IReadOnlyList<SimpleMeasurementDto> Measurements = Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
		private static readonly IReadOnlyList<DataMeasurementDto> Values = Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<DataMeasurementDto>>( ValuesJson );

		private static readonly IReadOnlyList<string[]> TestCases = new[]
		{
			new[] { InspectionPlanCategory },
			new[] { MeasurementCategory },
			new[] { ValueCategory },
			new[] { SerializationCategory },
			new[] { DeserializationCategory },

			new[] { NewtonsoftJsonCategory, InspectionPlanCategory },
			new[] { NewtonsoftJsonCategory, MeasurementCategory },
			new[] { NewtonsoftJsonCategory, ValueCategory },
			new[] { NewtonsoftJsonCategory, SerializationCategory },
			new[] { NewtonsoftJsonCategory, DeserializationCategory },

			new[] { SystemTextJsonCategory, InspectionPlanCategory },
			new[] { SystemTextJsonCategory, MeasurementCategory },
			new[] { SystemTextJsonCategory, ValueCategory },
			new[] { SystemTextJsonCategory, SerializationCategory },
			new[] { SystemTextJsonCategory, DeserializationCategory },
		};

		#endregion

		#region methods

		[Test, Explicit]
		public void RunAllBenchmarks()
		{
			ExecuteBenchmarks();
		}

		[TestCaseSource( nameof( TestCases ) ), Explicit]
		public void RunBenchmarks( string[] categories )
		{
			ExecuteBenchmarks( categories );
		}

		private static void ExecuteBenchmarks( params string[] categories )
		{
			var config = (IConfig)new DebugInProcessConfig();
			if( categories != null && categories.Length > 0 )
				config = config.AddFilter( new AllCategoriesFilter( categories ) );

			var summary = BenchmarkRunner.Run<SerializationBenchmark>( config );

			Console.Out.WriteLine( summary );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, InspectionPlanCategory, NewtonsoftJsonCategory )]
		public string SerializeCharacteristicsNewtonsoftJson()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject( Characteristics );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, InspectionPlanCategory, SystemTextJsonCategory )]
		public string SerializeCharacteristicsSystemTextJson()
		{
			return System.Text.Json.JsonSerializer.Serialize( Characteristics );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, MeasurementCategory, NewtonsoftJsonCategory )]
		public string SerializeMeasurementsNewtonsoftJson()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject( Measurements );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, MeasurementCategory, SystemTextJsonCategory )]
		public string SerializeMeasurementsSystemTextJson()
		{
			return System.Text.Json.JsonSerializer.Serialize( Measurements );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, ValueCategory, NewtonsoftJsonCategory )]
		public string SerializeValuesNewtonsoftJson()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject( Values );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, ValueCategory, SystemTextJsonCategory )]
		public string SerializeValuesSystemTextJson()
		{
			return System.Text.Json.JsonSerializer.Serialize( Values );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, InspectionPlanCategory, NewtonsoftJsonCategory )]
		public IReadOnlyList<InspectionPlanCharacteristicDto> DeserializeCharacteristicsNewtonsoftJson()
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, InspectionPlanCategory, SystemTextJsonCategory )]
		public IReadOnlyList<InspectionPlanCharacteristicDto> DeserializeCharacteristicsSystemTextJson()
		{
			return System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, MeasurementCategory, NewtonsoftJsonCategory )]
		public IReadOnlyList<SimpleMeasurementDto> DeserializeMeasurementsNewtonsoftJson()
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, MeasurementCategory, SystemTextJsonCategory )]
		public IReadOnlyList<SimpleMeasurementDto> DeserializeMeasurementsSystemTextJson()
		{
			return System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, ValueCategory, NewtonsoftJsonCategory )]
		public IReadOnlyList<DataMeasurementDto> DeserializeValuesNewtonsoftJson()
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<DataMeasurementDto>>( ValuesJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, ValueCategory, SystemTextJsonCategory )]
		public IReadOnlyList<DataMeasurementDto> DeserializeValuesSystemTextJson()
		{
			return System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<DataMeasurementDto>>( ValuesJson );
		}

		#endregion
	}
}
