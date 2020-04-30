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
	using System.Globalization;
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
		public static AttributeDto GetAttribute( this IAttributeItemDto item, ushort key )
		{
			return GetAttribute( item, key, out _ );
		}

		/// <summary>
		/// Returns the attribute with the key <code>key</code>.
		/// </summary>
		public static AttributeDto GetAttribute( this IAttributeItemDto item, ushort key, out int index )
		{
			if( item?.Attributes != null )
			{
				for( index = 0; index < item.Attributes.Length; index++ )
				{
					if( item.Attributes[ index ].Key == key )
						return item.Attributes[ index ];
				}
			}

			index = -1;

			return null;
		}

		/// <summary>
		/// Returns the attribute's value of the attribute with the key <code>key</code>.
		/// </summary>
		public static string GetAttributeValue( this IAttributeItemDto item, ushort key )
		{
			var value = GetAttribute( item, key );
			return value?.Value;
		}

		/// <summary>
		/// Returns the attribute's value of the attribute with the key <see cref="double"/> as <code>double</code>.
		/// </summary>
		/// <remarks>
		/// If the attribute is either empty, does not exist or parsing fails <code>null</code> is returned.
		/// </remarks>
		public static double? GetDoubleAttributeValue( this IAttributeItemDto item, ushort key )
		{
			try
			{
				var value = GetAttributeValue( item, key );
				if( !string.IsNullOrEmpty( value ) )
					return double.Parse( value, CultureInfo.InvariantCulture );
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch
			{ }

			return null;
		}

		/// <summary>
		/// Returns the attribute's value of the attribute with the key <see cref="int"/> as <code>double</code>.
		/// </summary>
		/// <remarks>
		/// If the attribute is either empty, does not exist or parsing fails <code>null</code> is returned.
		/// </remarks>
		public static int? GetIntAttributeValue( this IAttributeItemDto item, ushort key )
		{
			try
			{
				var value = GetAttributeValue( item, key );
				if( !string.IsNullOrEmpty( value ) )
					return int.Parse( value, CultureInfo.InvariantCulture );
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch
			{ }

			return null;
		}

		/// <summary>
		/// Returns the attribute's value of the attribute with the key <see cref="DateTime"/> as <code>double</code>.
		/// </summary>
		/// <remarks>
		/// If the attribute is either empty, does not exist or parsing fails <code>null</code> is returned.
		/// </remarks>
		public static DateTime? GetDateAttributeValue( this IAttributeItemDto item, ushort key )
		{
			try
			{
				var value = GetAttributeValue( item, key );
				if( !string.IsNullOrEmpty( value ) )
					return XmlConvert.ToDateTime( value, XmlDateTimeSerializationMode.RoundtripKind );
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch
			{ }

			return null;
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
			var value = item.GetAttribute( key );
			if( value == null )
				return null;

			if( value.RawValue != null )
				return value.RawValue;

			return configuration.ParseValue( key, value.Value, catalogs );
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
		public static void SetAttribute( this IAttributeItemDto item, ushort key, string value )
		{
			item.SetAttribute( new AttributeDto( key, value ) );
		}

		/// <summary>
		/// Sets the <code>value</code> for the attribute with the key <code>key</code>.
		/// </summary>
		public static void SetAttribute( this IAttributeItemDto item, AttributeDto value )
		{
			if( value != null )
			{
				if( string.IsNullOrEmpty( value.Value ) )
				{
					RemoveAttribute( item, value.Key );
				}
				else
				{
					InternalSetAttribute( item, value );
				}
			}
		}

		/// <summary>
		/// Removes the attribute with the key <code>key</code>
		/// </summary>
		public static void RemoveAttribute( this IAttributeItemDto item, ushort key )
		{
			var att = GetAttribute( item, key, out var index );
			if( att != null )
			{
				// Remove attribute
				var atts = new AttributeDto[ item.Attributes.Length - 1 ];
				Array.Copy( item.Attributes, 0, atts, 0, index );
				Array.Copy( item.Attributes, index + 1, atts, index, item.Attributes.Length - index - 1 );

				item.Attributes = atts;
			}
		}

		private static void InternalSetAttribute( this IAttributeItemDto item, AttributeDto value )
		{
			var att = GetAttribute( item, value.Key, out var index );
			if( att == null )
			{
				// Add attribute
				if( item.Attributes != null && item.Attributes.Length > 0 )
				{
					var atts = new AttributeDto[ item.Attributes.Length + 1 ];
					Array.Copy( item.Attributes, atts, item.Attributes.Length );
					atts[ atts.Length - 1 ] = value;
					item.Attributes = atts;
				}
				else
				{
					item.Attributes = new[] { value };
				}
			}
			else
			{
				// Replace attribute
				var atts = item.Attributes;
				atts[ index ] = value;

				// Hack, damit das Objekt (z.B. SimpleMeasurement) die Änderungen mitbekommt und
				// evtl. gecachte Daten wieder wegwirft (z.B. SimpleMeasurement._CachedTime)
				item.Attributes = atts;
			}
		}

		#endregion
	}
}