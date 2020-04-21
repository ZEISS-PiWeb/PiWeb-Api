#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Linq;
	using System.Security.Cryptography.X509Certificates;

	#endregion

	public static class CertificateHelper
	{
		#region methods

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

		#endregion
	}
}