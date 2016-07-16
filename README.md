## Try Monad

```csharp
Func<int, Try<int>> addOne = (x) => () => x + 1;
var query =
    from a in addOne(0)
    from b in addOne(a)
    from c in addOne(b)
    from d in addOne(c)
    select d;
var result = query();
result.Value.Should().Be(0 + 1 * 4);
```

## Install

> Install-Package TryMonad
