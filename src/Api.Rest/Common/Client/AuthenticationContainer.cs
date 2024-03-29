﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Net;
	using System.Security.Cryptography.X509Certificates;
	using Zeiss.PiWeb.Api.Rest.Common.Utilities;

	#endregion

	public sealed class AuthenticationContainer : IEquatable<AuthenticationContainer>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationContainer" /> class.
		/// </summary>
		public AuthenticationContainer( AuthenticationMode authenticationMode )
		{
			Mode = authenticationMode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationContainer" /> class.
		/// </summary>
		public AuthenticationContainer( NetworkCredential credentials )
		{
			Mode = AuthenticationMode.NoneOrBasic;
			Credentials = credentials;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationContainer" /> class.
		/// </summary>
		public AuthenticationContainer( X509Certificate2 certificate )
		{
			Mode = AuthenticationMode.Certificate;
			Certificate = certificate;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationContainer" /> class.
		/// </summary>
		public AuthenticationContainer( string oAuthAccessToken )
		{
			Mode = AuthenticationMode.OAuth;
			OAuthAccessToken = oAuthAccessToken;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationContainer" /> class.
		/// </summary>
		public AuthenticationContainer( AuthenticationMode authenticationMode, NetworkCredential credentials )
		{
			Mode = authenticationMode;
			Credentials = credentials;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationContainer" /> class.
		/// </summary>
		public AuthenticationContainer( AuthenticationMode authenticationMode, X509Certificate2 certificate )
		{
			Mode = authenticationMode;
			Certificate = certificate;
		}

		#endregion

		#region properties

		public AuthenticationMode Mode { get; }

		public NetworkCredential Credentials { get; }

		public string OAuthAccessToken { get; }

		public X509Certificate2 Certificate { get; }

		#endregion

		#region methods

		public static AuthenticationContainer FromUsernamePasswordCredential( UsernamePasswordCredential credential )
		{
			return credential != null
				? new AuthenticationContainer( credential.ToNetworkCredential() )
				: null;
		}

		public static AuthenticationContainer FromCertificateCredential( CertificateCredential credential )
		{
			return credential != null
				? new AuthenticationContainer( credential.Certificate )
				: null;
		}

		public static AuthenticationContainer FromOAuthTokenCredential( OAuthTokenCredential credential )
		{
			return credential != null
				? new AuthenticationContainer( credential.AccessToken )
				: null;
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return Equals( obj as AuthenticationContainer );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return (int)Mode;
		}

		/// <summary>
		/// Indicates whether the values of two specified <see cref="AuthenticationContainer" /> instances are equal.
		/// </summary>
		/// <param name="a">The first <see cref="AuthenticationContainer" /> to compare.</param>
		/// <param name="b">The seconds <see cref="AuthenticationContainer" /> to compare.</param>
		/// <returns>
		/// Returns <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> points to the exactly
		/// same <see cref="AuthenticationContainer"/>; Otherwise returns <see langword="false" />.
		/// </returns>
		public static bool operator ==( AuthenticationContainer a, AuthenticationContainer b )
		{
			return Equals( a, b );
		}

		/// <summary>
		/// Indicates whether the values of two specified <see cref="AuthenticationContainer" /> instances are not equal.
		/// </summary>
		/// <param name="a">The first <see cref="AuthenticationContainer" /> to compare.</param>
		/// <param name="b">The seconds <see cref="AuthenticationContainer" /> to compare.</param>
		/// <returns>
		/// Returns <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> points to different
		/// authentication container; Otherwise returns <see langword="false" />.
		/// </returns>
		public static bool operator !=( AuthenticationContainer a, AuthenticationContainer b )
		{
			return !Equals( a, b );
		}

		private static bool CredentialEquals( NetworkCredential a, NetworkCredential b )
		{
			if( ReferenceEquals( a, b ) ) return true;
			if( a == null || b == null ) return false;

			return Equals( a.UserName, b.UserName )
					&& Equals( a.Password, b.Password );
		}

		private static bool CertificateEquals( X509Certificate2 a, X509Certificate2 b )
		{
			if( ReferenceEquals( a, b ) ) return true;
			if( a == null || b == null ) return false;

			return Equals( a.Thumbprint, b.Thumbprint );
		}

		#endregion

		#region interface IEquatable<AuthenticationContainer>

		/// <inheritdoc />
		public bool Equals( AuthenticationContainer other )
		{
			return other != null
					&& Equals( Mode, other.Mode )
					&& CredentialEquals( Credentials, other.Credentials )
					&& CertificateEquals( Certificate, other.Certificate )
					&& Equals( OAuthAccessToken, other.OAuthAccessToken );
		}

		#endregion
	}
}