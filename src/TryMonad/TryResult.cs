using System;

namespace TryMonad {
	public struct TryResult<T> {
		public readonly T Value;
		public readonly Exception Exception;

		public TryResult(T value) {
			Value = value;
			Exception = null;
		}

		public TryResult(Exception ex) {
			Value = default(T);
			Exception = ex;
		}

		public bool IsFaulted  => Exception != null;
		public static implicit operator TryResult<T>(T value) => new TryResult<T>(value);
	}
}

