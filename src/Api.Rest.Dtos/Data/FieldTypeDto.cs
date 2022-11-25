#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// These are the possible data types of attributes.
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( Newtonsoft.Json.Converters.StringEnumConverter ) )]
	[JsonConverter( typeof( JsonStringEnumConverter ) )]
	public enum FieldTypeDto
	{
		/// <summary>
		/// AlphaNumeric represents a string value.
		/// </summary>
		AlphaNumeric,

		/// <summary>
		/// Integer is a signed 32-bit type.
		/// </summary>
		Integer,

		/// <summary>
		/// Float corresponds to the IEEE single-precision 32-bit floating point type. The following
		/// xamples are legal literals for Float: <code>-1E4, 1267.43233E12, 12.78e-2, 12</code>.
		/// </summary>
		Float,

		/// <summary>
		/// DateTime represents a specific instant of time. The lexical representation has to be
		/// in the format "CCYY-MM-DDThh:mm:ss". The letter "T" is the date/time separator. "ss" may
		/// contain fractional seconds (like "ss.ssss...") to increase the precision.
		/// </summary>
		DateTime
	}
}