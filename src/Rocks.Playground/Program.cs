using Rocks.Playground;

//[assembly: Rock(typeof(IUseData), BuildType.Create | BuildType.Make)]

//var expectations = new IUseDataCreateExpectations();
var expectations = new IUseDataRefStructCreateExpectations();
expectations.Methods.Use(new()).ReturnValue(() => new Data { Value = 3 });

var mock = expectations.Instance();
var result = mock.Use(new Data { Value = 2 });

Console.WriteLine(result.Value);