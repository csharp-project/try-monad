## Try Monad

```csharp
using TryMonad;

Func<string, Try<int>> parseInt = (x) => () => Int32.Parse(x);

var query =
    from a in parseInt("100")
    from b in parseInt("200")
    select a + b;

var result = query();

if (result.Success) {
    Console.WriteLine(result.Value);
}
```