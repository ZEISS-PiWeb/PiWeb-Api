#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Security.Cryptography.X509Certificates;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Credential that contains certificate information.
	/// </summary>
	public sealed class CertificateCredential : ICredential, IEquatable<CertificateCredential>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateCredential"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="certificate"/> is <see langword="true" />.</exception>
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
			return certificate == null ? null : new CertificateCredential( certificate );
		}


		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return ( Certificate.GetHashCode() * 397 ) ^ ( DisplayId != null ? DisplayId.GetHashCode() : 0 );
			}
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return Equals( obj as CertificateCredential );
		}

		#endregion

		#region interface ICredential

		/// <inheritdoc />
		[CanBeNull]
		public string DisplayId { get; }

		/// <inheritdoc />
		public bool Equals( ICredential other )
		{
			return Equals( other as CertificateCredential );
		}

		#endregion

		#region interface IEquatable<CertificateCredential>

		/// <inheritdoc />
		public bool Equals( CertificateCredential other )
		{
			return other != null
					&& Equals( Certificate, other.Certificate )
					&& string.Equals( DisplayId, other.DisplayId );
		}

		#endregion
	}
}