## DJson - dynamic JSON

<a href="https://www.nuget.org/packages/DJson/"><img src="https://img.shields.io/nuget/v/DJson.svg" /></a>
[![Made With](https://img.shields.io/badge/made%20with-C%23-success.svg?)](https://docs.microsoft.com/en-gb/dotnet/csharp/language-reference/)
<a href="https://github.com/Groxan/DJson/blob/master/LICENSE"><img src="https://img.shields.io/github/license/groxan/DJson.svg" /></a>

This is a .NET Standard 2.0 library providing a lightweight dynamic wrapper for super-neat, fast and low-allocating working with JSON, based on the new `System.Text.Json`.

This library is especially useful for prototyping and scripting when you have to keep the code as clean and easy-to-read as possible.

## Install
`PM> Install-Package DJson`

## How to use

### Instantiate dynamic json:

````cs
// parse json from string/stream/etc
var json = DJson.Parse(@"
{
    ""numValue"": 1,
    ""string_value"": ""qwerty"",
    ""array"": [ 1, 2, 3 ],
    ""object"": {
        ""value"": false
    }
}");

// or read json from file
var json = DJson.Read("file.json");

// or get json from HTTP
var json = await DJson.GetAsync("https://api.com/endpoint");

// or use HttpClient extension
var json = await httpClient.GetJsonAsync("https://api.com/endpoint");
````

### Use dynamic json as a normal class

````cs
var val = json.numValue;
var str = json.string_value;
var arr = json.array[1];
var len = json.array.length; //or .count
var obj = json.object.value;
````

### Access props using PascalCase (it's C#, baby)

````cs
var val = json.NumValue;
var str = json.StringValue;
var arr = json.Array[1];
var len = json.Array.Length; //or .Count
var obj = json.Object.Value;
````

### Enumerate arrays

````cs
foreach (var item in json.Array)
    Console.WriteLine($"{item}");

var arr = (IEnumerable<dynamic>)json.Array;
var sum = arr.Where(x => x > 1).Sum(x => x);
````

### Enumerate objects

````cs
foreach (var prop in json.Object)
    Console.WriteLine($"{prop.Name}: {prop.Value}");

var arr = (IEnumerable<dynamic>)json.Object;
var props = arr.Where(x => !x.Value).Select(x => x.Name);
````

### Convert dynamic json to any valid type

````cs
var num = (string)json.NumValue;
var list = (List<int>)json.Array;
var myObj = (MyClass)json;
````

### Extract `DateTime` like a boss

````cs
var json = DJson.Parse(@"
{
    ""time"": ""2020-09-23T21:12:16Z"",
    ""unix_time"": 1600895536,
    ""unix_time_ms"": 1600895536123
}");

DateTime time = json.Time;
DateTime unixTime = json.UnixTime; // DJson automatically detects if it's Unix time in seconds
DateTime unixTimeMs = json.UnixTimeMs; // or if it's Unix time in milliseconds
````

## What about performance?

One can think that using dynamic wrapper brings a huge overhead, **but it's actually not**. [Here is a simple benchmark](https://github.com/Groxan/DJson/blob/master/DJson.Benchmarks/DJsonBenchmarks.cs), comparing `DJson` with `JsonDocument` from `System.Text.Json` and `JToken` from `Newtonsoft.Json`:

````
|         Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Allocated |
|--------------- |---------:|----------:|----------:|------:|--------:|-------:|----------:|
| SystemTextJson | 5.367 us | 0.0251 us | 0.0235 us |  1.00 |    0.00 | 0.5493 |   2.25 KB |
|          DJson | 6.947 us | 0.1375 us | 0.1286 us |  1.29 |    0.03 | 1.0300 |   4.16 KB |
|     Newtonsoft | 9.288 us | 0.1479 us | 0.1384 us |  1.73 |    0.03 | 2.1820 |   8.97 KB |
````

Of course, using `.Deserialize<T>()` would be even faster, but we're only talking about dynamic-like access, when you will likely spend a lot more time describing the types do deserialize your JSON to.

## Contributing

Feel free to create issues, feature requests and pull requests.

Cheers 🍻
