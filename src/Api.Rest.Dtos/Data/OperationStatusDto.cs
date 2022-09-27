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

	using System;
	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Contains information about the execution status of a long running operation.
	/// </summary>
	public class OperationStatusDto
	{
		#region properties

		/// <summary>
		/// Uuid to identify the operation.
		/// </summary>
		public Guid OperationUuid { get; set; }

		/// <summary>
		/// Status of execution, e.g. Running, Finished or Exception.
		/// </summary>
		public OperationExecutionStatusDto ExecutionStatus { get; set; }

		/// <summary>
		/// Thrown exception during execution, wrapped in an error object.
		/// </summary>
		[JsonIgnore( Condition = JsonIgnoreCondition.Never )]
		public Error Exception { get; set; }

		#endregion
	}
}