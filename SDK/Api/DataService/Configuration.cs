#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Xml;
	
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This element holds the configuration of the service.
	/// It defines the possible attributes that each entity (like Part, Characteristic, ...) may have.
	/// When a client wants to construct an entity, it must not use any attributes which are not defined
	/// in the corresponding list.
	/// The entities Part and Characteristic have significant attributes. The list of significant
	/// attributes has the function of a primary key: There must be not more than one Part in the database
	/// with the same values for the set of significant part attributes. The same applies to
	/// Characteristics, but there must be not more than one Characteristic with a given
	/// signature below a given root Part (a Part with no parent). This means, that there may be
	/// several Characteristics with the same signature, but then each one has to have a different root Part.
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.ConfigurationConverter ) )]
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
		{
			get
			{
				if( _PartAttributesDict == null )
					FillTable( PartAttributes, ref _PartAttributesDict );
				return _PartAttributesDict;
			}
		}

		private Dictionary<ushort, AbstractAttributeDefinition> CharacteristicAttributesDict
		{
			get
			{
				if( _CharacteristicAttributesDict == null )
					FillTable( CharacteristicAttributes, ref _CharacteristicAttributesDict );
				return _CharacteristicAttributesDict;
			}
		}

		private Dictionary<ushort, AbstractAttributeDefinition> MeasurementAttributesDict
		{
			get
			{
				if( _MeasurementAttributesDict == null )
					FillTable( MeasurementAttributes, ref _MeasurementAttributesDict );
				return _MeasurementAttributesDict;
			}
		}

		private Dictionary<ushort, AbstractAttributeDefinition> ValueAttributesDict
		{
			get
			{
				if( _ValueAttributesDict == null )
					FillTable( ValueAttributes, ref _ValueAttributesDict );
				return _ValueAttributesDict;
			}
		}

		private Dictionary<ushort, AbstractAttributeDefinition> CatalogAttributesDict
		{
			get
			{
				if( _CatalogAttributesDict == null )
					FillTable( _CatalogAttributes, ref _CatalogAttributesDict );
				return _CatalogAttributesDict;
			}
		}

		private Dictionary<ushort, AbstractAttributeDefinition> AllAttributesDict
		{
			get
			{
				if( _AllAttributesDict == null )
				{
					_AllAttributesDict = new Dictionary<ushort, AbstractAttributeDefinition>();
					FillTable( _PartAttributes, ref _AllAttributesDict );
					FillTable( _CharacteristicAttributes, ref _AllAttributesDict );
					FillTable( _MeasurementAttributes, ref  _AllAttributesDict );
					FillTable( _ValueAttributes, ref _AllAttributesDict );
					FillTable( _CatalogAttributes, ref  _AllAttributesDict );
				}
				return _AllAttributesDict;
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
			get { return AllAttributesDict.Values.ToArray(); }
		}

		/// <summary>
		/// Gets or sets a list of all part attribute definitions.
		/// </summary>
		public AbstractAttributeDefinition[] PartAttributes
		{
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
		public AbstractAttributeDefinition[] CharacteristicAttributes
		{
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
		public AbstractAttributeDefinition[] MeasurementAttributes
		{
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
		public AbstractAttributeDefinition[] ValueAttributes
		{
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
		public AttributeDefinition[] CatalogAttributes
		{
			get { return _CatalogAttributes; }
			set
			{
				_CatalogAttributes = value ?? new AttributeDefinition[ 0 ];
				_CatalogAttributes = _CatalogAttributes.OrderBy( a => a.Key ).ToArray();

				_CatalogAttributesDict = null;
			}
		}

		/// <summary>
		/// The attribute <code>versioningType</code> defines, whether updates to Parts or Characteristics
		/// overwrite existing values, or whether they create a new version of the entity.
		/// </summary>
		[JsonConverter( typeof( Newtonsoft.Json.Converters.StringEnumConverter ) )]
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

		private static void FillTable( AbstractAttributeDefinition[] atts, ref Dictionary<ushort, AbstractAttributeDefinition> table )
		{
			table = table ?? new Dictionary<ushort, AbstractAttributeDefinition>();
			foreach( var def in atts ) table[ def.Key ] = def;
		}

		#endregion
	}
}