#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Xml;

	#endregion

	/// <summary>
	/// Extension class with various helper methods for attribute list manipulation.
	/// </summary>
	public static class AttributeItemExtensions
	{
		#region methods

		/// <summary>
		/// Returns the attribute with the key <code>key</code>.
		/// </summary>
		public static AttributeDto? GetAttribute( this IAttributeItemDto item, ushort key )
		{
			return GetAttribute( item, key, out _ );
		}

		/// <summary>
		/// Returns the attribute with the key <code>key</code>.
		/// </summary>
		public static AttributeDto? GetAttribute( this IAttributeItemDto item, ushort key, out int index )
		{
			var attributes = item?.Attributes;
			if( attributes != null )
			{
				for( index = 0; index < attributes.Count; index++ )
				{
					if( attributes[ index ].Key == key )
						return attributes[ index ];
				}
			}

			index = -1;

			return null;
		}

		/// <summary>
		/// Returns the value for key <code>key</code>.
		/// </summary>
		public static string GetAttributeValue( this IAttributeItemDto item, ushort key )
		{
			return GetAttribute( item, key )?.Value;
		}

		/// <summary>
		/// Returns the value for key <code>key</code> as a <see cref="double"/>.
		/// </summary>
		/// <remarks>
		/// If the attribute is either empty, does not exist or parsing fails <code>null</code> is returned.
		/// </remarks>
		public static double? GetDoubleAttributeValue( this IAttributeItemDto item, ushort key )
		{
			return GetAttribute( item, key )?.GetDoubleValue();
		}

		/// <summary>
		/// Returns the value for key <code>key</code> as a <see cref="int"/>.
		/// </summary>
		/// <remarks>
		/// If the attribute is either empty, does not exist or parsing fails <code>null</code> is returned.
		/// </remarks>
		public static int? GetIntAttributeValue( this IAttributeItemDto item, ushort key )
		{
			return GetAttribute( item, key )?.GetIntValue();
		}

		/// <summary>
		/// Returns the value for key <code>key</code> as a <see cref="DateTime"/>.
		/// </summary>
		/// <remarks>
		/// If the attribute is either empty, does not exist or parsing fails <code>null</code> is returned.
		/// </remarks>
		public static DateTime? GetDateAttributeValue( this IAttributeItemDto item, ushort key )
		{
			return GetAttribute( item, key )?.GetDateValue();
		}

		/// <summary>
		/// Returns the attribute's value of the attribute with the key <paramref name="key"/> If the attribute consists of a catalog entry the entry
		/// is returned, otherwise the attribute's value (string, int, double or DateTime) is returned.
		/// </summary>
		public static object GetRawAttributeValue(
			this IAttributeItemDto item,
			ushort key,
			ConfigurationDto configuration,
			CatalogCollectionDto catalogs )
		{
			var attribute = item.GetAttribute( key );
			if( attribute == null )
				return null;

			if( attribute.Value.RawValue != null )
				return attribute.Value.RawValue;

			return configuration.ParseValue( key, attribute.Value.Value, catalogs );
		}

		/// <summary>
		/// Inserts or replaces the attribute for the given key with an attribute that is interpreted as deletion
		/// by merge models (effectively deleting the corresponding base attribute from existance)
		/// </summary>
		public static void InsertNullAttribute( this IAttributeItemDto item, ushort key )
		{
			InternalSetAttribute( item, new AttributeDto( key, null ) );
		}

		/// <summary>
		/// Sets the <code>value</code> for the attribute with the key <code>key</code>.
		/// </summary>
		public static void SetAttributeValue( this IAttributeItemDto item, ushort key, string value )
		{
			item.SetAttribute( new AttributeDto( key, value ) );
		}

		/// <summary>
		/// Sets the <code>value</code> for the attribute with the key <code>key</code>.
		/// </summary>
		public static void SetAttribute( this IAttributeItemDto item, AttributeDto? value )
		{
			if( value == null )
				return;

			if( string.IsNullOrEmpty( value.Value.Value ) )
			{
				RemoveAttribute( item, value.Value.Key );
			}
			else
			{
				InternalSetAttribute( item, value.Value );
			}
		}

		/// <summary>
		/// Removes the attribute with the key <code>key</code>
		/// </summary>
		public static void RemoveAttribute( this IAttributeItemDto item, ushort key )
		{
			var attribute = GetAttribute( item, key, out var index );
			if( attribute == null )
				return;

			// Remove attribute
			var sourceAttributes = item.Attributes;
			var attributes = new AttributeDto[ sourceAttributes.Count - 1 ];

			for( var i = 0; i < index; i++ )
			{
				attributes[ i ] = sourceAttributes[ i ];
			}

			var count = sourceAttributes.Count - index - 1;
			for( var i = index + 1; i < count; i++ )
			{
				attributes[ i ] = sourceAttributes[ i ];
			}

			item.Attributes = attributes;
		}

		private static void InternalSetAttribute( this IAttributeItemDto item, AttributeDto value )
		{
			var sourceAttributes = item.Attributes;

			var attribute = GetAttribute( item, value.Key, out var index );
			if( attribute == null )
			{
				// Add attribute
				if( sourceAttributes != null && sourceAttributes.Count > 0 )
				{
					var attributes = new AttributeDto[ sourceAttributes.Count + 1 ];
					for( var i = 0; i < sourceAttributes.Count; i++ )
					{
						attributes[ i ] = sourceAttributes[ i ];
					}
					attributes[ attributes.Length - 1 ] = value;

					item.Attributes = attributes;
				}
				else
				{
					item.Attributes = new[] { value };
				}
			}
			else
			{
				// Replace attribute
				var attributes = sourceAttributes.ToArray();
				attributes[ index ] = value;

				item.Attributes = attributes;
			}
		}

		#endregion
	}
}