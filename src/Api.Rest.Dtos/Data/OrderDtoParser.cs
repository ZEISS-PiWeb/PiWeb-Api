#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Globalization;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Parser class for an <see cref="OrderDto"/>.
	/// </summary>
	public static class OrderDtoParser
	{
		#region methods

		/// <summary>
		/// Parses the given value string into an <see cref="OrderDto"/>.
		/// </summary>
		/// <param name="value">The string to parse.</param>
		/// <param name="entityType">The type of entity.</param>
		/// <returns>An parsed <see cref="OrderDto"/>.</returns>
		/// <example>4 desc</example>
		/// <example>12 asc</example>
		public static OrderDto Parse( [NotNull] string value, EntityDto entityType )
		{
			if( string.IsNullOrEmpty( value ) ) throw new ArgumentNullException( nameof( value ) );

			var items = value.Split( ' ' );

			var key = ushort.Parse( items[ 0 ], CultureInfo.InvariantCulture );
			var direction = string.Equals( "asc", items[ 1 ], StringComparison.OrdinalIgnoreCase ) ? OrderDirectionDto.Asc : OrderDirectionDto.Desc;

			return new OrderDto( key, direction, entityType );
		}

		#endregion
	}
}