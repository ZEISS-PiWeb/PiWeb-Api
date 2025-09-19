// Source: https://github.com/dotnet/aspnetcore/blob/c2a442982e736e17ae6bcadbfd8ccba278ee1be6/src/HealthChecks/Abstractions/src/HealthStatus.cs

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// The referenced license file can be found here: https://github.com/dotnet/aspnetcore/blob/main/LICENSE.txt

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Health;

/// <summary>
/// Represents the reported status of a health check result.
/// </summary>
/// <remarks>
/// <para>
/// A status of <see cref="Unhealthy"/> should be considered the default value for a failing health check. Application
/// developers may configure a health check to report a different status as desired.
/// </para>
/// <para>
/// The values of this enum or ordered from least healthy to most healthy. So <see cref="HealthCheckResultType.Degraded"/> is
/// greater than <see cref="HealthCheckResultType.Unhealthy"/> but less than <see cref="HealthCheckResultType.Healthy"/>.
/// </para>
/// </remarks>
/// <seealso cref="Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus"/>
public enum HealthCheckResultType
{
	/// <summary>
	/// Indicates that the health check determined that the component was unhealthy, or an unhandled
	/// exception was thrown while executing the health check.
	/// </summary>
	Unhealthy = 0,

	/// <summary>
	/// Indicates that the health check determined that the component was in a degraded state.
	/// </summary>
	Degraded = 1,

	/// <summary>
	/// Indicates that the health check determined that the component was healthy.
	/// </summary>
	Healthy = 2,
}