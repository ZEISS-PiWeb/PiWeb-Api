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
	using Newtonsoft.Json;
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

		#endregion

		#region members

		private static readonly string CharacteristicJson = SerializationTestHelper.ReadResourceString( "Samples.characteristics.json" );
		private static readonly string MeasurementsJson = SerializationTestHelper.ReadResourceString( "Samples.measurements.json" );
		private static readonly string ValuesJson = SerializationTestHelper.ReadResourceString( "Samples.values.json" );

		private static readonly IReadOnlyList<InspectionPlanCharacteristicDto> Characteristics = JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		private static readonly IReadOnlyList<SimpleMeasurementDto> Measurements = JsonConvert.DeserializeObject<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
		private static readonly IReadOnlyList<DataMeasurementDto> Values = JsonConvert.DeserializeObject<IReadOnlyList<DataMeasurementDto>>( ValuesJson );

		#endregion

		#region methods

		[Test, Explicit]
		public void RunAllBenchmarks()
		{
			ExecuteBenchmarks();
		}

		[Test, Explicit]
		public void RunInspectionPlanBenchmarks()
		{
			ExecuteBenchmarks( InspectionPlanCategory );
		}

		[Test, Explicit]
		public void RunMeasurementBenchmarks()
		{
			ExecuteBenchmarks( MeasurementCategory );
		}

		[Test, Explicit]
		public void RunValueBenchmarks()
		{
			ExecuteBenchmarks( ValueCategory );
		}

		[Test, Explicit]
		public void RunDeserializationBenchmarks()
		{
			ExecuteBenchmarks( DeserializationCategory );
		}

		[Test, Explicit]
		public void RunSerializationBenchmarks()
		{
			ExecuteBenchmarks( SerializationCategory );
		}

		private static void ExecuteBenchmarks( params string[] categories )
		{
			var config = (IConfig)new DebugInProcessConfig();
			if( categories != null && categories.Length > 0 )
				config = config.AddFilter( new AnyCategoriesFilter( categories ) );

			var summary = BenchmarkRunner.Run<SerializationBenchmark>( config );

			Console.Out.WriteLine( summary );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, InspectionPlanCategory )]
		public string SerializeCharacteristics()
		{
			return JsonConvert.SerializeObject( Characteristics );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, MeasurementCategory )]
		public string SerializeMeasurements()
		{
			return JsonConvert.SerializeObject( Measurements );
		}

		[Benchmark]
		[BenchmarkCategory( SerializationCategory, ValueCategory )]
		public string SerializeValues()
		{
			return JsonConvert.SerializeObject( Values );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, InspectionPlanCategory )]
		public IReadOnlyList<InspectionPlanCharacteristicDto> DeserializeCharacteristics()
		{
			return JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, MeasurementCategory )]
		public IReadOnlyList<SimpleMeasurementDto> DeserializeMeasurements()
		{
			return JsonConvert.DeserializeObject<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
		}

		[Benchmark]
		[BenchmarkCategory( DeserializationCategory, ValueCategory )]
		public IReadOnlyList<DataMeasurementDto> DeserializeValues()
		{
			return JsonConvert.DeserializeObject<IReadOnlyList<DataMeasurementDto>>( ValuesJson );
		}

		#endregion
	}
}