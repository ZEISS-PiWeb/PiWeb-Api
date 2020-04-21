#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System.Net;
	using System.Security.Cryptography.X509Certificates;
	using Zeiss.PiWeb.Api.Rest.Common.Utilities;

	#endregion

	public class AuthenticationContainer
	{
		#region constructors

		public AuthenticationContainer( AuthenticationMode authenticationMode )
		{
			Mode = authenticationMode;
		}

		public AuthenticationContainer( NetworkCredential credentials )
		{
			Mode = AuthenticationMode.NoneOrBasic;
			Credentials = credentials;
		}

		public AuthenticationContainer( X509Certificate2 certificate )
		{
			Mode = AuthenticationMode.Certificate;
			Certificate = certificate;
		}

		public AuthenticationContainer( string oAuthAccessToken )
		{
			Mode = AuthenticationMode.OAuth;
			OAuthAccessToken = oAuthAccessToken;
		}

		public AuthenticationContainer( AuthenticationMode authenticationMode, NetworkCredential credentials )
		{
			Mode = authenticationMode;
			Credentials = credentials;
		}

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

		public override bool Equals( object obj )
		{
			var other = obj as AuthenticationContainer;

			return other != null
			       && Equals( Mode, other.Mode )
			       && CredentialEquals( Credentials, other.Credentials )
			       && CertificateEquals( Certificate, other.Certificate )
			       && Equals( OAuthAccessToken, other.OAuthAccessToken );
		}

		public override int GetHashCode() => (int) Mode;

		public static bool operator ==( AuthenticationContainer a, AuthenticationContainer b )
		{
			return Equals( a, b );
		}

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
	}
}