#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.RawData
{
	#region using

	using System;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This object holds information for a raw data object like its size, key, name etc.
	/// </summary>
	[Serializable]
	public class RawDataInformation
	{
		#region constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public RawDataInformation()
		{
			Target = new RawDataTargetEntity();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="entity">The entity this object belongs to.</param>
		/// <param name="key">A unique key that identifies this information object.</param>
		public RawDataInformation( RawDataTargetEntity entity, int key )
		{
			Target = entity;
			Key = key;
		}

		/// <summary>
		/// Copy-Constructor.
		/// </summary>
		/// <param name="data">The raw data information object that should be copied.</param>
		public RawDataInformation( RawDataInformation data )
		{
			if( data == null ) throw new ArgumentNullException( "data" );

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
		[JsonProperty( "target" )]
		public RawDataTargetEntity Target { get; set; }

		/// <summary>
		/// This is a unique key that identifies this specific raw data object for a corresponding entity. An entity (Part, Characteristic, Measurement, Value)
		/// can have multiple raw data object that are distinct by this key.
		/// </summary>
		[JsonProperty( "key" )]
		public int? Key { get; set; }

		/// <summary>
		/// Gets or sets the filename of the raw data object.
		/// </summary>
		/// <remarks>
		/// Please note that this filename is not unique (unlike filenames in traditional file systems).
		/// </remarks>
		[JsonProperty( "fileName" )]
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the mime type of the raw data object.
		/// </summary>
		[JsonProperty( "mimeType" )]
		public string MimeType { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the last modification of the corresponding raw data object.
		/// </summary>
		/// <remarks>
		/// The attribute <code>LastModified</code> will be set by the Rawdata-Service.
		/// A user value will be ignored. 
		/// </remarks>
		[JsonProperty( "lastModified" )]
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets or sets the timestamp of the creation of the corresponding raw data object.
		/// </summary>
		/// <remarks>
		/// The attribute <code>Created</code> will be set by the Rawdata-Service.
		/// A user value will be ignored. 
		/// </remarks>
		[JsonProperty( "created" )]
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets the size of the raw data object in bytes.
		/// </summary>
		[JsonProperty( "size" )]
		public int Size { get; set; }

		/// <summary>
		/// Gets or sets the MD5-Hash of the raw data object.
		/// </summary>
		[JsonProperty( "md5" )]
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
			get { return MD5GuidToString( MD5 ); }
			set { MD5 = MD5StringToGuid( value ); }
		}

		#endregion

		#region methods

		/// <summary>
		/// Overriden <see cref="Object.ToString"/>-method.
		/// </summary>
		public override string ToString()
		{
			return string.Format( "{0}: {1} ({2})", Key, FileName, MimeType );
		}

		public static Guid MD5StringToGuid( string md5 )
		{
			if( string.IsNullOrWhiteSpace( md5 ) || md5.Length != 32 )
				return Guid.Empty;

			md5 = md5.ToUpperInvariant();

			var result = new byte[16];
			bool upper = true;
			int current = 0;

			for( int i = 0; i < md5.Length; i += 1 )
			{
				var num = (int)md5[i];
				num -= 0x30;
				if( num >= 10 ) num -= ( 0x41 - 0x30 - 10 );
				if( num > 15 ) return Guid.Empty;
				if( upper ) result[current] |= (byte)( num << 4 );
				else
				{
					result[current] |= (byte)( num & 15 );
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
			var chars = new char[2 * data.Length];
			int counter = 0;

			foreach( var b in data )
			{
				chars[counter++] = "0123456789abcdef"[b >> 4];
				chars[counter++] = "0123456789abcdef"[b & 15];
			}
			return new string( chars );
		}

		#endregion
	}
}