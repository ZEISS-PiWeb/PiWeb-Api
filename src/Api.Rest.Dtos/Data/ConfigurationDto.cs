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
	using JetBrains.Annotations;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Holds the configuration of the entire database and defines the possible attribute definitions of all entities.
	/// </summary>
	public class ConfigurationDto
	{
		#region members

		private AbstractAttributeDefinitionDto[] _PartAttributes = new AbstractAttributeDefinitionDto[ 0 ];
		private AbstractAttributeDefinitionDto[] _CharacteristicAttributes = new AbstractAttributeDefinitionDto[ 0 ];
		private AbstractAttributeDefinitionDto[] _MeasurementAttributes = new AbstractAttributeDefinitionDto[ 0 ];
		private AbstractAttributeDefinitionDto[] _ValueAttributes = new AbstractAttributeDefinitionDto[ 0 ];
		private AttributeDefinitionDto[] _CatalogAttributes = new AttributeDefinitionDto[ 0 ];

		private Dictionary<ushort, AbstractAttributeDefinitionDto> _PartAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _CharacteristicAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _MeasurementAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _ValueAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _CatalogAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _AllAttributesDict;

		#endregion

		#region properties

		private Dictionary<ushort, AbstractAttributeDefinitionDto> PartAttributesDict
			=> _PartAttributesDict ?? ( _PartAttributesDict = FillTable( PartAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> CharacteristicAttributesDict
			=> _CharacteristicAttributesDict ?? ( _CharacteristicAttributesDict = FillTable( CharacteristicAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> MeasurementAttributesDict
			=> _MeasurementAttributesDict ?? ( _MeasurementAttributesDict = FillTable( MeasurementAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> ValueAttributesDict
			=> _ValueAttributesDict ?? ( _ValueAttributesDict = FillTable( ValueAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> CatalogAttributesDict
			=> _CatalogAttributesDict ?? ( _CatalogAttributesDict = FillTable( CatalogAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> AllAttributesDict =>
			_AllAttributesDict ?? ( _AllAttributesDict = PartAttributesDict
				.Concat( CharacteristicAttributesDict )
				.Concat( MeasurementAttributesDict )
				.Concat( ValueAttributesDict )
				.Concat( CatalogAttributesDict )
				.ToDictionary( kvp => kvp.Key, kvp => kvp.Value ) );

		/// <summary>
		/// Returns a list of all attribute definitions in this configuration.
		/// </summary>
		[JsonIgnore]
		public AbstractAttributeDefinitionDto[] AllAttributes
		{
			[NotNull] get => AllAttributesDict.Values.ToArray();
		}

		/// <summary>
		/// Gets or sets a list of all part attribute definitions.
		/// </summary>
		[JsonProperty( "partAttributes" )]
		public AbstractAttributeDefinitionDto[] PartAttributes
		{
			[NotNull] get => _PartAttributes;
			set
			{
				_PartAttributes = value ?? new AbstractAttributeDefinitionDto[ 0 ];
				_PartAttributes = _PartAttributes.OrderBy( a => a.Key ).ToArray();

				_PartAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all characteristic attribute definitions.
		/// </summary>
		[JsonProperty( "characteristicAttributes" )]
		public AbstractAttributeDefinitionDto[] CharacteristicAttributes
		{
			[NotNull] get => _CharacteristicAttributes;
			set
			{
				_CharacteristicAttributes = value ?? new AbstractAttributeDefinitionDto[ 0 ];
				_CharacteristicAttributes = _CharacteristicAttributes.OrderBy( a => a.Key ).ToArray();

				_CharacteristicAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all measurement attribute definitions.
		/// </summary>
		[JsonProperty( "measurementAttributes" )]
		public AbstractAttributeDefinitionDto[] MeasurementAttributes
		{
			[NotNull] get => _MeasurementAttributes;
			set
			{
				_MeasurementAttributes = value ?? new AbstractAttributeDefinitionDto[ 0 ];
				_MeasurementAttributes = _MeasurementAttributes.OrderBy( a => a.Key ).ToArray();

				_MeasurementAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all value attribute definitions.
		/// </summary>
		[JsonProperty( "valueAttributes" )]
		public AbstractAttributeDefinitionDto[] ValueAttributes
		{
			[NotNull] get => _ValueAttributes;
			set
			{
				_ValueAttributes = value ?? new AbstractAttributeDefinitionDto[ 0 ];
				_ValueAttributes = _ValueAttributes.OrderBy( a => a.Key ).ToArray();

				_ValueAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all catalog attribute definitions.
		/// </summary>
		[JsonProperty( "catalogAttributes" )]
		public AttributeDefinitionDto[] CatalogAttributes
		{
			[NotNull] get => _CatalogAttributes;
			set
			{
				_CatalogAttributes = value ?? new AttributeDefinitionDto[ 0 ];
				_CatalogAttributes = _CatalogAttributes.OrderBy( a => a.Key ).ToArray();

				_CatalogAttributesDict = null;
			}
		}

		/// <summary>
		/// The attribute <code>versioningType</code> defines, whether updates to parts or characteristics
		/// overwrite existing values, or whether they create a new version of the entity.
		/// </summary>
		[JsonProperty( "versioningType" )]
		public VersioningTypeDto VersioningType { get; set; }

		#endregion

		#region methods

		/// <summary>
		///Returns the name of the attribute definition specified by <code>key</code>.
		/// </summary>
		/// <param name="key">The key of the attribute definition.</param>
		public string GetName( ushort key )
		{
			return AllAttributesDict.TryGetValue( key, out var def ) ? def.Description : "";
		}

		/// <summary>
		/// Returns the attribute definition with <code>key</code>.
		/// </summary>
		/// <param name="key">The key of the attribute definition.</param>
		public AbstractAttributeDefinitionDto GetDefinition( ushort key )
		{
			return AllAttributesDict.TryGetValue( key, out var result ) ? result : null;
		}

		/// <summary>
		/// Returns all attribute definitions for entity type <code>entityType</code>.
		/// </summary>
		public IEnumerable<AbstractAttributeDefinitionDto> GetDefinitions( EntityDto entityType )
		{
			switch( entityType )
			{
				case EntityDto.Part:
					return _PartAttributes;
				case EntityDto.Characteristic:
					return _CharacteristicAttributes;
				case EntityDto.Measurement:
					return _MeasurementAttributes;
				case EntityDto.Value:
					return _ValueAttributes;
				case EntityDto.Catalog:
					return _CatalogAttributes;
				default:
					return new AbstractAttributeDefinitionDto[ 0 ];
			}
		}

		/// <summary>
		/// Returns the attribute definition with <code>key</code> for entity type <code>entity</code>.
		/// </summary>
		/// <param name="key">The key of the attribute definition.</param>
		/// <param name="type">The entity type for which the definition should be returned.</param>
		[CanBeNull]
		public AbstractAttributeDefinitionDto GetDefinition( EntityDto type, ushort key )
		{
			AbstractAttributeDefinitionDto result = null;
			switch( type )
			{
				case EntityDto.Part:
					PartAttributesDict.TryGetValue( key, out result );
					break;
				case EntityDto.Characteristic:
					CharacteristicAttributesDict.TryGetValue( key, out result );
					break;
				case EntityDto.Measurement:
					MeasurementAttributesDict.TryGetValue( key, out result );
					break;
				case EntityDto.Value:
					ValueAttributesDict.TryGetValue( key, out result );
					break;
				case EntityDto.Catalog:
					CatalogAttributesDict.TryGetValue( key, out result );
					break;
			}

			return result;
		}

		/// <summary>
		/// Returns <code>true</code> if the attribute definition with key <code>key</code> belongs to an entity
		/// with type <code>entityType</code>.
		/// </summary>
		public bool IsKeyOfType( EntityDto entityType, ushort key )
		{
			switch( entityType )
			{
				case EntityDto.Part:
					return PartAttributesDict.ContainsKey( key );
				case EntityDto.Characteristic:
					return CharacteristicAttributesDict.ContainsKey( key );
				case EntityDto.Measurement:
					return MeasurementAttributesDict.ContainsKey( key );
				case EntityDto.Value:
					return ValueAttributesDict.ContainsKey( key );
				case EntityDto.Catalog:
					return CatalogAttributesDict.ContainsKey( key );
			}

			return false;
		}

		/// <summary>
		/// Return the entity type <code>entityType</code> of the attribute definition with <code>key</code>. If the key
		/// does not exist in this configuration, <code>null</code> is returned.
		/// </summary>
		public EntityDto? GetTypeForKey( ushort key )
		{
			if( PartAttributesDict.ContainsKey( key ) )
				return EntityDto.Part;

			if( CharacteristicAttributesDict.ContainsKey( key ) )
				return EntityDto.Characteristic;

			if( MeasurementAttributesDict.ContainsKey( key ) )
				return EntityDto.Measurement;

			if( ValueAttributesDict.ContainsKey( key ) )
				return EntityDto.Value;

			if( CatalogAttributesDict.ContainsKey( key ) )
				return EntityDto.Catalog;

			return null;
		}

		/// <summary>
		/// Returns a formatted value for the attribute <paramref name="value"/>. If the value is a catalog entry, the catalog value will
		/// be returned. This method uses the <paramref name="provider"/> for formatting the value. If the <paramref name="provider"/>
		/// is not specified, the <see cref="CultureInfo.CurrentUICulture"/> is used to format the resulting value.
		/// </summary>
		public string GetFormattedValue( ushort key, string value, CatalogCollectionDto catalogs, IFormatProvider provider = null )
		{
			if( value == null )
				return null;

			var def = GetDefinition( key );
			if( def is CatalogAttributeDefinitionDto catalogAttributeDefinition )
			{
				var entry = catalogs?[ catalogAttributeDefinition.Catalog, value ];
				if( entry != null )
					return entry.ToString( CultureInfo.InvariantCulture );
			}
			else if( value.Length > 0 )
			{
				try
				{
					var attDef = (AttributeDefinitionDto)def;
					switch( attDef.Type )
					{
						case AttributeTypeDto.Integer:
							return int.Parse( value, CultureInfo.InvariantCulture ).ToString( provider ?? CultureInfo.CurrentUICulture );
						case AttributeTypeDto.Float:
							return double.Parse( value, CultureInfo.InvariantCulture ).ToString( provider ?? CultureInfo.CurrentUICulture );
						case AttributeTypeDto.DateTime:
							return XmlConvert.ToDateTime( value, XmlDateTimeSerializationMode.RoundtripKind ).ToString( provider ?? CultureInfo.CurrentUICulture );
					}
				}
				catch
				{
					// ignored
				}
			}

			return value;
		}

		/// <summary>
		/// Returns the value represented by <paramref name="attributeValue"/>. <paramref name="attributeValue"/> musst be the language neutral database entry of an attribute.
		/// I.e. if an attribute represents a catatalogue entry, the catalog entry is returned. If an attribute represents a normal value
		/// of type string, int, double or DateTime the value is returned with that type.
		/// </summary>
		public object ParseValue( ushort key, string attributeValue, CatalogCollectionDto catalogs )
		{
			object result = attributeValue;
			if( attributeValue != null )
			{
				var def = GetDefinition( key );
				if( def is CatalogAttributeDefinitionDto definition )
				{
					if( catalogs != null )
						result = catalogs[ definition.Catalog, attributeValue ];
				}
				else if( attributeValue.Length > 0 )
				{
					try
					{
						var attDef = (AttributeDefinitionDto)def;
						switch( attDef.Type )
						{
							case AttributeTypeDto.Integer:
								result = int.Parse( attributeValue, CultureInfo.InvariantCulture );
								break;
							case AttributeTypeDto.Float:
								result = double.Parse( attributeValue, CultureInfo.InvariantCulture );
								break;
							case AttributeTypeDto.DateTime:
								result = XmlConvert.ToDateTime( attributeValue, XmlDateTimeSerializationMode.RoundtripKind );
								break;
						}
					}
					catch
					{
						// ignored
					}
				}
			}

			return result ?? attributeValue;
		}

		private static Dictionary<ushort, AbstractAttributeDefinitionDto> FillTable( IEnumerable<AbstractAttributeDefinitionDto> atts )
		{
			return atts.ToDictionary( a => a.Key, a => a );
		}

		#endregion
	}
}