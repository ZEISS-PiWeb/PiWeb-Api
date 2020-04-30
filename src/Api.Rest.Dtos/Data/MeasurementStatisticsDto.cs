#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	/// <summary>
	/// Enumeration that specifies what additional statistical information should be returned with each measurement.
	/// </summary>
	public enum MeasurementStatisticsDto
	{
		/// <summary>
		/// No additional statistical information is calculated.
		/// </summary>
		None,

		/// <summary>
		/// Statistical information should be returned. This information includes:
		/// * Number of characteristics out of tolerance.
		/// * Number of characteristics out of warning limits.
		/// * Number of characteristics in warning limits and in tolerance.
		/// </summary>
		Simple,

		/// <summary>
		/// Statistical information should be returned. This information includes:
		/// In addition the the information returned by <see cref="MeasurementStatisticsDto.Simple"/>,
		/// the uuids of the characteristics that belong to these groups will be returned.
		/// </summary>
		Detailed
	}
}