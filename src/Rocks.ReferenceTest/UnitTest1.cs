using Rocks;
using Rocks.SourceTest;

[assembly: Rock(typeof(IDoStuff), BuildType.Create)]

namespace Rocks.ReferenceTest;

public static class Tests
{
   [Test]
   public static void Test1()
   {
		var expectations = new IDoStuffCreateExpectations();
   }
}
