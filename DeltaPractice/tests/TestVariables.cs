using core.classes;

namespace tests;

public class VariableTests
{
    [Fact]
    public void Variable_TestArgumentNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new Variable(null, Variable.VariableTypeEnum.Integer, 5, 10, null, null);
        });

        Assert.Throws<ArgumentNullException>(() =>
        {
            new Variable("myVar", null, 5, 10, null, null);
        });

        // should not throw null exception
        new Variable("myVar", Variable.VariableTypeEnum.Integer, null, null, null, null);
        new Variable("myVar", Variable.VariableTypeEnum.Decimal, null, 10, null, []);
        new Variable("myVar", Variable.VariableTypeEnum.Choice, 1, 10, null, null);
    }

    [Fact]
    public void Variable_TestRangeValueSetter()
    {
        var myVar = new Variable("myVar", Variable.VariableTypeEnum.Integer, null, null, null, null);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            myVar.LimitLower = 0;
            myVar.LimitUpper = -10;
        });

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            myVar.LimitLower = 50;
            myVar.LimitUpper = 49;
        });

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            myVar.LimitLower = -4;
            myVar.LimitUpper = -10;
        });

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            myVar.LimitLower = 0;
            myVar.LimitUpper = -100;
        });

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var myVar2 = new Variable("myVar2", Variable.VariableTypeEnum.Integer, 5, 1, null, null);
        });

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var myVar2 = new Variable("myVar2", Variable.VariableTypeEnum.Integer, -10, -100, null, null);
        });
        
    }

    [Fact]
    public void Variable_TestRecalculateExceptions()
    {
        Variable myVar;

        Assert.Throws<InvalidOperationException>(() =>
        {
            myVar = new Variable("myVar", Variable.VariableTypeEnum.Integer, null, null, null, null);
            myVar.Recalculate();
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            myVar = new Variable("myVar", Variable.VariableTypeEnum.Integer, 1, null, 10, null);
            myVar.Recalculate();
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            myVar = new Variable("myVar", Variable.VariableTypeEnum.Integer, null, 10, null, null);
            myVar.Recalculate();
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            myVar = new Variable("myVar", Variable.VariableTypeEnum.Decimal, 1, null, null, null);
            myVar.Recalculate();
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            myVar = new Variable("myVar", Variable.VariableTypeEnum.Decimal, null, 10, null, null);
            myVar.Recalculate();
        });

        Assert.Throws<InvalidOperationException>(() =>
        {
            myVar = new Variable("myVar", Variable.VariableTypeEnum.Choice, null, null, null, null);
            myVar.Recalculate();
        });
        
    }


    [Fact]
    public void Variable_RangeIntegers()
    {
        var myVar = new Variable("myVar", Variable.VariableTypeEnum.Integer, 5, 10, null, null);

        for (int i = 0; i < 5000; i++)
        {
            myVar.Recalculate();
            Assert.InRange((int)myVar.CurrentValue, 5, 10);
        }
    }

    [Fact]
    public void Variable_RangeDecimals()
    {
        var myVar = new Variable("myVar", Variable.VariableTypeEnum.Decimal, 5.5, 7, 2, null);

        for (int i = 0; i < 5000; i++)
        {
            myVar.Recalculate();
            Assert.InRange((double)myVar.CurrentValue, 5.5, 7);
        }
    }

    [Fact]
    public void Variable_Choices()
    {
        object[] choices = { "apple", 42, 3.14159, "banana", 100, 5.5 };
        
        var myVar = new Variable("myVar", Variable.VariableTypeEnum.Choice, null, null, null, choices);

        for (int i = 0; i < 10000; i++)
        {
            myVar.Recalculate();
            Assert.Contains(myVar.CurrentValue, choices);
        }
    }
}
