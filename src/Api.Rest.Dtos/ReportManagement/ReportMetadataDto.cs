#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.ReportManagement
{

	#region usings

	using System;
	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Contains the report metadata.
	/// </summary>
	public sealed class ReportMetadataDto
	{
		#region properties

		/// <summary>
		/// Gets the unique identifier of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "uuid" )]
		[JsonPropertyName( "uuid" )]
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets the unique identifier of the report directory.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "directoryUuid" )]
		[JsonPropertyName( "directoryUuid" )]
		public Guid DirectoryUuid { get; set; }

		/// <summary>
		/// Gets the file name of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "fileName" )]
		[JsonPropertyName( "fileName" )]
		public string FileName { get; set; }

		/// <summary>
		/// Gets the user UUID of the user who  setially uploaded the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "creator" )]
		[JsonPropertyName( "creator" )]
		public Guid Creator { get; set; }

		/// <summary>
		/// Gets the creation time (UTC) of the report metadata, which is the  setial upload time of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "created" )]
		[JsonPropertyName( "created" )]
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets the user UUID of the user who uploaded the report the last time.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "lastModifier" )]
		[JsonPropertyName( "lastModifier" )]
		public Guid LastModifier { get; set; }

		/// <summary>
		///Gets the last modification time (UTC) of the report metadata.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "lastModified" )]
		[JsonPropertyName( "lastModified" )]
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets the user UUID of the user who deleted the report or <see langword="null"/> if the report is not deleted.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "deleter" )]
		[JsonPropertyName( "deleter" )]
		public Guid? Deleter { get; set; }

		/// <summary>
		/// Gets the deletion time (UTC) of the report metadata or <see langword="null"/> if the report is not deleted.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "deleted" )]
		[JsonPropertyName( "deleted" )]
		public DateTime? Deleted { get; set; }

		/// <summary>
		/// Gets the file size of the report in bytes.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "size" )]
		[JsonPropertyName( "size" )]
		public long Size { get; set; }

		/// <summary>
		/// Gets the checksum of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "mD5" )]
		[JsonPropertyName( "mD5" )]
		public Guid? MD5 { get; set; }

		/// <summary>
		/// Gets the version of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "version" )]
		[JsonPropertyName( "version" )]
		public Version Version { get; set; }

		/// <summary>
		/// Gets the display name of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "displayName" )]
		[JsonPropertyName( "displayName" )]
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets the description of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "description" )]
		[JsonPropertyName( "description" )]
		public string Description { get; set; }

		/// <summary>
		/// Gets the group of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "reportGroup" )]
		[JsonPropertyName( "reportGroup" )]
		public string ReportGroup { get; set; }

		/// <summary>
		/// Gets the creator of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "reportCreator" )]
		[JsonPropertyName( "reportCreator" )]
		public string ReportCreator { get; set; }

		/// <summary>
		/// Gets the creation time of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "reportCreated" )]
		[JsonPropertyName( "reportCreated" )]
		public DateTime ReportCreated { get; set; }

		/// <summary>
		/// Gets the last modifier of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "reportLastModifier" )]
		[JsonPropertyName( "reportLastModifier" )]
		public string ReportLastModifier { get; set; }

		/// <summary>
		/// Gets the last modification time of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "reportLastModified" )]
		[JsonPropertyName( "reportLastModified" )]
		public DateTime ReportLastModified { get; set; }

		/// <summary>
		/// Gets the links to further endpoints of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "links" )]
		[JsonPropertyName( "links" )]
		public ReportMetadataLinksDto Links { get; set; }

		#endregion
	}
}