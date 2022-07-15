#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converters
{
	#region usings

	using System;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="PathInformationDto"/>-objects.
	/// </summary>
	public sealed class PathInformationJsonConverter : JsonConverter<PathInformationDto>
	{
		#region methods

		/// <inheritdoc />
		public override PathInformationDto Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			if( reader.TokenType != JsonTokenType.String )
				return null;

			return PathHelper.RoundtripString2PathInformation( reader.GetString() );

		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, PathInformationDto value, JsonSerializerOptions options )
		{
			writer.WriteStringValue( PathHelper.PathInformation2RoundtripString( value ) );
		}

		#endregion
	}
}