#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data
{
	#region using

	using System;
	using System.Globalization;
	using System.Xml;
	using Zeiss.IMT.PiWeb.Api.DataService.Rest;
	using Attribute = Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute;

	#endregion

	/// <summary>
	/// Extension class with various helper methods for attribute list manipulation.
	/// </summary>
	public static class AttributeItemExtensions
	{
		/// <summary>
		/// Returns the attribute with the key <code>key</code>.
		/// </summary>
		public static Attribute GetAttribute( this IAttributeItem item, ushort key )
		{
			int index;
			return GetAttribute( item, key, out index );
		}

		/// <summary>
		/// Returns the attribute with the key <code>key</code>.
		/// </summary>
		public static Attribute GetAttribute( this IAttributeItem item, ushort key, out int index )
		{
			if( item != null && item.Attributes != null )
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
		public static string GetAttributeValue( this IAttributeItem item, ushort key )
		{
			var value = GetAttribute( item, key );
			return value != null ? value.Value : null;
		}

		/// <summary>
		/// Returns the attribute's value of the attribute with the key <see cref="double"/> as <code>double</code>.
		/// </summary>
		/// <remarks>
		/// If the attribute is either empty, does not exist or parsing fails <code>null</code> is returned.
		/// </remarks>
		public static double? GetDoubleAttributeValue( this IAttributeItem item, ushort key )
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
		public static int? GetIntAttributeValue( this IAttributeItem item, ushort key )
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
		public static DateTime? GetDateAttributeValue( this IAttributeItem item, ushort key )
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
			this IAttributeItem item,
			ushort key,
			Configuration configuration,
			CatalogCollection catalogs )
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
		public static void InsertNullAttribute( this IAttributeItem item, ushort key )
		{
			InternalSetAttribute( item, new Attribute( key, null ) );
		}

		/// <summary>
		/// Sets the <code>value</code> for the attribute with the key <code>key</code>.
		/// </summary>
		public static void SetAttribute( this IAttributeItem item, ushort key, string value )
		{
			item.SetAttribute( new Attribute( key, value ) );
		}

		/// <summary>
		/// Sets the <code>value</code> for the attribute with the key <code>key</code>.
		/// </summary>
		public static void SetAttribute( this IAttributeItem item, Attribute value )
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
		public static void RemoveAttribute( this IAttributeItem item, ushort key )
		{
			int index;
			var att = GetAttribute( item, key, out index );
			if( att != null )
			{
				// Remove attribute
				var atts = new Attribute[ item.Attributes.Length - 1 ];
				Array.Copy( item.Attributes, 0, atts, 0, index );
				Array.Copy( item.Attributes, index + 1, atts, index, item.Attributes.Length - index - 1 );

				item.Attributes = atts;
			}
		}

		private static void InternalSetAttribute( this IAttributeItem item, Attribute value )
		{
			int index;
			var att = GetAttribute( item, value.Key, out index );
			if( att == null )
			{
				// Add attribute
				if( item.Attributes != null && item.Attributes.Length > 0 )
				{
					var atts = new Attribute[ item.Attributes.Length + 1 ];
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
	}
}