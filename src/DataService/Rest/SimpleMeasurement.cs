#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.DataService.Rest
{
	#region using

	using System;
	using System.Diagnostics;
	using System.Xml;
	using Newtonsoft.Json;
	using JetBrains.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Data;
	using Zeiss.IMT.PiWeb.Api.Common.Data.Converter;

	#endregion

	/// <summary>
	/// This class represents the base class for measurements with its attributes. The concrete class for measurements that also contains the measurement values is <see cref="DataMeasurement"/>.
	/// A measurement is identified by an <see cref="Uuid"/>. A measurement always belongs to one and only one part.
	/// </summary>
	[DebuggerDisplay( "Measurement (Uuid={Uuid} Time={Time})" )]
	public class SimpleMeasurement : IAttributeItem
	{
		#region contants

		private static readonly Attribute[] EmptyAttributeList = new Attribute[ 0 ];

		#endregion
		
		#region members

		private static readonly DateTime MinimumValidDatabaseDateTime = DateTime.SpecifyKind( System.Data.SqlTypes.SqlDateTime.MinValue.Value, DateTimeKind.Utc );

		private Attribute[] _Attributes = EmptyAttributeList;
		private DateTime? _CachedTimeValue;
		private bool _HasCachedTime;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the uuid of this measurement.
		/// </summary>
		[JsonProperty( "uuid" )]
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the uuid the part this measurement belongs to.
		/// </summary>
		[JsonProperty( "partUuid" )]
		public Guid PartUuid { get; set; }

		/// <summary>
		/// Gets or sets the last modification timestamp of this measurement. The server will update this 
		/// timestamp whenever an attribute of this measurement is changed or whenever measurement values
		/// of this measurement are updated, deleted and added.
		/// </summary>
		[JsonProperty( "lastModified" )]
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets or sets the creation timestamp of this measurement. 
		/// </summary>
		[JsonProperty( "created" )]
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets all attributes that belong to this measurement.
		/// </summary>
		[JsonProperty( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		public Attribute[] Attributes
		{
			[NotNull]
			get { return _Attributes; }
			set
			{
				_Attributes = value ?? EmptyAttributeList;
				_CachedTimeValue = null;
				_HasCachedTime = false;
			}
		}

		/// <summary>
		/// Gets or sets the status information for this measurement. This status information can be requested when 
		/// performing a measurement search using one of the values from <see cref="MeasurementStatistics"/>.
		/// </summary>
		[JsonProperty( "status" )]
		public SimpleMeasurementStatus[] Status { get; set; }

		/// <summary>
		/// Gets or sets the time of this measurement. If this measurement has no time attribute, then <see cref="MinimumValidDatabaseDateTime"/> will
		/// be returned.
		/// </summary>
		[JsonIgnore]
		public DateTime? Time
		{
			[DebuggerStepThrough]
			get
			{
				if( _HasCachedTime ) 
					return _CachedTimeValue;

				try
				{
					var att = this.GetAttribute( WellKnownKeys.Measurement.Time );
					if( att != null && !string.IsNullOrEmpty( att.Value ) )
						_CachedTimeValue = XmlConvert.ToDateTime( att.Value, XmlDateTimeSerializationMode.RoundtripKind );
				}
				catch { }

				_HasCachedTime = true;

				return _CachedTimeValue;
			}
			set
			{
				if( value == null )
				{
					this.RemoveAttribute( WellKnownKeys.Measurement.Time );
					_CachedTimeValue = null;
					_HasCachedTime = true;
				}
				else
				{
					this.SetAttribute( new Attribute( WellKnownKeys.Measurement.Time, XmlConvert.ToString( value.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );
					_CachedTimeValue = value;
					_HasCachedTime = true;
				}
			}
		}

		/// <summary>
		/// Returns the measurement time and in case of no time is specified, the minimum time allowed (<see cref="System.Data.SqlTypes.SqlDateTime.MinValue"/>).
		/// </summary>
		[JsonIgnore]
		public DateTime TimeOrMinDate => Time ?? MinimumValidDatabaseDateTime;

		/// <summary>
		/// Returns the measurement time and in case of no time is specified, the creation date of the measurement.
		/// </summary>
		[JsonIgnore]
		public DateTime TimeOrCreationDate => Time ?? Created;

		#endregion

		#region methods

		/// <summary>
		/// Overridden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return string.Format( "'{0}' [{1}]", Time, Uuid );
		}

		#endregion
	}
}
