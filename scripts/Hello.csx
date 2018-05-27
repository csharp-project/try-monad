#! "netcoreapp2.1"
#r "../src/TryMonad/obj/Debug/netstandard2.0/TryMonad.dll"

using TryMonad;

Func<string, Try<int>> parseInt = (x) => () => Int32.Parse(x);

var query =
    from a in parseInt("100")
    from b in parseInt("200")
    select a + b;

var result = query();

if (!result.IsFaulted) {
    Console.WriteLine(result.Value);
}