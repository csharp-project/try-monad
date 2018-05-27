using System;
using FluentAssertions;

namespace TryMonad.Tests {
	public class TryMonadTests {
		public Try<int> GetA() {
			return () => 1;
		}
		public Try<int> GetB() {
			return () => { throw new Exception("Error B"); };
		}

		public Try<int> GetC() {
			return () => 2;
		}

		public void ShouldFailWithLinq() {
			var query =
				from a in GetA()
				from b in GetB()
				select a + b;

			var result = query();
			result.Value.Should().Be(0);
			result.IsFaulted.Should().Be(true);
			result.Exception.Message.Should().Be("Error B");
		}

		public void ShouldFailWithLambda() {
			var a = GetA();
			var b = GetB();

			var query = a.SelectMany(x => b, (x, y) => x + y);
			var rs = query();
			rs.Value.Should().Be(0);
			rs.Exception.Message.Should().Be("Error B");
		}

		public void SuccessWithLinq() {
			var query =
				from a in GetA()
				from c in GetC()
				select a + c;

			var rs = query();
			rs.Value.Should().Be(3);
			rs.IsFaulted.Should().Be(false);
				
		}

		public void Chain() {
			Func<int, Try<int>> addOne = (x) => () => x + 1;

			var query =
				from a in addOne(0)
				from b in addOne(a)
				from c in addOne(b)
				from d in addOne(c)
				select d;

			var rs = query();
			rs.Value.Should().Be(0 + 1 * 4);
		}

		public void LambdaChain() {
			Func<int, Try<int>> addOne = (x) => () => x + 1;

			var a = addOne(0);
			var query =
				a.SelectMany(b => addOne(b), (x, y) => y)
				 .SelectMany(c => addOne(c), (x, y) => y)
				 .SelectMany(d => addOne(d), (x, y) => y);
			var result = query();
			result.Value.Should().Be(0 + 1 * 4);
				
		}
	}
}

