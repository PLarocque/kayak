﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kayak.Core
{
    public interface IHttpResponder
    {
        // may return IHttpServerResponse or IObservable<IHttpServerResponse>
        // (observables must yield single value and complete or yield exception.)
        object Respond(IHttpServerRequest request);
    }

    public interface IHttpServerRequest
    {
        IDictionary<object, object> Context { get; }
        string Verb { get; }
        string RequestUri { get; }
        string HttpVersion { get; }
        IDictionary<string, string> Headers { get; } // all keys lower-cased.

        // application must call the function with a destination buffer for each read. the returned observable
        // yields a single integer indicating the number of bytes read and completes, or yields a single exception.
        // observable may yield 0 bytes, this does not signify the end of the stream (stream ends when enumerable
        // ends). this is useful for middlewares which must do buffering.
        IEnumerable<Func<ArraySegment<byte>, IObservable<int>>> GetBody();
    }

    public interface IHttpServerResponse
    {
        string Status { get; }
        IDictionary<string, string> Headers { get; }

        // may contain string, ArraySegment<byte>, FileInfo, or observables which contain
        // string, ArraySegment<byte>, or FileInfo.
        IEnumerable<object> GetBody();
    }
}
