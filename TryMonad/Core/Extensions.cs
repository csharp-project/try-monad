using System;
namespace TryMonad {
	public static class Extensions {

		public static Try<V> SelectMany<T, U, V>(
			this Try<T> self, Func<T, Try<U>> select, Func<T, U, V> bind) {

			if (select == null) throw new ArgumentNullException(nameof(select));
			if (bind == null) throw new ArgumentNullException(nameof(bind));

			return new Try<V>(() => {
				// T
				TryResult<T> restT;
				try {
					restT = self();
					if (restT.IsFaulted)
						return new TryResult<V>(restT.Exception);
				} catch (Exception ex) {
					return new TryResult<V>(ex);
				}

				// U
				TryResult<U> restU;
				try {
					restU = select(restT.Value)();
					if (restU.IsFaulted)
						return new TryResult<V>(restU.Exception);
				} catch (Exception ex) {
					return new TryResult<V>(ex);
				}

				// V
				V restV;
				try {
					restV = bind(restT.Value, restU.Value);
				} catch (Exception ex) {
					return new TryResult<V>(ex);
				}

				return new TryResult<V>(restV);
			});
		}
	}
}

