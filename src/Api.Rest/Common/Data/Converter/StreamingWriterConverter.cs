#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.Converter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for streamed writing a list or an array.
	/// </summary>
	public class StreamingWriterConverter<T> : JsonConverter
	{
		#region properties

		/// <summary>
		/// Returns <code>false</code>. No read support.
		/// </summary>
		public override bool CanRead => false;

		#endregion

		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return typeof( IEnumerable<T> ).IsAssignableFrom( objectType );
		}

		/// <inheritdoc />
		/// <remarks>Not supported right now.</remarks>
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			throw new NotImplementedException( "This converter does not support reading." );
		}

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			var items = (IEnumerable<T>)value;

			writer.WriteStartArray();
			foreach( var item in items )
			{
				serializer.Serialize( writer, item );
			}

			writer.WriteEndArray();
		}

		#endregion
	}
}