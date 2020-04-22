#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region using

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
	public class Configuration
	{
		#region members

		private AbstractAttributeDefinition[] _PartAttributes = new AbstractAttributeDefinition[ 0 ];
		private AbstractAttributeDefinition[] _CharacteristicAttributes = new AbstractAttributeDefinition[ 0 ];
		private AbstractAttributeDefinition[] _MeasurementAttributes = new AbstractAttributeDefinition[ 0 ];
		private AbstractAttributeDefinition[] _ValueAttributes = new AbstractAttributeDefinition[ 0 ];
		private AttributeDefinition[] _CatalogAttributes = new AttributeDefinition[ 0 ];

		private Dictionary<ushort, AbstractAttributeDefinition> _PartAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinition> _CharacteristicAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinition> _MeasurementAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinition> _ValueAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinition> _CatalogAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinition> _AllAttributesDict;

		private Dictionary<ushort, AbstractAttributeDefinition> PartAttributesDict
			=> _PartAttributesDict ?? ( _PartAttributesDict = FillTable( PartAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinition> CharacteristicAttributesDict
			=> _CharacteristicAttributesDict ?? ( _CharacteristicAttributesDict = FillTable( CharacteristicAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinition> MeasurementAttributesDict
			=> _MeasurementAttributesDict ?? ( _MeasurementAttributesDict = FillTable( MeasurementAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinition> ValueAttributesDict
			=> _ValueAttributesDict ?? ( _ValueAttributesDict = FillTable( ValueAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinition> CatalogAttributesDict
			=> _CatalogAttributesDict ?? ( _CatalogAttributesDict = FillTable( CatalogAttributes ) );

		private Dictionary<ushort, AbstractAttributeDefinition> AllAttributesDict
		{
			get
			{
				return _AllAttributesDict ?? ( _AllAttributesDict = PartAttributesDict
					.Concat( CharacteristicAttributesDict )
					.Concat( MeasurementAttributesDict )
					.Concat( ValueAttributesDict )
					.Concat( CatalogAttributesDict )
					.ToDictionary( kvp => kvp.Key, kvp => kvp.Value ) );
			}
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns a list of all attribute definitions in this configuration.
		/// </summary>
		[JsonIgnore]
		public AbstractAttributeDefinition[] AllAttributes 
		{
			[NotNull]
			get { return AllAttributesDict.Values.ToArray(); }
		}

		/// <summary>
		/// Gets or sets a list of all part attribute definitions.
		/// </summary>
		[JsonProperty( "partAttributes" )]
		public AbstractAttributeDefinition[] PartAttributes
		{
			[NotNull]
			get { return _PartAttributes; }
			set
			{
				_PartAttributes = value ?? new AbstractAttributeDefinition[ 0 ];
				_PartAttributes = _PartAttributes.OrderBy( a => a.Key ).ToArray();

				_PartAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all characteristic attribute definitions.
		/// </summary>
		[JsonProperty( "characteristicAttributes" )]
		public AbstractAttributeDefinition[] CharacteristicAttributes
		{
			[NotNull]
			get { return _CharacteristicAttributes; }
			set
			{
				_CharacteristicAttributes = value ?? new AbstractAttributeDefinition[ 0 ];
				_CharacteristicAttributes = _CharacteristicAttributes.OrderBy( a => a.Key ).ToArray();

				_CharacteristicAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all measurement attribute definitions.
		/// </summary>
		[JsonProperty( "measurementAttributes" )]
		public AbstractAttributeDefinition[] MeasurementAttributes
		{
			[NotNull]
			get { return _MeasurementAttributes; }
			set
			{
				_MeasurementAttributes = value ?? new AbstractAttributeDefinition[ 0 ];
				_MeasurementAttributes = _MeasurementAttributes.OrderBy( a => a.Key ).ToArray();
				
				_MeasurementAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all value attribute definitions.
		/// </summary>
		[JsonProperty( "valueAttributes" )]
		public AbstractAttributeDefinition[] ValueAttributes
		{
			[NotNull]
			get { return _ValueAttributes; }
			set
			{
				_ValueAttributes = value ?? new AbstractAttributeDefinition[ 0 ];
				_ValueAttributes = _ValueAttributes.OrderBy( a => a.Key ).ToArray();

				_ValueAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all catalog attribute definitions.
		/// </summary>
		[JsonProperty( "catalogAttributes" )]
		public AttributeDefinition[] CatalogAttributes
		{
			[NotNull]
			get { return _CatalogAttributes; }
			set
			{
				_CatalogAttributes = value ?? new AttributeDefinition[ 0 ];
				_CatalogAttributes = _CatalogAttributes.OrderBy( a => a.Key ).ToArray();

				_CatalogAttributesDict = null;
			}
		}

		/// <summary>
		/// The attribute <code>versioningType</code> defines, whether updates to parts or characteristics
		/// overwrite existing values, or whether they create a new version of the entity.
		/// </summary>
		[JsonProperty( "versioningType" )]
		public VersioningType VersioningType { get; set; }

		#endregion

		#region methods

		/// <summary>
		///Returns the name of the attribute definition specified by <code>key</code>.
		/// </summary>
		/// <param name="key">The key of the attribute definition.</param>
		public string GetName( ushort key )
		{
			AbstractAttributeDefinition def;
			return AllAttributesDict.TryGetValue( key, out def ) ? def.Description : "";
		}

		/// <summary>
		/// Returns the attribute definition with <code>key</code>.
		/// </summary>
		/// <param name="key">The key of the attribute definition.</param>
		public AbstractAttributeDefinition GetDefinition( ushort key )
		{
			AbstractAttributeDefinition result;
			return AllAttributesDict.TryGetValue( key, out result ) ? result : null;
		}

		/// <summary>
		/// Returns all attribute definitions for entity type <code>entityType</code>.
		/// </summary>
		public IEnumerable<AbstractAttributeDefinition> GetDefinitions( Entity entityType )
		{
			switch( entityType )
			{
				case Entity.Part:
					return _PartAttributes;
				case Entity.Characteristic:
					return _CharacteristicAttributes;
				case Entity.Measurement:
					return _MeasurementAttributes;
				case Entity.Value:
					return _ValueAttributes;
				case Entity.Catalog:
					return _CatalogAttributes;
				default:
					return new AbstractAttributeDefinition[ 0 ];
			}
		}
		
		/// <summary>
		/// Returns the attribute definition with <code>key</code> for entity type <code>entity</code>.
		/// </summary>
		/// <param name="key">The key of the attribute definition.</param>
		/// <param name="type">The entity type for which the definition should be returned.</param>
		[CanBeNull]
		public AbstractAttributeDefinition GetDefinition( Entity type, ushort key )
		{
			AbstractAttributeDefinition result = null;
			switch( type )
			{
				case Entity.Part:
					PartAttributesDict.TryGetValue( key, out result );
					break;
				case Entity.Characteristic:
					CharacteristicAttributesDict.TryGetValue( key, out result );
					break;
				case Entity.Measurement:
					MeasurementAttributesDict.TryGetValue( key, out result );
					break;
				case Entity.Value:
					ValueAttributesDict.TryGetValue( key, out result );
					break;
				case Entity.Catalog:
					CatalogAttributesDict.TryGetValue( key, out result );
					break;
			}
			return result;
		}

		/// <summary>
		/// Returns <code>true</code> if the attribute definition with key <code>key</code> belongs to an entity 
		/// with type <code>entityType</code>.
		/// </summary>
		public bool IsKeyOfType( Entity entityType, ushort key )
		{
			switch( entityType )
			{
				case Entity.Part:
					return PartAttributesDict.ContainsKey( key );
				case Entity.Characteristic:
					return CharacteristicAttributesDict.ContainsKey( key );
				case Entity.Measurement:
					return MeasurementAttributesDict.ContainsKey( key );
				case Entity.Value:
					return ValueAttributesDict.ContainsKey( key );
				case Entity.Catalog:
					return CatalogAttributesDict.ContainsKey( key );
			}
			return false;
		}

		/// <summary>
		/// Return the entity type <code>entityType</code> of the attribute definition with <code>key</code>. If the key 
		/// does not exist in this configuration, <code>null</code> is returned.
		/// </summary>
		public Entity? GetTypeForKey( ushort key )
		{
			if( PartAttributesDict.ContainsKey( key ) )
				return Entity.Part;

			if( CharacteristicAttributesDict.ContainsKey( key ) )
				return Entity.Characteristic;

			if( MeasurementAttributesDict.ContainsKey( key ) )
				return Entity.Measurement;

			if( ValueAttributesDict.ContainsKey( key ) )
				return Entity.Value;

			if( CatalogAttributesDict.ContainsKey( key ) )
				return Entity.Catalog;
		
			return null;
		}

		/// <summary>
		/// Returns a formatted value for the attribute <paramref name="value"/>. If the value is a catalog entry, the catalog value will
		/// be returned. This method uses the <paramref name="provider"/> for formatting the value. If the <paramref name="provider"/> 
		/// is not specified, the <see cref="CultureInfo.CurrentUICulture"/> is used to format the resulting value.
		/// </summary>
		public string GetFormattedValue( ushort key, string value, CatalogCollection catalogs, IFormatProvider provider = null )
		{
			if( value == null )
				return null;

			var def = GetDefinition( key );
			if( def is CatalogAttributeDefinition )
			{
				if( catalogs != null )
				{
					var entry = catalogs[ ( (CatalogAttributeDefinition)def ).Catalog, value ];
					if( entry != null )
						return entry.ToString( CultureInfo.InvariantCulture );
				}
			}
			else if( value.Length > 0 )
			{
				try
				{
					var attDef = (AttributeDefinition)def;
					switch( attDef.Type )
					{
						case AttributeType.Integer:
							return int.Parse( value, CultureInfo.InvariantCulture ).ToString( provider ?? CultureInfo.CurrentUICulture );
						case AttributeType.Float:
							return double.Parse( value, CultureInfo.InvariantCulture ).ToString( provider ?? CultureInfo.CurrentUICulture );
						case AttributeType.DateTime:
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
		public object ParseValue( ushort key, string attributeValue, CatalogCollection catalogs )
		{
			object result = attributeValue;
			if( attributeValue != null )
			{
				var def = GetDefinition( key );
				var definition = def as CatalogAttributeDefinition;
				if( definition != null )
				{
					if( catalogs != null )
						result = catalogs[definition.Catalog, attributeValue];
				}
				else if( attributeValue.Length > 0 )
				{
					try
					{
						var attDef = ( AttributeDefinition )def;
						switch( attDef.Type )
						{
							case AttributeType.Integer: result = int.Parse( attributeValue, CultureInfo.InvariantCulture ); break;
							case AttributeType.Float: result = double.Parse( attributeValue, CultureInfo.InvariantCulture ); break;
							case AttributeType.DateTime: result = XmlConvert.ToDateTime( attributeValue, XmlDateTimeSerializationMode.RoundtripKind ); break;
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

		private static Dictionary<ushort, AbstractAttributeDefinition> FillTable( IEnumerable<AbstractAttributeDefinition> atts )
		{
			return atts.ToDictionary( a => a.Key, a => a );
		}

		#endregion
	}
}