## Dynamic JSON

<a href="https://www.nuget.org/packages/Dynamic.Json/"><img src="https://img.shields.io/nuget/v/Dynamic.Json.svg" /></a>
[![Made With](https://img.shields.io/badge/made%20with-C%23-success.svg?)](https://docs.microsoft.com/en-gb/dotnet/csharp/language-reference/)
<a href="https://github.com/Groxan/Dynamic.Json/blob/master/LICENSE"><img src="https://img.shields.io/github/license/groxan/Dynamic.Json.svg" /></a>

This is a .NET Standard 2.0 library providing a lightweight dynamic wrapper for super-neat, fast and low-allocating working with JSON, based on the new `System.Text.Json`.

This library is especially useful for prototyping and scripting when you have to keep the code as clean and easy-to-read as possible.

## Install
`PM> Install-Package Dynamic.Json`

## How to use

### Instantiate dynamic json:

````cs
// parse json from string/stream/etc, for example
var json = DJson.Parse(@"
{
    ""versionNumber"": 1,
    ""product_name"": ""qwerty"",
    ""items"": [ 1, 2, 3 ],
    ""settings"": {
        ""enabled"": false
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

You can access json props either like class properties (`json.someProperty`) or like dictionary elements (`json["someProperty"]`). Both methods are identical, however, the first one looks better :)

````cs
int version = json.versionNumber;
string name = json.product_name;
int item = json.items[1];
int length = json.items.length; //or json.items.count, as you prefer
bool enabled = json.settings.enabled;
````

### Flexible naming convention

`camelCase` and `snake_case` are common naming styles in JSON, but uncommon in C# where we use `PascalCase` for public properties. Therefore, **for perfectionists** like me `DJson` allows to access json props not only by their original names (e.g. `json.ip_endpoint`), but also by its **PascalCase** version (e.g. `json.IPEndpoint`). `DJson` will try to resolve names automatically.

````cs
var val = json.VersionNumber;
var str = json.ProductName;
var arr = json.Items[1];
var len = json.Items.Length; //or .Count
var obj = json.Settings.Enabled;
````

### Enumerate arrays

You can enumerate json arrays in `foreach` or by implicit conversion to `IEnumerable<dynamic>`.

````cs
foreach (var item in json.Items)
    Console.WriteLine($"{item}");

var items = (IEnumerable<dynamic>)json.Items;
var sum = items.Where(x => x > 1).Sum(x => x);
````

### Enumerate objects

You can enumerate json object props `(string Name, dynamic Value)` in `foreach` or by implicit conversion to `IEnumerable<dynamic>`.

````cs
foreach (var prop in json.Settings)
    Console.WriteLine($"{prop.Name}: {prop.Value}");

var props = (IEnumerable<dynamic>)json.Settings;
var names = props.Where(x => !x.Value).Select(x => x.Name);
````

### Convert dynamic json to any valid type

While conversion to all built-in C# types is pretty transparent, you can also serialize json to any other type using implicit conversion.

````cs
var list = (List<int>)json.Items; // dynamic array -> List<int>
var settings = (MyClass)json.Settings; // dynamic object -> MyClass
var obj = (AnotherType)json;
````

### Extract `DateTime` like a boss

People often use Unix time format in JSON and that's quite annoying to convert it to `DateTime`, so `DJson` does this for you automatically.

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

One can think that using dynamic wrapper brings a huge overhead, **but it's actually not**. [Here is a simple benchmark](https://github.com/Groxan/Dynamic.Json/blob/master/Dynamic.Json.Benchmarks/DJsonBenchmarks.cs), comparing `DJson` with `JsonDocument` from `System.Text.Json` and `JToken` from `Newtonsoft.Json`:

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
