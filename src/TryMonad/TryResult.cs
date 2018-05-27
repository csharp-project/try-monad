using System;

namespace TryMonad {
    public delegate TryResult<T> Try<T>();

    public struct TryResult<T> {
        public readonly T Value;
        public readonly Exception Exception;

        public TryResult(T value) => (Value, Exception) = (value, null);
        public TryResult(Exception ex) => (Value, Exception) = (default(T), ex);

        public bool Success => Exception != null;
        public static implicit operator TryResult<T>(T value) => new TryResult<T>(value);
    }
}
