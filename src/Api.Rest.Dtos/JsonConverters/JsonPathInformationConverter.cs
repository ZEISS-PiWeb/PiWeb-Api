#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters
{
	#region usings

	using System;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Core;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="PathInformation"/>-objects.
	/// </summary>
	public sealed class JsonPathInformationConverter : JsonConverter<PathInformation>
	{
		#region methods

		/// <inheritdoc />
		public override PathInformation Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			if( reader.TokenType != JsonTokenType.String )
				return null;

			return PathHelper.RoundtripString2PathInformation( reader.GetString() );

		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, PathInformation value, JsonSerializerOptions options )
		{
			writer.WriteStringValue( PathHelper.PathInformation2RoundtripString( value ) );
		}

		#endregion
	}
}