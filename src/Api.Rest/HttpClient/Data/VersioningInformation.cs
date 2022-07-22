#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Data
{
	#region usings

	using System;
	using System.Text.Json.Serialization;

	#endregion

	/// <remarks/>
	public class VersioningInformation
	{
		#region properties

		/// <summary>
		/// Gets or sets the id of this version entry.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "id" )]
		[JsonPropertyName( "id" )]
		public int ID { get; set; }

		/// <summary>
		/// Gets or sets the versioning comment.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "comment" )]
		[JsonPropertyName( "comment" )]
		public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the comment of this inspection plan entity.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "user" )]
		[JsonPropertyName( "user" )]
		public string User { get; set; }

		/// <summary>
		/// Gets or sets the number of changed inspection plan items that where change.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "changeCount" )]
		[JsonPropertyName( "changeCount" )]
		public int ChangeCount { get; set; }

		/// <summary>
		/// Contains the date and time of the versioning change.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "dateCreated" )]
		[JsonPropertyName( "dateCreated" )]
		public DateTime DateCreated { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return DateCreated + " - " + Comment;
		}

		#endregion
	}
}