namespace TestProject1;

[TestClass]
public class DummyCalculatorTest
{
    [TestMethod]
    public void TestAddMethod()
    {
        Assert.AreEqual(3, WebApplication1.DummyCalculator.Add(1, 2));
    }
}
