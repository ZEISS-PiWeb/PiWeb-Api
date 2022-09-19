#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System;
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// This object holds information for a raw data object like its size, key, name etc.
	/// </summary>
	[Serializable]
	public class RawDataInformationDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataInformationDto"/> class.
		/// </summary>
		public RawDataInformationDto()
		{
			Target = new RawDataTargetEntityDto();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataInformationDto"/> class.
		/// </summary>
		/// <param name="entity">The entity this object belongs to.</param>
		/// <param name="key">A unique key that identifies this information object.</param>
		/// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null" />.</exception>
		public RawDataInformationDto( [NotNull] RawDataTargetEntityDto entity, int key )
		{
			Target = entity ?? throw new ArgumentNullException( nameof( entity ) );
			Key = key;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataInformationDto"/> class.
		/// </summary>
		/// <param name="data">The raw data information object that should be copied.</param>
		/// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null" />.</exception>
		public RawDataInformationDto( [NotNull] RawDataInformationDto data )
		{
			if( data == null ) throw new ArgumentNullException( nameof( data ) );

			Target = data.Target;
			Key = data.Key;
			FileName = data.FileName;
			MimeType = data.MimeType;
			Created = data.Created;
			LastModified = data.LastModified;
			Size = data.Size;
			MD5 = data.MD5;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the target object this raw data object belongs to.
		/// </summary>
		[JsonPropertyName( "target" )]
		public RawDataTargetEntityDto Target { get; set; }

		/// <summary>
		/// This is a unique key that identifies this specific raw data object for a corresponding entity. An entity (Part, Characteristic, Measurement, Value)
		/// can have multiple raw data object that are distinct by this key.
		/// </summary>
		[JsonPropertyName( "key" )]
		public int? Key { get; set; }

		/// <summary>
		/// Gets or sets the filename of the raw data object.
		/// </summary>
		/// <remarks>
		/// Please note that this filename is not unique (unlike filenames in traditional file systems).
		/// </remarks>
		[JsonPropertyName( "fileName" )]
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the mime type of the raw data object.
		/// </summary>
		[JsonPropertyName( "mimeType" )]
		public string MimeType { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the last modification of the corresponding raw data object.
		/// </summary>
		/// <remarks>
		/// The attribute <code>LastModified</code> will be set by the Rawdata-Service.
		/// A user value will be ignored.
		/// </remarks>
		[JsonPropertyName( "lastModified" )]
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the creation of the corresponding raw data object.
		/// </summary>
		/// <remarks>
		/// The attribute <code>Created</code> will be set by the Rawdata-Service.
		/// A user value will be ignored.
		/// </remarks>
		[JsonPropertyName( "created" )]
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets the size of the raw data object in bytes.
		/// </summary>
		[JsonPropertyName( "size" )]
		public int Size { get; set; }

		/// <summary>
		/// Gets or sets the MD5-Hash of the raw data object.
		/// </summary>
		[JsonPropertyName( "md5" )]
		public Guid MD5 { get; set; }

		/// <summary>
		/// Gets or sets the MD5-Hash of the raw data object.
		/// </summary>
		/// <remarks>
		/// This property exists for serialization compatibility reasons only. This will be removed in a future version of this interface.
		/// </remarks>
		[JsonIgnore]
		public string MD5String
		{
			get => MD5GuidToString( MD5 );
			set => MD5 = MD5StringToGuid( value );
		}

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{Key}: {FileName} ({MimeType})";
		}

		public static Guid MD5StringToGuid( string md5 )
		{
			if( string.IsNullOrWhiteSpace( md5 ) || md5.Length != 32 )
				return Guid.Empty;

			md5 = md5.ToUpperInvariant();

			var result = new byte[ 16 ];
			var upper = true;
			var current = 0;

			for( var i = 0; i < md5.Length; i += 1 )
			{
				var num = (int)md5[ i ];
				num -= 0x30;
				if( num >= 10 ) num -= ( 0x41 - 0x30 - 10 );
				if( num > 15 ) return Guid.Empty;
				if( upper ) result[ current ] |= (byte)( num << 4 );
				else
				{
					result[ current ] |= (byte)( num & 15 );
					current += 1;
				}

				upper = !upper;
			}

			return new Guid( result );
		}

		public static string MD5GuidToString( Guid md5 )
		{
			if( md5 == Guid.Empty )
				return null;

			var data = md5.ToByteArray();
			var chars = new char[ 2 * data.Length ];
			var counter = 0;

			foreach( var b in data )
			{
				chars[ counter++ ] = "0123456789abcdef"[ b >> 4 ];
				chars[ counter++ ] = "0123456789abcdef"[ b & 15 ];
			}

			return new string( chars );
		}

		#endregion
	}
}