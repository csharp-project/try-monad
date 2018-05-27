using System;
using Xunit;

namespace XXX {

    public delegate TryResult<T> Try<T>();

    public class TryResult<T> {

        public TryResult(Exception e) => (Success, Error) = (false, e);
        public TryResult(T value) => (Success, Value) = (true, value);
        public bool Success { get; }
        public T Value { get; }
        public Exception Error { get; }
        public static implicit operator TryResult<T>(T value) => new TryResult<T>(value);
    }

    public static class Extensions {
        public static Try<TResult> SelectMany<TSource, TCollection, TResult>(
                this Try<TSource> self,
                Func<TSource, Try<TCollection>> collectionSelector,
                Func<TSource, TCollection, TResult> resultSelector
                ) {

            TryResult<TResult> t;

            try {
                var source = self();
                var collection = collectionSelector(source.Value)();
                var result = resultSelector(source.Value, collection.Value);
                t = new TryResult<TResult>(result);
            } catch (Exception e) {
                t = new TryResult<TResult>(e);
            }
            return new Try<TResult>(() => t);
        }
    }

    public class TryAgainTest {
        [Fact]
        public void TryTest() {

            Func<string, Try<int>> parseInt = (x) => () => {
                Console.WriteLine("~ Parse {0}", x);
                return Int32.Parse(x);
            };

            var query =
                from a in parseInt("100")
                from b in parseInt("A00")
                select a + b;

            var rs = query();

            Console.WriteLine("~ Success {0}", rs.Success);
        }
    }
}