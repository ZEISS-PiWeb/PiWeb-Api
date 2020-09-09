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

	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	#endregion

	/// <summary>
	/// Holds attributes' possible data types.
	/// </summary>
	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum AttributeTypeDto
	{
		/// <summary>
		/// A string value.
		/// </summary>
		AlphaNumeric,

		/// <summary>
		/// Represents singned numbers which don't have any fractional digits in its lexical or value spaces.
		/// Legal examples are: <code>12, -38</code>
		/// </summary>
		Integer,

		/// <summary>
		/// Represents IEEE simple (32 bits) and double (64 bits) precision floating-point types.
		/// Legal examples are: <code>-5E2, 1.35, 5.6e-2</code>.
		/// </summary>
		Float,

		/// <summary>
		/// Describes combination of a date and a time. Its lexical space is the extended format
		/// "CCYY-MM-DDThh:mm:ssZ".  The letter "T" is the date/time separator.
		/// The timezone is specified as "Z" (UTC).
		/// </summary>
		DateTime
	}
}