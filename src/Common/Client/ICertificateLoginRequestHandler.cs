#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Security.Cryptography.X509Certificates;
	using System.Threading.Tasks;
	using Zeiss.IMT.PiWeb.Api.Common.Utilities;

	#endregion

	public interface ICertificateLoginRequestHandler : ICacheClearable
	{
		#region properties

		ISet<StoreLocation> CertificateStoreLocations { get; }

		#endregion

		#region methods

		Task<CertificateCredential> CertificateRequestAsync( Uri uri, bool preferHardwareCertificate = false );

		CertificateCredential CertificateRequest( Uri uri, bool preferHardwareCertificate = false );

		#endregion
	}
}