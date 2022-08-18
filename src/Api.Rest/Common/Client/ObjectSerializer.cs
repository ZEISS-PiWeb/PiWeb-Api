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
	using System.Threading.Tasks;
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

			#region Methods

			private static T Deserialize<T>( Stream stream )
			{
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

			#region interface ISerializer

			Task IObjectSerializer.SerializeAsync<T>( Stream stream, T value )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				try
				{
					using var streamWriter = new StreamWriter( stream, Encoding.UTF8, 64 * 1024, false );

					var jsonSerializer = CreateJsonSerializer();

					jsonSerializer.Serialize( streamWriter, value );

					return Task.CompletedTask;
				}
				catch( Newtonsoft.Json.JsonException exception )
				{
					throw new ObjectSerializerException( $"Serializing {typeof( T ).Name}", exception );
				}
			}

			Task<T> IObjectSerializer.DeserializeAsync<T>( Stream stream )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				return Task.FromResult( Deserialize<T>( stream ) );
			}

			IAsyncEnumerable<T> IObjectSerializer.DeserializeAsyncEnumerable<T>( [NotNull] Stream stream )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );
								
				async IAsyncEnumerable<T> DeserializeAsyncEnumerable()
				{
					var items = Deserialize<IEnumerable<T>>( stream );

					if( items == null ) yield break;

					foreach( var item in items )
						yield return await Task.FromResult( item ).ConfigureAwait( false );
				}

				return DeserializeAsyncEnumerable();
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

			Task IObjectSerializer.SerializeAsync<T>( Stream stream, T value )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				async Task SerializeAsync()
				{
					try
					{
						await System.Text.Json.JsonSerializer.SerializeAsync( stream, value, Options ).ConfigureAwait( false );

						stream.Close();
					}
					catch( System.Text.Json.JsonException exception )
					{
						throw new ObjectSerializerException( $"Serializing {typeof( T ).Name}", exception );
					}
				}

				return SerializeAsync();
			}

			Task<T> IObjectSerializer.DeserializeAsync<T>( Stream stream )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				async Task<T> DeserializeAsync()
				{
					try
					{
						return await System.Text.Json.JsonSerializer.DeserializeAsync<T>( stream, Options ).ConfigureAwait( false );
					}
					catch( System.Text.Json.JsonException exception )
					{
						throw new ObjectSerializerException( $"Deserializing {typeof( T ).Name}", exception );
					}
				}

				return DeserializeAsync();
			}

			IAsyncEnumerable<T> IObjectSerializer.DeserializeAsyncEnumerable<T>( [NotNull] Stream stream )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				async IAsyncEnumerable<T> DeserializeAsyncEnumerable()
				{
					var values = System.Text.Json.JsonSerializer.DeserializeAsyncEnumerable<T>( stream, Options ).ConfigureAwait( false );

					await using( var enumerator = values.GetAsyncEnumerator() )
					{
						for( var moveNext = true; moveNext; )
						{
							try
							{
								moveNext = await enumerator.MoveNextAsync();
							}
							catch( System.Text.Json.JsonException exception )
							{
								throw new ObjectSerializerException( $"Deserializing enumerable of {typeof( T ).Name}", exception );
							}

							if( moveNext )
							{
								yield return enumerator.Current;
							}
						}
					}
				}

				return DeserializeAsyncEnumerable();
			}

			#endregion
		}

		#endregion
	}
}