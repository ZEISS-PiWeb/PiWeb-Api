#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	#endregion

	/// <summary>
	/// Die Enumeration definiert die Bedingung, die im Rahmen einer Messungssuche
	/// beim Koppeln von Messungen verschiedener Teile anhand eines primären Messungsschlüssels
	/// für jeden im Suchergebnis enthaltenen Attributwert des Schlüssels erfüllt sein muss.
	/// </summary>
	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum MeasurementMergeCondition
	{
		/// <summary>
		/// Es ist egal, in wievielen der durchsuchten Teile
		/// Messungen mit dem gleichen Attributwert des primären Messungsschlüssels vorkommen.
		/// </summary>
		None,

		/// <summary>
		/// In mindestens 2 Teilen müssen Messungen mit dem gleichen Attributwert des primären Messungsschlüssels vorkommen.
		/// </summary>
		MeasurementsInAtLeastTwoParts, // oder "MeasurementsInMultipleParts"?

		/// <summary>
		/// In allen Teilen müssen Messungen mit dem gleichen Attributwert des primären Messungsschlüssels vorkommen.
		/// </summary>
		MeasurementsInAllParts
	}
}