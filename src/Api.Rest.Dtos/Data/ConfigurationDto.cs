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

		private IReadOnlyList<AbstractAttributeDefinitionDto> _PartAttributes = Array.Empty<AbstractAttributeDefinitionDto>();
		private IReadOnlyList<AbstractAttributeDefinitionDto> _CharacteristicAttributes = Array.Empty<AbstractAttributeDefinitionDto>();
		private IReadOnlyList<AbstractAttributeDefinitionDto> _MeasurementAttributes = Array.Empty<AbstractAttributeDefinitionDto>();
		private IReadOnlyList<AbstractAttributeDefinitionDto> _ValueAttributes = Array.Empty<AbstractAttributeDefinitionDto>();
		private IReadOnlyList<AttributeDefinitionDto> _CatalogAttributes = Array.Empty<AttributeDefinitionDto>();

		private Dictionary<ushort, AbstractAttributeDefinitionDto> _PartAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _CharacteristicAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _MeasurementAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _ValueAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _CatalogAttributesDict;
		private Dictionary<ushort, AbstractAttributeDefinitionDto> _AllAttributesDict;

		#endregion

		#region properties

		private Dictionary<ushort, AbstractAttributeDefinitionDto> PartAttributesDict
			=> _PartAttributesDict ??= FillTable( PartAttributes );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> CharacteristicAttributesDict
			=> _CharacteristicAttributesDict ??= FillTable( CharacteristicAttributes );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> MeasurementAttributesDict
			=> _MeasurementAttributesDict ??= FillTable( MeasurementAttributes );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> ValueAttributesDict
			=> _ValueAttributesDict ??= FillTable( ValueAttributes );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> CatalogAttributesDict
			=> _CatalogAttributesDict ??= FillTable( CatalogAttributes );

		private Dictionary<ushort, AbstractAttributeDefinitionDto> AllAttributesDict =>
			_AllAttributesDict ??= PartAttributesDict
				.Concat( CharacteristicAttributesDict )
				.Concat( MeasurementAttributesDict )
				.Concat( ValueAttributesDict )
				.Concat( CatalogAttributesDict )
				.ToDictionary( kvp => kvp.Key, kvp => kvp.Value );

		/// <summary>
		/// Returns a list of all attribute definitions in this configuration.
		/// </summary>
		[JsonIgnore]
		public IReadOnlyCollection<AbstractAttributeDefinitionDto> AllAttributes
		{
			[NotNull] get => AllAttributesDict.Values;
		}

		/// <summary>
		/// Gets or sets a list of all part attribute definitions.
		/// </summary>
		[JsonProperty( "partAttributes" )]
		public IReadOnlyList<AbstractAttributeDefinitionDto> PartAttributes
		{
			[NotNull] get => _PartAttributes;
			set
			{
				_PartAttributes = value ?? Array.Empty<AbstractAttributeDefinitionDto>();
				_PartAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all characteristic attribute definitions.
		/// </summary>
		[JsonProperty( "characteristicAttributes" )]
		public IReadOnlyList<AbstractAttributeDefinitionDto> CharacteristicAttributes
		{
			[NotNull] get => _CharacteristicAttributes;
			set
			{
				_CharacteristicAttributes = value ?? Array.Empty<AbstractAttributeDefinitionDto>();
				_CharacteristicAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all measurement attribute definitions.
		/// </summary>
		[JsonProperty( "measurementAttributes" )]
		public IReadOnlyList<AbstractAttributeDefinitionDto> MeasurementAttributes
		{
			[NotNull] get => _MeasurementAttributes;
			set
			{
				_MeasurementAttributes = value ?? Array.Empty<AbstractAttributeDefinitionDto>();
				_MeasurementAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all value attribute definitions.
		/// </summary>
		[JsonProperty( "valueAttributes" )]
		public IReadOnlyList<AbstractAttributeDefinitionDto> ValueAttributes
		{
			[NotNull] get => _ValueAttributes;
			set
			{
				_ValueAttributes = value ?? Array.Empty<AbstractAttributeDefinitionDto>();
				_ValueAttributesDict = null;
			}
		}

		/// <summary>
		/// Gets or sets a list of all catalog attribute definitions.
		/// </summary>
		[JsonProperty( "catalogAttributes" )]
		public IReadOnlyList<AttributeDefinitionDto> CatalogAttributes
		{
			[NotNull] get => _CatalogAttributes;
			set
			{
				_CatalogAttributes = value ?? Array.Empty<AttributeDefinitionDto>();
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
			return entityType switch
			{
				EntityDto.Part => _PartAttributes,
				EntityDto.Characteristic => _CharacteristicAttributes,
				EntityDto.Measurement => _MeasurementAttributes,
				EntityDto.Value => _ValueAttributes,
				EntityDto.Catalog => _CatalogAttributes,
				_ => Array.Empty<AbstractAttributeDefinitionDto>()
			};
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
			return entityType switch
			{
				EntityDto.Part => PartAttributesDict.ContainsKey( key ),
				EntityDto.Characteristic => CharacteristicAttributesDict.ContainsKey( key ),
				EntityDto.Measurement => MeasurementAttributesDict.ContainsKey( key ),
				EntityDto.Value => ValueAttributesDict.ContainsKey( key ),
				EntityDto.Catalog => CatalogAttributesDict.ContainsKey( key ),
				_ => false
			};
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
			if( string.IsNullOrEmpty( value ) )
				return null;

			var parsedValue = ParseValue( key, value, catalogs );
			if( parsedValue is IFormattable formattable )
				return formattable.ToString( null, provider );

			return parsedValue.ToString();
		}

		/// <summary>
		/// Returns the value represented by <paramref name="attributeValue"/>. <paramref name="attributeValue"/> musst be the language neutral database entry of an attribute.
		/// I.e. if an attribute represents a catatalogue entry, the catalog entry is returned. If an attribute represents a normal value
		/// of type string, int, double or DateTime the value is returned with that type.
		/// </summary>
		public object ParseValue( ushort key, string attributeValue, CatalogCollectionDto catalogs )
		{
			if( string.IsNullOrEmpty( attributeValue ) )
				return attributeValue;

			var definition = GetDefinition( key );
			if( definition is null )
				throw new ArgumentException( $"Unable to parse attribute value - key '{key}' does not exist." );

			if( definition is CatalogAttributeDefinitionDto catalogAttributeDefinition )
			{
				if( catalogs != null )
					return catalogs[ catalogAttributeDefinition.Catalog, attributeValue ];

				return attributeValue;
			}

			var attributeDefinition = (AttributeDefinitionDto)definition;
			switch( attributeDefinition.Type )
			{
				case AttributeTypeDto.Integer:
#if NETSTANDARD
					return int.Parse( attributeValue, NumberStyles.Integer, CultureInfo.InvariantCulture );
#else
					return int.Parse( attributeValue.AsSpan(), NumberStyles.Integer, CultureInfo.InvariantCulture );
#endif
				case AttributeTypeDto.Float:
#if NETSTANDARD
					return double.Parse( attributeValue, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture );
#else
					return double.Parse( attributeValue.AsSpan(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture );
#endif
				case AttributeTypeDto.DateTime:
					return XmlConvert.ToDateTime( attributeValue, XmlDateTimeSerializationMode.RoundtripKind );
			}

			return attributeValue;
		}

		private static Dictionary<ushort, AbstractAttributeDefinitionDto> FillTable( IEnumerable<AbstractAttributeDefinitionDto> attributes )
		{
			return attributes.ToDictionary( a => a.Key, a => a );
		}

		#endregion
	}
}