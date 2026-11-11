#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading;
	using System.Threading.Tasks;

	#endregion

	/// <summary>
	/// <see cref="HttpContent"/> that writes its body directly to the request stream by calling a callback.
	/// The content length is unknown, so the request is sent with chunked transfer encoding.
	/// The callback may dispose the stream it gets; the underlying request stream stays open.
	/// </summary>
	internal sealed class StreamingJsonContent : HttpContent
	{
		#region members

		private readonly Func<Stream, Task> _WriteAsync;

		#endregion

		#region constructors

		public StreamingJsonContent( Func<Stream, Task> writeAsync, string mediaType )
		{
			_WriteAsync = writeAsync ?? throw new ArgumentNullException( nameof( writeAsync ) );
			Headers.ContentType = new MediaTypeHeaderValue( mediaType );
		}

		#endregion

		#region methods

		protected override async Task SerializeToStreamAsync( Stream stream, TransportContext context )
		{
			using var nonClosingStream = new NonClosingStream( stream );
			await _WriteAsync( nonClosingStream ).ConfigureAwait( false );
		}

		protected override bool TryComputeLength( out long length )
		{
			length = 0;
			return false;
		}

		#endregion

		#region class NonClosingStream

		/// <summary>
		/// Write-only wrapper that flushes, but does not dispose, the inner stream when it is disposed.
		/// </summary>
		private sealed class NonClosingStream : Stream
		{
			private readonly Stream _Inner;

			public NonClosingStream( Stream inner )
			{
				_Inner = inner;
			}

			public override bool CanRead => false;
			public override bool CanSeek => false;
			public override bool CanWrite => true;
			public override long Length => throw new NotSupportedException();

			public override long Position
			{
				get => throw new NotSupportedException();
				set => throw new NotSupportedException();
			}

			public override void Flush() => _Inner.Flush();
			public override Task FlushAsync( CancellationToken cancellationToken ) => _Inner.FlushAsync( cancellationToken );
			public override void Write( byte[] buffer, int offset, int count ) => _Inner.Write( buffer, offset, count );
			public override Task WriteAsync( byte[] buffer, int offset, int count, CancellationToken cancellationToken ) => _Inner.WriteAsync( buffer, offset, count, cancellationToken );
#if NETCOREAPP
			public override void Write( ReadOnlySpan<byte> buffer ) => _Inner.Write( buffer );
			public override ValueTask WriteAsync( ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default ) => _Inner.WriteAsync( buffer, cancellationToken );
#endif
			public override int Read( byte[] buffer, int offset, int count ) => throw new NotSupportedException();
			public override long Seek( long offset, SeekOrigin origin ) => throw new NotSupportedException();
			public override void SetLength( long value ) => throw new NotSupportedException();
		}

		#endregion
	}
}
