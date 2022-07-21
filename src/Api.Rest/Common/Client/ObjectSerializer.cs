#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters;

	/// <summary>
	/// Provides a set of static extensions to <see cref="IObjectSerializer"/>.
	/// </summary>
	public static class ObjectSerializer
	{
		#region members

		/// <summary>
		/// Newtonsoft.Json de-/serialization
		/// </summary>
		public static readonly IObjectSerializer NewtonsoftJson = new NewtonsoftJsonSerializer();

		/// <summary>
		/// System.Text.Json de-/serialization
		/// </summary>
		public static readonly IObjectSerializer SystemTextJson = new SystemTextJsonSerializer();

		/// <summary>
		/// Default de-/serialization
		/// </summary>
		public static readonly IObjectSerializer Default = NewtonsoftJson;

		#endregion

		#region methods

		internal static IEnumerable<T> DeserializeEnumeratedObject<T>( [NotNull] this IObjectSerializer serializer, [NotNull] Stream data )
		{
			if( serializer == null ) throw new ArgumentNullException( nameof( serializer ) );
			if( data == null ) throw new ArgumentNullException( nameof( data ) );

			IEnumerable<T> DeserializeEnumeratedObject()
			{
				var result = serializer.Deserialize<IEnumerable<T>>( data );

				if( result == null ) yield break;

				foreach( var entity in result )
					yield return entity;
			}

			return DeserializeEnumeratedObject();
		}

		#endregion

		#region NewtonsoftJsonSerializer

		private sealed class NewtonsoftJsonSerializer : IObjectSerializer
		{
			#region methods

			private static Newtonsoft.Json.JsonSerializer CreateJsonSerializer()
			{
				return new Newtonsoft.Json.JsonSerializer
				{
					Formatting = Newtonsoft.Json.Formatting.None,

					DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
					NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,

					Converters =
					{
						new Newtonsoft.Json.Converters.VersionConverter(),
						new InspectionPlanDtoBaseConverter()
					}
				};
			}

			#endregion

			#region interface ISerializer

			void IObjectSerializer.Serialize<T>( Stream stream, T value )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				try
				{
					using var streamWriter = new StreamWriter( stream, Encoding.UTF8, 64 * 1024, false );

					var jsonSerializer = CreateJsonSerializer();

					jsonSerializer.Serialize( streamWriter, value );
				}
				catch( Newtonsoft.Json.JsonException exception )
				{
					throw new ObjectSerializerException( $"Serializing {typeof( T ).Name}", exception );
				}
			}

			T IObjectSerializer.Deserialize<T>( Stream stream )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				try
				{
					using var reader = new Newtonsoft.Json.JsonTextReader( new StreamReader( stream, Encoding.UTF8, true, 4096, true ) ) { CloseInput = false };

					var jsonSerializer = CreateJsonSerializer();

					return jsonSerializer.Deserialize<T>( reader );
				}
				catch( Newtonsoft.Json.JsonException exception )
				{
					throw new ObjectSerializerException( $"Deserializing {typeof( T ).Name}", exception );
				}
			}

			#endregion
		}

		#endregion

		#region SystemTextJsonSerializer

		private sealed class SystemTextJsonSerializer : IObjectSerializer
		{
			#region members

			private static readonly System.Text.Json.JsonSerializerOptions Options = new()
			{
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,

				WriteIndented = false,

				Converters =
				{
					new JsonInspectionPlanDtoBaseConverter()
				}
			};

			#endregion

			#region interface ISerializer

			void IObjectSerializer.Serialize<T>( Stream stream, T value )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				try
				{
					System.Text.Json.JsonSerializer.Serialize( stream, value, Options );
				}
				catch( System.Text.Json.JsonException exception )
				{
					throw new ObjectSerializerException( $"Serializing {typeof( T ).Name}", exception );
				}
			}

			T IObjectSerializer.Deserialize<T>( Stream stream )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				try
				{
					return System.Text.Json.JsonSerializer.Deserialize<T>( stream, Options );
				}
				catch( System.Text.Json.JsonException exception )
				{
					throw new ObjectSerializerException( $"Deserializing {typeof( T ).Name}", exception );
				}
			}

			#endregion
		}

		#endregion
	}
}