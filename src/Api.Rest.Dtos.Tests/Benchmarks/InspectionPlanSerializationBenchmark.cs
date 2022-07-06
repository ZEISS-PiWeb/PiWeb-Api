namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Benchmarks
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using BenchmarkDotNet.Attributes;
	using BenchmarkDotNet.Configs;
	using BenchmarkDotNet.Running;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion


	/// <summary>
	/// This class contains various benchmarks for testing the inspection plan serialization/deseialization.
	/// </summary>
	/// <remarks>
	/// Please note: Run these benchmarks in Release mode for reliable and realistic performance numbers.
	/// </remarks>
	[TestFixture]
	[SuppressMessage( "Performance", "CA1822:Member als statisch markieren" )]
	[MemoryDiagnoser]
	public class InspectionPlanSerializationBenchmark
	{
		#region members

		private static readonly string CharacteristicJson = ReadResourceString( "Samples.characteristics.json" );
		private static readonly IReadOnlyList<InspectionPlanCharacteristicDto> Characteristics = JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );

		#endregion

		#region methods

		[Test, Explicit]
		public void RunBenchmarks()
		{
			var config = new DebugInProcessConfig();
			var summary = BenchmarkRunner.Run<InspectionPlanSerializationBenchmark>( config );
			Console.Out.WriteLine( summary );
		}

		[Benchmark]
		public string SerializeCharacteristics()
		{
			return JsonConvert.SerializeObject( Characteristics );
		}

		[Benchmark]
		public IReadOnlyList<InspectionPlanCharacteristicDto> DeserializeCharacteristics()
		{
			return JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
		}

		private static string ReadResourceString( string resourceName )
		{
			using var stream = typeof( InspectionPlanSerializationBenchmark ).Assembly.GetManifestResourceStream( "Zeiss.PiWeb.Api.Rest.Dtos.Tests." + resourceName );
			using var reader = new StreamReader( stream );

			return reader.ReadToEnd();
		}

		#endregion
	}
}