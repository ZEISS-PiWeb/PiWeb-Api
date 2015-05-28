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
	using System.Diagnostics;
	using System.Xml;
	using Common.Data;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This element describes the entity Measurement without any measurement values.
	/// Like all other entities, the Measurement entity has an unique identifier, which is used as a primary key,
	/// a type description, and zero or more attributes.
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

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the uuid of this measurement.
		/// </summary>
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the uuid the part this measurement belongs to.
		/// </summary>
		public Guid PartUuid { get; set; }

		/// <summary>
		/// Gets or sets the last modification timestamp of this measurement. The server will update this 
		/// timestamp whenever an attribute of this measurement is changed or whenever measurement values
		/// of this measurement are updated, deleted and added.
		/// </summary>
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets or sets all attributes that belong to this measurement.
		/// </summary>
		public Attribute[] Attributes
		{
			get { return _Attributes; }
			set { _Attributes = value ?? EmptyAttributeList; _CachedTimeValue = null; }
		}

		/// <summary>
		/// Gets or sets the status information for this measurement. This status information can be requested when 
		/// performing a measurement search using one of the values from <see cref="MeasurementStatistics"/>.
		/// </summary>
		public SimpleMeasurementStatus[] Status { get; set; }

		/// <summary>
		/// Gets or sets the time of this measurement. If this measurement has no time attribute, then <see cref="MinimumValidDatabaseDateTime"/> will
		/// be returned.
		/// </summary>
		[JsonIgnore]
		public DateTime Time
		{
			[DebuggerStepThrough]
			get
			{
				if( _CachedTimeValue.HasValue ) 
					return _CachedTimeValue.Value;

				try
				{
					_CachedTimeValue = MinimumValidDatabaseDateTime;

					var att = this.GetAttribute( WellKnownKeys.Measurement.Time );
					if( att != null && !string.IsNullOrEmpty( att.Value ) )
						_CachedTimeValue = XmlConvert.ToDateTime( att.Value, XmlDateTimeSerializationMode.RoundtripKind );
				}
				catch { }

				return _CachedTimeValue.GetValueOrDefault( MinimumValidDatabaseDateTime );
			}
			set
			{
				if( value == MinimumValidDatabaseDateTime )
				{
					this.RemoveAttribute( WellKnownKeys.Measurement.Time );
				}
				else
				{
					this.SetAttribute( new Attribute( WellKnownKeys.Measurement.Time, XmlConvert.ToString( value, XmlDateTimeSerializationMode.RoundtripKind ) ) );
					_CachedTimeValue = value;
				}
			}
		}

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
