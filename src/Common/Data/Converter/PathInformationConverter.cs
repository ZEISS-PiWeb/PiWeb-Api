#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data.Converter
{
	#region using

	using System;
	using System.Globalization;
	using Newtonsoft.Json;
	using DataService;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="DataService.PathInformation"/>-objects.
	/// </summary>
	public class PathInformationConverter : JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( PathInformation ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( reader.TokenType == JsonToken.String )
			{
				var value = (string)reader.Value;
				if( string.Equals( value, "/", StringComparison.Ordinal ) )
					return PathInformation.Root;

				var index = value.IndexOf( ':' );

				if( index < 1 )
					throw new ArgumentException( "The path need to have the following structure:\"structure:path\", e.g.: \"PC:part/characteristic\"" );

				var structure = value.Substring( 0, index );
				var path = value.Substring( index + 1 );

				return PathHelper.String2PathInformation( path, structure );
			}
			return null;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			var path = (PathInformation)value;
			if( path.IsRoot )
			{
				writer.WriteValue( "/" );
			}
			else
			{
				var pathText = new System.Text.StringBuilder()
					.Append( PathHelper.GetStructure( path ) )
					.Append( ":" )
					.Append( PathHelper.PathInformation2String( path ) )
					.ToString();
				writer.WriteValue( pathText );
			}
		}

		#endregion
	}
}