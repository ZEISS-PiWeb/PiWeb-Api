#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.CompilerServices;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// Provides a set of static extensions to <see cref="IObjectSerializer"/>.
	/// </summary>
	public static class ObjectSerializer
	{
		#region members

		/// <summary>
		/// Default de-/serialization
		/// </summary>
		public static readonly IObjectSerializer Default = new SystemTextJsonSerializer();

		#endregion

		#region class SystemTextJsonSerializer

		private sealed class SystemTextJsonSerializer : IObjectSerializer
		{
			#region members

			private static readonly JsonSerializerOptions Options = new()
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

				WriteIndented = false,

				Converters =
				{
					new InspectionPlanDtoBaseConverter()
				}
			};

			#endregion

			#region interface IObjectSerializer

			Task IObjectSerializer.SerializeAsync<T>( Stream stream, T value, CancellationToken cancellationToken )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				async Task SerializeAsync()
				{
					try
					{
						await JsonSerializer.SerializeAsync( stream, value, Options, cancellationToken ).ConfigureAwait( false );
					}
					catch( JsonException exception )
					{
						throw new ObjectSerializerException( $"Serializing {typeof( T ).Name}", exception );
					}
					finally
					{
						stream.Close();
					}
				}

				return SerializeAsync();
			}

			Task<T> IObjectSerializer.DeserializeAsync<T>( Stream stream, CancellationToken cancellationToken )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				async Task<T> DeserializeAsync()
				{
					try
					{
						return await JsonSerializer.DeserializeAsync<T>( stream, Options, cancellationToken ).ConfigureAwait( false );
					}
					catch( JsonException exception )
					{
						throw new ObjectSerializerException( $"Deserializing {typeof( T ).Name}", exception );
					}
				}

				return DeserializeAsync();
			}

			IAsyncEnumerable<T> IObjectSerializer.DeserializeAsyncEnumerable<T>( Stream stream, CancellationToken cancellationToken )
			{
				if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

				async IAsyncEnumerable<T> DeserializeAsyncEnumerable( [EnumeratorCancellation] CancellationToken cancellationToken )
				{
					var values = JsonSerializer.DeserializeAsyncEnumerable<T>( stream, Options, cancellationToken );

					var enumerator = values.GetAsyncEnumerator( cancellationToken );

					await using( enumerator.ConfigureAwait( false ) )
					{
						for( var moveNext = true; moveNext; )
						{
							try
							{
								moveNext = await enumerator.MoveNextAsync().ConfigureAwait( false );
							}
							catch( JsonException exception )
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

				return DeserializeAsyncEnumerable( cancellationToken );
			}

			#endregion
		}

		#endregion
	}
}