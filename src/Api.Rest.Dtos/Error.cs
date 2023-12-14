#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos
{
	#region usings

	using System;
	using System.Xml.Serialization;

	#endregion

	/// <summary> 
	/// Class holds an error which can occur on calling a webservice method. 
	/// </summary>
	[XmlType( Namespace = "http://www.zeiss.com/CmmService" )]
	public class Error
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Error"/> class.
		/// </summary>
		public Error()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Error"/> class.
		/// </summary>
		public Error( string message )
		{
			Message = message;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Error"/> class.
		/// </summary>
		public Error( Exception ex )
		{
			Message = ex.Message;

			if( ex.InnerException != null )
				InnerError = new Error( ex.InnerException );

			ExceptionType = ex.GetType().ToString();

			StackTrace = ex.StackTrace;
		}

		#endregion

		#region properties

		/// <summary> 
		/// Returns the error description message.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// The exception message that might be generated on the server side.
		/// </summary>
		public string ExceptionMessage { get; set; }

		/// <summary>
		/// The type of exception that might be generated on the server side.
		/// </summary>
		public string ExceptionType { get; set; }

		/// <summary>
		/// The stack trace of the exception that might be generated on the server side.
		/// </summary>
		public string StackTrace { get; set; }

		/// <summary>
		/// The nested <see cref="Error"/> instance that triggered the current error.
		/// </summary>
		public Error InnerError { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return !string.IsNullOrEmpty( ExceptionMessage ) ? $"{Message}: {ExceptionMessage}" : Message;
		}

		#endregion
	}
}