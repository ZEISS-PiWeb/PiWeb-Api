#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region using

	using System;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;

	#endregion

	/// <summary>
	/// Special implementation of <see cref="HttpContent"/> that supports deferred streaming of the payload data.
	/// </summary>
	internal class PushStreamContent : HttpContent
	{
		#region members

		private readonly Action<Stream, HttpContent, TransportContext> _OnStreamAvailable;

		#endregion

		#region constructor

		public PushStreamContent( Action<Stream, HttpContent, TransportContext> onStreamAvailable )
			: this( onStreamAvailable, (MediaTypeHeaderValue) null )
		{}

		public PushStreamContent( Action<Stream, HttpContent, TransportContext> onStreamAvailable, MediaTypeHeaderValue mediaType )
		{
			if( onStreamAvailable == null )
				throw new ArgumentNullException( "onStreamAvailable" );

			_OnStreamAvailable = onStreamAvailable;
			Headers.ContentType = mediaType ?? new MediaTypeHeaderValue( "application/octet-stream" );
		}

		public PushStreamContent( Action<Stream, HttpContent, TransportContext> onStreamAvailable, string mediaType )
			: this( onStreamAvailable, new MediaTypeHeaderValue( mediaType ) )
		{}

		#endregion

		#region methods

		protected override Task SerializeToStreamAsync( Stream stream, TransportContext context )
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

		protected override bool TryComputeLength( out long length )
		{
			length = -1;
			return false;
		}

		#endregion

		#region class CompleteTaskOnCloseStream

		internal class CompleteTaskOnCloseStream : DelegatingStream
		{
			private TaskCompletionSource<bool> _SerializeToStreamTask;

			public CompleteTaskOnCloseStream( Stream innerStream, TaskCompletionSource<bool> serializeToStreamTask )
				: base( innerStream )
			{
				_SerializeToStreamTask = serializeToStreamTask;
			}

			public override void Close()
			{
				_SerializeToStreamTask.TrySetResult( true );
			}
		}

		#endregion

		#region class DelegatingStream

		internal abstract class DelegatingStream : Stream
		{
			private Stream _InnerStream;

			protected DelegatingStream( Stream innerStream )
			{
				if( innerStream == null )
					throw new ArgumentNullException( "innerStream" );
				_InnerStream = innerStream;
			}

			public override IAsyncResult BeginRead( byte[] buffer, int offset, int count, AsyncCallback callback, object state )
			{
				return _InnerStream.BeginRead( buffer, offset, count, callback, state );
			}

			public override IAsyncResult BeginWrite( byte[] buffer, int offset, int count, AsyncCallback callback, object state )
			{
				return _InnerStream.BeginWrite( buffer, offset, count, callback, state );
			}

			protected override void Dispose( bool disposing )
			{
				if( disposing )
				{
					_InnerStream.Dispose();
				}
				base.Dispose( disposing );
			}

			public override int EndRead( IAsyncResult asyncResult )
			{
				return _InnerStream.EndRead( asyncResult );
			}

			public override void EndWrite( IAsyncResult asyncResult )
			{
				_InnerStream.EndWrite( asyncResult );
			}

			public override void Flush()
			{
				_InnerStream.Flush();
			}

			public override int Read( byte[] buffer, int offset, int count )
			{
				return _InnerStream.Read( buffer, offset, count );
			}

			public override int ReadByte()
			{
				return _InnerStream.ReadByte();
			}

			public override long Seek( long offset, SeekOrigin origin )
			{
				return _InnerStream.Seek( offset, origin );
			}

			public override void SetLength( long value )
			{
				_InnerStream.SetLength( value );
			}

			public override void Write( byte[] buffer, int offset, int count )
			{
				_InnerStream.Write( buffer, offset, count );
			}

			public override void WriteByte( byte value )
			{
				_InnerStream.WriteByte( value );
			}

			public override bool CanRead
			{
				get { return _InnerStream.CanRead; }
			}

			public override bool CanSeek
			{
				get { return _InnerStream.CanSeek; }
			}

			public override bool CanTimeout
			{
				get { return _InnerStream.CanTimeout; }
			}

			public override bool CanWrite
			{
				get { return _InnerStream.CanWrite; }
			}

			protected Stream InnerStream
			{
				get { return _InnerStream; }
			}

			public override long Length
			{
				get { return _InnerStream.Length; }
			}

			public override long Position
			{
				get { return _InnerStream.Position; }
				set { _InnerStream.Position = value; }
			}

			public override int ReadTimeout
			{
				get { return _InnerStream.ReadTimeout; }
				set { _InnerStream.ReadTimeout = value; }
			}

			public override int WriteTimeout
			{
				get { return _InnerStream.WriteTimeout; }
				set { _InnerStream.WriteTimeout = value; }
			}
		}

		#endregion
	}
}