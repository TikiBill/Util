# Time-Based UUID (GUID) Creation for .NET

## Credit / References

This package is based on the work of the
[Creating a Time UUID (GUID) in .NET](http://nickberardi.com/creating-a-time-uuid-guid-in-net/)
blog post. It separates out the minimal number of parts to create a stand-alone
NuGet package.

This package shamelessly uses code from:

* [Fluent Cassandra](https://github.com/fluentcassandra/fluentcassandra/tree/master/src) -- Apache License 2.0
* [A gist by Nick Berardi](https://gist.github.com/nberardi/3759754) -- No License, blog post author.

## Usage

```csharp
using LavaData.Util.GuidGenerate;

// Based on the current UTC time.
Guid guid = GuidGenerator.GenerateTimeBasedGuid();

// Based on a past event.
Guid oldGuild = GuidGenerator.GenerateTimeBasedGuid(new DateTime(2020, 03, 17));
```

## END OF LINE
