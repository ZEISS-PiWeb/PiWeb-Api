namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Benchmarks
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Text.Json;
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

		private static readonly IReadOnlyList<InspectionPlanCharacteristicDto> Characteristics = JsonSerializer.Deserialize<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		private static readonly IReadOnlyList<SimpleMeasurementDto> Measurements = JsonSerializer.Deserialize<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
		private static readonly IReadOnlyList<DataMeasurementDto> Values = JsonSerializer.Deserialize<IReadOnlyList<DataMeasurementDto>>( ValuesJson );

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
		[BenchmarkCategory( SerializationCategory, InspectionPlanCategory, SystemTextJsonCategory )]
		public string SerializeCharacteristics()
		{
			return JsonSerializer.Serialize( Characteristics );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, MeasurementCategory, SystemTextJsonCategory )]
		public string SerializeMeasurements()
		{
			return JsonSerializer.Serialize( Measurements );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, ValueCategory, SystemTextJsonCategory )]
		public string SerializeValues()
		{
			return JsonSerializer.Serialize( Values );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, InspectionPlanCategory, SystemTextJsonCategory )]
		public IReadOnlyList<InspectionPlanCharacteristicDto> DeserializeCharacteristics()
		{
			return JsonSerializer.Deserialize<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, MeasurementCategory, SystemTextJsonCategory )]
		public IReadOnlyList<SimpleMeasurementDto> DeserializeMeasurements()
		{
			return JsonSerializer.Deserialize<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, ValueCategory, SystemTextJsonCategory )]
		public IReadOnlyList<DataMeasurementDto> DeserializeValues()
		{
			return JsonSerializer.Deserialize<IReadOnlyList<DataMeasurementDto>>( ValuesJson );
		}

		#endregion
	}
}
