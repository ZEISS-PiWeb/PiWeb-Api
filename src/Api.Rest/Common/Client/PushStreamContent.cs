#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
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
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Special implementation of <see cref="HttpContent"/> that supports deferred streaming of the payload data.
	/// </summary>
	internal class PushStreamContent : HttpContent
	{
		#region members

		private readonly Action<Stream, HttpContent, TransportContext> _OnStreamAvailable;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PushStreamContent"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="onStreamAvailable"/> is <see langword="null" />.</exception>
		public PushStreamContent(
			[NotNull] Action<Stream, HttpContent, TransportContext> onStreamAvailable,
			[CanBeNull] MediaTypeHeaderValue mediaType = null )
		{
			_OnStreamAvailable = onStreamAvailable ?? throw new ArgumentNullException( nameof( onStreamAvailable ) );
			Headers.ContentType = mediaType ?? new MediaTypeHeaderValue( "application/octet-stream" );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PushStreamContent"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="onStreamAvailable"/> is <see langword="null" />.</exception>
		public PushStreamContent( [NotNull] Action<Stream, HttpContent, TransportContext> onStreamAvailable, [NotNull] string mediaType )
			: this( onStreamAvailable, new MediaTypeHeaderValue( mediaType ) )
		{ }

		#endregion

		#region methods

		/// <inheritdoc />
		protected override Task SerializeToStreamAsync( [NotNull] Stream stream, TransportContext context )
		{
			var serializeToStreamTask = new TaskCompletionSource<bool>();
			try
			{
				_OnStreamAvailable( new CompleteTaskOnCloseStream( stream, serializeToStreamTask ), this, context );
			}
			catch( Exception exception )
			{
				serializeToStreamTask.TrySetException( exception );
			}

			return serializeToStreamTask.Task;
		}

		/// <inheritdoc />
		protected override bool TryComputeLength( out long length )
		{
			length = -1;
			return false;
		}

		#endregion

		#region class CompleteTaskOnCloseStream

		private class CompleteTaskOnCloseStream : DelegatingStream
		{
			#region members

			private readonly TaskCompletionSource<bool> _SerializeToStreamTask;

			#endregion

			#region constructors

			public CompleteTaskOnCloseStream( [NotNull] Stream innerStream, [NotNull] TaskCompletionSource<bool> serializeToStreamTask )
				: base( innerStream )
			{
				_SerializeToStreamTask = serializeToStreamTask;
			}

			#endregion

			#region methods

			/// <inheritdoc />
			public override void Close()
			{
				_SerializeToStreamTask.TrySetResult( true );
			}

			#endregion
		}

		#endregion

		#region class DelegatingStream

		private abstract class DelegatingStream : Stream
		{
			#region constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="DelegatingStream"/> class.
			/// </summary>
			/// <exception cref="ArgumentNullException"><paramref name="innerStream"/> is <see langword="null" />.</exception>
			protected DelegatingStream( [NotNull] Stream innerStream )
			{
				InnerStream = innerStream ?? throw new ArgumentNullException( nameof( innerStream ) );
			}

			#endregion

			#region properties

			/// <inheritdoc />
			public override bool CanRead => InnerStream.CanRead;

			/// <inheritdoc />
			public override bool CanSeek => InnerStream.CanSeek;

			/// <inheritdoc />
			public override bool CanTimeout => InnerStream.CanTimeout;

			/// <inheritdoc />
			public override bool CanWrite => InnerStream.CanWrite;

			protected Stream InnerStream { get; }

			/// <inheritdoc />
			public override long Length => InnerStream.Length;

			/// <inheritdoc />
			public override long Position
			{
				get => InnerStream.Position;
				set => InnerStream.Position = value;
			}

			/// <inheritdoc />
			public override int ReadTimeout
			{
				get => InnerStream.ReadTimeout;
				set => InnerStream.ReadTimeout = value;
			}

			/// <inheritdoc />
			public override int WriteTimeout
			{
				get => InnerStream.WriteTimeout;
				set => InnerStream.WriteTimeout = value;
			}

			#endregion

			#region methods

			/// <inheritdoc />
			public override IAsyncResult BeginRead( byte[] buffer, int offset, int count, AsyncCallback callback, object state )
			{
				return InnerStream.BeginRead( buffer, offset, count, callback, state );
			}

			/// <inheritdoc />
			public override IAsyncResult BeginWrite( byte[] buffer, int offset, int count, AsyncCallback callback, object state )
			{
				return InnerStream.BeginWrite( buffer, offset, count, callback, state );
			}

			/// <inheritdoc />
			protected override void Dispose( bool disposing )
			{
				if( disposing )
				{
					InnerStream.Dispose();
				}

				base.Dispose( disposing );
			}

			/// <inheritdoc />
			public override int EndRead( IAsyncResult asyncResult )
			{
				return InnerStream.EndRead( asyncResult );
			}

			/// <inheritdoc />
			public override void EndWrite( IAsyncResult asyncResult )
			{
				InnerStream.EndWrite( asyncResult );
			}

			/// <inheritdoc />
			public override void Flush()
			{
				InnerStream.Flush();
			}

			/// <inheritdoc />
			public override int Read( byte[] buffer, int offset, int count )
			{
				return InnerStream.Read( buffer, offset, count );
			}

			/// <inheritdoc />
			public override int ReadByte()
			{
				return InnerStream.ReadByte();
			}

			/// <inheritdoc />
			public override long Seek( long offset, SeekOrigin origin )
			{
				return InnerStream.Seek( offset, origin );
			}

			/// <inheritdoc />
			public override void SetLength( long value )
			{
				InnerStream.SetLength( value );
			}

			/// <inheritdoc />
			public override void Write( byte[] buffer, int offset, int count )
			{
				InnerStream.Write( buffer, offset, count );
			}

			/// <inheritdoc />
			public override void WriteByte( byte value )
			{
				InnerStream.WriteByte( value );
			}

			#endregion
		}

		#endregion
	}
}