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
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// This is the base class for inspection plan entities (i.e. parts and characteristics). Each inspection plan entity is identified by an <see cref="Uuid"/>
	/// (constants, even when renamed) and a unique <see cref="Path"/>.
	/// </summary>
	[DebuggerDisplay( "{" + nameof( Path ) + "}" )]
	public abstract class InspectionPlanDtoBase : IAttributeItemDto
	{
		#region members

		[JsonProperty( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		private IReadOnlyList<AttributeDto> _Attributes;

		[JsonProperty( "path" ), JsonConverter( typeof( PathInformationConverter ) )]
		private PathInformationDto _Path;

		#endregion

		#region properties

		/// <summary>
		/// Indexer for accessing the attribute value with the specified <code>key</code>.
		/// </summary>
		public string this[ ushort key ]
		{
			get => this.GetAttributeValue( key );
			set => this.SetAttributeValue( key, value );
		}

		/// <summary>
		/// Gets or sets the uuid of this inspection plan entity. The uuid is always constant, even if
		/// this entity is renamed.
		/// </summary>
		[JsonProperty( "uuid" )]
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the comment of this inspection plan entity.
		/// </summary>
		[JsonProperty( "comment" )]
		public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the path of this inspection plan entity.
		/// </summary>
		[JsonIgnore]
		public PathInformationDto Path
		{
			[NotNull] get => _Path ?? PathInformationDto.Root;
			set => _Path = value;
		}

		/// <summary>
		/// This is the version number of this entity. The version number is using a global versioning scheme accross all
		/// version changes of the whole inspection plan. This means, that single instances of a part or characteristic can
		/// have non consecutive version numbers.
		/// </summary>
		[JsonProperty( "version" )]
		public uint Version { get; set; }

		/// <summary>
		/// Contains the date and time of the last change applied to this instance.
		/// </summary>
		[JsonProperty( "timestamp" )]
		public DateTime Timestamp { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return Version + " - " + Path;
		}

		#endregion

		#region interface IAttributeItemDto

		/// <inheritdoc />
		[JsonIgnore]
		public IReadOnlyList<AttributeDto> Attributes
		{
			[NotNull] get => _Attributes ?? Array.Empty<AttributeDto>();
			set => _Attributes = value;
		}

		#endregion
	}
}