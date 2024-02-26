using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaffleApi.Helpers;

namespace RaffleApi.Tests.Helpers;

[TestClass]
[TestSubject(typeof(StepBackEnumerator<int>))]
public class StepBackEnumeratorTest
{
    private readonly List<int> numbers = new List<int>()
    {
        1, 2, 3, 4, 5, 6, 7, 8, 9
    };

    [TestMethod]
    public void Enumerator_Should_Read_One()
    {
        // Arrange
        var enumerator = new StepBackEnumerator<int>(numbers.GetEnumerator());
        
        // Act
        var shouldBeTrue = enumerator.MoveNext();
        var shouldBeOne = enumerator.Current;
        
        // Assert
        Assert.IsTrue(shouldBeTrue);
        Assert.AreEqual(shouldBeOne, 1);
    }

    [TestMethod]
    public void Enumerator_Should_Read_Four()
    {
        // Arrange
        var enumerator = new StepBackEnumerator<int>(numbers.GetEnumerator());
        
        // Act
        for (int i = 0; i < 4; i++)
        {
            enumerator.MoveNext();
        }
        var shouldBeFour = enumerator.Current;
        
        // Assert
        Assert.AreEqual(shouldBeFour, 4);
    }

    [TestMethod]
    public void Enumerator_Should_Read_Three()
    {
        // Arrange
        var enumerator = new StepBackEnumerator<int>(numbers.GetEnumerator());
        
        // Act
        for (int i = 0; i < 4; i++)
        {
            enumerator.MoveNext();
        }

        var shouldBeTrue = enumerator.MovePrevious();
        var shouldBeThree = enumerator.Current;
        
        // Assert
        Assert.IsTrue(shouldBeTrue);
        Assert.AreEqual(shouldBeThree, 3);
    }

    [TestMethod]
    public void Enumerator_Should_Not_Step_Backwards()
    {
        // Arrange
        var enumerator = new StepBackEnumerator<int>(numbers.GetEnumerator());
        
        // Act
        for (int i = 0; i < 4; i++)
        {
            enumerator.MoveNext();
        }

        enumerator.MovePrevious();
        var shouldBeFalse = enumerator.MovePrevious();
        var shouldBeThree = enumerator.Current;
        
        // Assert
        Assert.IsFalse(shouldBeFalse);
        Assert.AreEqual(shouldBeThree, 3);
    }

    [TestMethod]
    public void Enumerator_Should_Not_Step_Backwards_And_Read_Four()
    {
        // Arrange
        var enumerator = new StepBackEnumerator<int>(numbers.GetEnumerator());
        
        // Act
        for (int i = 0; i < 4; i++)
        {
            enumerator.MoveNext();
        }

        enumerator.MovePrevious();
        var shouldBeFalse = enumerator.MovePrevious();
        enumerator.MoveNext();
        var shouldBeFour = enumerator.Current;
        
        // Assert
        Assert.IsFalse(shouldBeFalse);
        Assert.AreEqual(shouldBeFour, 4);
    }

    [TestMethod]
    public void Enumerator_Should_Not_Step_Backwards_And_Read_Seven()
    {
        // Arrange
        var enumerator = new StepBackEnumerator<int>(numbers.GetEnumerator());
        
        // Act
        for (int i = 0; i < 4; i++)
        {
            enumerator.MoveNext();
        }

        enumerator.MovePrevious();
        var shouldBeFalse = enumerator.MovePrevious();
        for (int i = 0; i < 4; i++)
        {
            enumerator.MoveNext();
        }
        var shouldBeSeven = enumerator.Current;
        
        // Assert
        Assert.IsFalse(shouldBeFalse);
        Assert.AreEqual(shouldBeSeven, 7);
    }

    [TestMethod]
    public void Enumerator_Should_Not_Step_Forwards()
    {
        // Arrange
        var enumerator = new StepBackEnumerator<int>(numbers.GetEnumerator());
        
        // Act
        for (int i = 0; i < 9; i++)
        {
            enumerator.MoveNext();
        }
        
        var shouldBeNine = enumerator.Current;
        var shouldBeFalse = enumerator.MoveNext();
        
        // Assert
        Assert.IsFalse(shouldBeFalse);
        Assert.AreEqual(shouldBeNine, 9);
    }
}