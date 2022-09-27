#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Possible execution statuses of a long running operation.
	/// </summary>
	[JsonConverter( typeof( JsonStringEnumConverter ) )]
	public enum OperationExecutionStatusDto
	{
		/// <summary>
		/// Operation is currently running.
		/// </summary>
		Running,

		/// <summary>
		/// Operation has finished.
		/// </summary>
		Finished,

		/// <summary>
		/// Operation has finished with an exception.
		/// </summary>
		Exception
	}
}