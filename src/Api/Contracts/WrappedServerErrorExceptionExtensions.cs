#region copyright
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
// /* Carl Zeiss IMT (IZfM Dresden)                   */
// /* Softwaresystem PiWeb                            */
// /* (c) Carl Zeiss 2017                             */
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Contracts
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Net;

	#endregion

	public static class WrappedServerErrorExceptionExtensions
	{
		/// <summary>
		/// Contains mappings of HTTP statuscodes to .NET based exceptions for server based exceptions (500 HTTP status codes)
		/// </summary>
		public static IReadOnlyDictionary<HttpStatusCode, string> ServerBasedExceptions => InternalServerBasedExceptions;

		private static readonly Dictionary<HttpStatusCode, string> InternalServerBasedExceptions = new Dictionary<HttpStatusCode, string>
		{
			{ HttpStatusCode.InternalServerError, "System.ServiceModel.CommunicationException" },
			{ HttpStatusCode.NotImplemented, typeof( NotImplementedException ).ToString() },
			{ HttpStatusCode.BadGateway, "System.ServiceModel.EndpointNotFoundException" },
			{ HttpStatusCode.ServiceUnavailable, "System.ServiceModel.ServiceActivationException" },
			{ HttpStatusCode.GatewayTimeout, typeof( TimeoutException ).ToString() }
		};

		/// <summary>
		/// Indicates if <paramref name="exception"/> includes an <c>System.ServiceModel.EndpointNotFoundException</c>
		/// </summary>
		public static bool IncludesNotFoundException( this WrappedServerErrorException exception )
		{
			return string.Equals( exception?.Error?.ExceptionType, "System.EndpointNotFoundException", StringComparison.Ordinal );
		}

		/// <summary>
		/// Indicates if <paramref name="exception"/> includes an exception of type <typeparam name="T" />
		/// </summary>
		public static bool IncludesExceptionOfType<T>( this WrappedServerErrorException exception )
		{
			return string.Equals( exception?.Error?.ExceptionType, typeof( T ).ToString(), StringComparison.Ordinal );
		}

		/// <summary>
		/// Indicates if <paramref name="exception"/> includes an exception from <see cref="ServerBasedExceptions"/>
		/// and so is server based.
		/// </summary>
		public static bool IsServerBasedFault( this WrappedServerErrorException exception )
		{
			return exception?.Error?.ExceptionType != null && InternalServerBasedExceptions.ContainsValue( exception.Error.ExceptionType );
		}

		/// <summary>
		/// Indicates if <paramref name="exception"/> does not include an exception from <see cref="ServerBasedExceptions"/>
		/// and so is client based.
		/// </summary>
		public static bool IsClientBasedFault( this WrappedServerErrorException exception )
		{
			return !exception.IsServerBasedFault();
		}
	}
}