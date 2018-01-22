#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Security.Cryptography.X509Certificates;

	#endregion

	public static class CertificateHelper
	{
		#region methods

		/// <summary>
		/// Determines if the given certificate has a valid certificate chain.
		/// </summary>
		public static bool HasValidChain( X509Certificate2 certificate, X509RevocationMode revocationMode = X509RevocationMode.NoCheck )
		{
			var chain = new X509Chain
			{
				ChainPolicy =
				{
					RevocationFlag = X509RevocationFlag.EntireChain,
					RevocationMode = revocationMode
				}
			};

			return chain.Build( certificate );
		}

		/// <summary>
		/// Determines if the given certificate is certificate authority - CA.
		/// </summary>
		public static bool IsCertificateAuthority( X509Certificate2 certificate )
		{
			foreach( var basicConstraintsExtension in EnumEnhancedKeyUsageExtensions( certificate, "2.5.29.19" ).OfType<X509BasicConstraintsExtension>() )
			{
				if( basicConstraintsExtension.CertificateAuthority )
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines if the given certificate is valid for server authentication (extended key usage "1.3.6.1.5.5.7.3.1").
		/// </summary>
		public static bool IsServerAuthenticationCertificate( X509Certificate2 certificate )
		{
			if( certificate == null ) return false;

			foreach( var enhancedKeyUsageOid in EnumEnhancedKeyUsageOids( certificate ) )
			{
				// Server Authentication
				if( enhancedKeyUsageOid.Value == "1.3.6.1.5.5.7.3.1" )
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines if the given certificate is valid for client authentication (extended key usage "1.3.6.1.5.5.7.3.2").
		/// </summary>
		public static bool IsClientAuthenticationCertificate( X509Certificate2 certificate )
		{
			if( certificate == null ) return false;

			foreach( var enhancedKeyUsageOid in EnumEnhancedKeyUsageOids( certificate ) )
			{
				// client authentication (1.3.6.1.5.5.7.3.2)
				if( enhancedKeyUsageOid.Value == "1.3.6.1.5.5.7.3.2" )
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Detemines if the given certificate is a hardware certificate.
		/// </summary>
		public static bool IsHardwareCertificate( X509Certificate2 certificate )
		{
			if( certificate == null ) return false;

			foreach( var enhancedKeyUsageOid in EnumEnhancedKeyUsageOids( certificate ) )
			{
				// Smartcard-Anmeldung (1.3.6.1.4.1.311.20.2.2)
				if( enhancedKeyUsageOid.Value == "1.3.6.1.4.1.311.20.2.2" )
				{
					return true;
				}
			}

			try
			{
				var privateKey = certificate.PrivateKey as ICspAsymmetricAlgorithm;
				if( privateKey != null && privateKey.CspKeyContainerInfo.HardwareDevice ) return true;
			}
			catch( Exception )
			{
				// ignored
			}

			return false;
		}

		public static string ThumbPrintToFriendlyText( string thumbPrint )
		{
			if( string.IsNullOrEmpty( thumbPrint ) )
				return null;

			var certificate =
				FindCertificateByThumbprint( thumbPrint, StoreName.My, StoreLocation.CurrentUser, false ) ??
				FindCertificateByThumbprint( thumbPrint, StoreName.My, StoreLocation.LocalMachine, false );

			if( certificate == null )
				return thumbPrint;

			var nameCn = certificate.Subject
				.Split( ',' )
				.Select( s => s.Trim() )
				.SingleOrDefault( s => s.StartsWith( "CN", StringComparison.OrdinalIgnoreCase ) );

			var issuerCn = certificate.Issuer
				.Split( ',' )
				.Select( s => s.Trim() )
				.SingleOrDefault( s => s.StartsWith( "CN", StringComparison.OrdinalIgnoreCase ) );

			var name = nameCn?.Split( '=' )[ 1 ] ?? certificate.Subject;
			var issuer = issuerCn?.Split( '=' )[ 1 ] ?? certificate.Issuer;

			return $"{name} ({issuer}), {certificate.NotBefore:d} - {certificate.NotAfter:d}";
		}


		public static X509Certificate2 FindCertificateByThumbprint( string thumbprint, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool onlyValidCertificates = true )
		{
			if( string.IsNullOrEmpty( thumbprint ) ) return null;

			var storeCerts = GetCertificatesFromStoreInternal( storeName, storeLocation ).Find( X509FindType.FindByThumbprint, thumbprint, onlyValidCertificates );

			return storeCerts.Count == 1 ? storeCerts[ 0 ] : null;
		}

		public static ICollection<X509Certificate2> GetCertificatesFromStore( StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser, bool onlyFindByTimeValid = true )
		{
			var certificates = GetCertificatesFromStoreInternal( storeName, storeLocation );

			return onlyFindByTimeValid ?
				certificates.Find( X509FindType.FindByTimeValid, DateTime.UtcNow, false ).OfType<X509Certificate2>().ToList() :
				certificates.OfType<X509Certificate2>().ToList();
		}

		public static X509Certificate2 SelectOneCertficate( IEnumerable<X509Certificate2> certificates, string title = null, string message = null, IntPtr? owner = null )
		{
			return SelectCertificates( certificates, title, message, X509SelectionFlag.SingleSelection, owner ).SingleOrDefault();
		}

		public static ICollection<X509Certificate2> SelectCertificates( IEnumerable<X509Certificate2> certificates, string title = null, string message = null, X509SelectionFlag selectionFlag = X509SelectionFlag.SingleSelection, IntPtr? owner = null )
		{
			if( certificates == null ) return new List<X509Certificate2>();

			var certificateCollection = new X509Certificate2Collection( certificates.ToArray() );

			// Fenster
			var selectedCertificates = X509Certificate2UI.SelectFromCollection( certificateCollection, title, message, selectionFlag, owner ?? IntPtr.Zero );

			var result = selectedCertificates.OfType<X509Certificate2>().ToList();

			if( selectionFlag == X509SelectionFlag.SingleSelection )
			{
				return result.Count == 1 ? result : new List<X509Certificate2>();
			}

			return result;
		}

		public static void ShowDetailsOfCertficate( X509Certificate2 certificate, IntPtr? owner = null )
		{
			X509Certificate2UI.DisplayCertificate( certificate, owner ?? IntPtr.Zero );
		}

		private static X509Certificate2Collection GetCertificatesFromStoreInternal( StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser )
		{
			X509Store store = null;

			try
			{
				store = new X509Store( storeName, storeLocation );
				store.Open( OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly );
				return store.Certificates;
			}
			finally
			{
				store?.Close();
			}
		}

		private static IEnumerable<Oid> EnumEnhancedKeyUsageOids( X509Certificate2 certificate )
		{
			foreach( var enhancedKeyUsageExtension in EnumEnhancedKeyUsageExtensions( certificate ).OfType<X509EnhancedKeyUsageExtension>() )
			{
				foreach( var enhancedKeyUsageOid in enhancedKeyUsageExtension.EnhancedKeyUsages.OfType<Oid>() )
				{
					yield return enhancedKeyUsageOid;
				}
			}
		}

		private static IEnumerable<X509Extension> EnumEnhancedKeyUsageExtensions( X509Certificate2 certificate, string oid = "2.5.29.37" )
		{
			if( certificate == null ) yield break;

			foreach( var enhancedKeyUsageExtension in certificate.Extensions.OfType<X509Extension>().Where( e => e.Oid != null && e.Oid.Value == oid ) )
			{
				yield return enhancedKeyUsageExtension;
			}
		}

		#endregion
	}
}