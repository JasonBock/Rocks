using Rocks.Playground;
using Rocks.Runtime;

[assembly: Rock(typeof(IUseData), BuildType.Create | BuildType.Make)]

var expectations = new IUseDataCreateExpectations();
expectations.Methods.Use(new()).ReturnValue(() => new Data { Value = 3 });

var mock = expectations.Instance();
var result = mock.Use(new Data { Value = 2 });

Console.WriteLine(result.Value);