#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Common.Utilities
{
	#region usings

	using System;
	using System.Security.Cryptography.X509Certificates;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Credential that contains certificate information.
	/// </summary>
	public sealed class CertificateCredential : ICredential
	{
		#region constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="certificate"></param>
		public CertificateCredential( [NotNull] X509Certificate2 certificate )
		{
			Certificate = certificate ?? throw new ArgumentNullException( nameof( certificate ) );
			DisplayId = CertificateHelper.ThumbPrintToFriendlyText( certificate.Thumbprint );
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns the chosen certificate.
		/// </summary>
		[NotNull]
		public X509Certificate2 Certificate { get; }

		#endregion

		#region methods

		public static CertificateCredential CreateFromCertificate( X509Certificate2 certificate )
		{
			if( certificate == null )
				return null;

			return new CertificateCredential( certificate );
		}

		public bool Equals( CertificateCredential other )
		{
			return other != null
			       && Equals( Certificate, other.Certificate )
			       && string.Equals( DisplayId, other.DisplayId );
		}

		public override bool Equals( object other ) => Equals( other as CertificateCredential );

		public override int GetHashCode()
		{
			unchecked
			{
				return ( Certificate.GetHashCode() * 397 ) ^ ( DisplayId != null ? DisplayId.GetHashCode() : 0 );
			}
		}

		#endregion

		#region interface ICredential

		/// <summary>
		/// Return a text that can be used for displaying.
		/// </summary>
		[CanBeNull]
		public string DisplayId { get; }

		public bool Equals( ICredential other ) => Equals( other as CertificateCredential );

		#endregion
	}
}