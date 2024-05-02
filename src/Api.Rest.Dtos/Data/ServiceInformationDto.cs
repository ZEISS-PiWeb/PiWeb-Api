#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// This class contains general information about the DataService like the server name, server version etc.
	/// </summary>
	public class ServiceInformationDto
	{
		#region properties

		/// <summary>
		/// Gets or sets of the <see cref="Edition"/> contains a values.
		/// </summary>
		public bool EditionSpecified { get; set; }

		/// <summary>
		/// Gets or sets the server name.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "serverName" )]
		[JsonPropertyName( "serverName" )]
		public string ServerName { get; set; }

		/// <summary>
		/// Gets or sets the server version.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "version" )]
		[JsonPropertyName( "version" )]
		public string Version { get; set; }

		/// <summary>
		/// Gets or sets the extended server version.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "extendedVersion" )]
		[JsonPropertyName( "extendedVersion" )]
		public string ExtendedVersion { get; set; }

		/// <summary>
		/// Gets or sets whether the server has security enabled.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "securityEnabled" )]
		[JsonPropertyName( "securityEnabled" )]
		public bool SecurityEnabled { get; set; }

		/// <summary>
		/// Gets or sets the servers edition.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "edition" )]
		[JsonPropertyName( "edition" )]
		public string Edition { get; set; }

		/// <summary>
		/// Gets or sets the number of parts that currently exist in the server. This number is just an approximation.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "partCount" )]
		[JsonPropertyName( "partCount" )]
		public int PartCount { get; set; }

		/// <summary>
		/// Gets or sets the number of characteristics that currently exist in the server. This number is just an approximation.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "characteristicCount" )]
		[JsonPropertyName( "characteristicCount" )]
		public int CharacteristicCount { get; set; }

		/// <summary>
		/// Gets or sets the number of measurements that currently exist in the server. This number is just an approximation.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "measurementCount" )]
		[JsonPropertyName( "measurementCount" )]
		public int MeasurementCount { get; set; }

		/// <summary>
		/// Gets or sets the number of values that currently exist in the server. This number is just an approximation.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "valueCount" )]
		[JsonPropertyName( "valueCount" )]
		public int ValueCount { get; set; }

		/// <summary>
		/// Gets or sets a list of features that are supported by the server.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "featureList" )]
		[JsonPropertyName( "featureList" )]
		public IReadOnlyCollection<string> FeatureList { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the last inspection plan modification accross the whole server.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "inspectionPlanTimestamp" )]
		[JsonPropertyName( "inspectionPlanTimestamp" )]
		public DateTime? InspectionPlanTimestamp { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the last structure modification accross the whole server.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "structureTimestamp" )]
		[JsonPropertyName( "structureTimestamp" )]
		public DateTime? StructureTimestamp { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the last measurement modification accross the whole server.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "measurementTimestamp" )]
		[JsonPropertyName( "measurementTimestamp" )]
		public DateTime? MeasurementTimestamp { get; set; }

		/// <summary>
		/// Gets or sets the timestamp for the last modification of the server configuration.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "configurationTimestamp" )]
		[JsonPropertyName( "configurationTimestamp" )]
		public DateTime? ConfigurationTimestamp { get; set; }

		/// <summary>
		/// Gets or sets the timestamp for the last modification of the catalogs.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "catalogTimestamp" )]
		[JsonPropertyName( "catalogTimestamp" )]
		public DateTime? CatalogTimestamp { get; set; }

		[Newtonsoft.Json.JsonProperty( "requestHeaderSize" )]
		[JsonPropertyName( "requestHeaderSize" )]
		public int RequestHeaderSize { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			if( Version == null )
				return string.Empty;

			return EditionSpecified ? $"{Edition} Edition" : $"QDB, Version {Version}";
		}

		#endregion
	}
}