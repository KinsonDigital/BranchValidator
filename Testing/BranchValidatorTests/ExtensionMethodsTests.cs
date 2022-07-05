// <copyright file="ExtensionMethodsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests;

/// <summary>
/// Tests the <see cref="ExtensionMethods"/> class.
/// </summary>
public class ExtensionMethodsTests
{
    #region Method Tests
    [Fact]
    public void ToReadOnlyCollection_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var items = new[] { 1, 2, 3, 4 };

        // Act
        var actual = items.ToReadOnlyCollection();

        // Assert
        actual.Should().HaveCount(4);
        actual.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("z", "Z")]
    [InlineData("TestValue", "TestValue")]
    [InlineData("testValue", "TestValue")]
    [InlineData("testvalue", "Testvalue")]
    [InlineData("_Testvalue", "_Testvalue")]
    public void ToPascalCase_WhenInvoked_ReturnsCorrectResult(
        string value,
        string expected)
    {
        // Act
        var actual = value.ToPascalCase();

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("hi", "hi", 1)]
    [InlineData("hi hi", "hi", 2)]
    [InlineData("hihi", "hi", 2)]
    [InlineData("hih", "hi", 1)]
    [InlineData("hi on the himalaya mountain", "hi", 2)]
    [InlineData("funA() funB() || funC()", "||", 1)]
    public void Count_WhenInvoked_ReturnsCorrectResult(string value, string searchValue, int expected)
    {
        // Act
        var actual = value.Count(searchValue);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("item1,item2", "item3", true)]
    [InlineData("item1,item2", "item2", false)]
    public void DoesNotContain_WithIEnumerableList_ReturnsCorrectResult(
        string itemList,
        string item,
        bool expected)
    {
        // Arrange
        var items = itemList.Split(',');

        // Act
        var actual = items.DoesNotContain(item);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData('e', false)]
    [InlineData('~', true)]
    public void DoesNotContain_WithArrayOfCharacters_ReturnsCorrectResult(char character, bool expected)
    {
        // Arrange
        var characters = new[] { 't', 'e', 's', 't' };

        // Act
        var actual = characters.DoesNotContain(character);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("does-not-have", "test", true)]
    [InlineData("does-have", "does", false)]
    [InlineData("", "test", false)]
    [InlineData(null, "test", false)]
    [InlineData("", "", true)]
    [InlineData(null, null, true)]
    public void DoesNotContains_WhenInvoked_ReturnsCorrectResult(string containerStr, string value, bool expected)
    {
        // Act
        var actual = containerStr.DoesNotContain(value);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("test-value", false)]
    [InlineData("testvalue", true)]
    public void DoesNotContains_WhenInvokedWithCharParam_ReturnsCorrectResult(string testValue, bool expected)
    {
        // Act
        var actual = testValue.DoesNotContain('-');

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("valuez", true)]
    [InlineData("zvalue", false)]
    public void DoesNotStartWith(string value, bool expected)
    {
        // Act
        var actual = value.DoesNotStartWith('z');

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("value", true)]
    [InlineData("valuez", false)]
    public void DoesNotEndWith(string value, bool expected)
    {
        // Act
        var actual = value.DoesNotEndWith('z');

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("testvalue", true)]
    [InlineData("test-value", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void DoesNotContain_WhenInvoked_ReturnsCorrectResult(string value, bool expected)
    {
        // Act
        var actual = value.DoesNotContain('-');

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("test-value", "test")]
    [InlineData("testvalue", "testvalue")]
    [InlineData("-test-value", "")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void GetUpToChar_WhenInvoked_ReturnsCorrectResult(string testValue, string expected)
    {
        // Act
        var actual = testValue.GetUpToChar('-');

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void ToUpper_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        const char expected = 'k';

        // Act
        var actual = expected.ToUpper();

        // Assert
        actual.Should().Be('K');
    }

    [Fact]
    public void ToLower_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        const char expected = 'K';

        // Act
        var actual = expected.ToLower();

        // Assert
        actual.Should().Be('k');
    }

    [Theory]
    [InlineData("is(test)value", "test")]
    [InlineData("(istestvalue)", "istestvalue")]
    [InlineData("is)test(value", "")]
    [InlineData("istest)value", "")]
    [InlineData("is(testvalue", "")]
    [InlineData("is()value", "")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void GetBetween_WhenInvoked_ReturnsCorrectResult(string thisValue, string expected)
    {
        // Act
        var actual = thisValue.GetBetween('(', ')');

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("refs/heads/feature/123-my-branch", "refs/heads/", "feature/123-my-branch")]
    [InlineData("feature/123-branch-123", "123", "feature/123-branch-123")]
    [InlineData("feature/123-my-branch", "refs/heads/", "feature/123-my-branch")]
    [InlineData("feature/123-my-branch", null, "feature/123-my-branch")]
    [InlineData("feature/123-my-branch", "", "feature/123-my-branch")]
    public void TrimStart_WhenInvoked_ReturnsCorrectResult(
        string thisStr,
        string value,
        string expected)
    {
        // Act
        var actual = thisStr.TrimStart(value);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "", 0, 0, false)]
    [InlineData("", "", 0, 0, false)]
    [InlineData("funA()", null, 0, 0, false)]
    [InlineData("funA()", "", 0, 0, false)]
    [InlineData("funA() && funB()", "||", 4, 14, false)]
    [InlineData("funA() && funB() funC()", "&&", 14, 20, false)]
    [InlineData("funA() funB()", "&&", 0, 13, false)]
    [InlineData("funA() funB()", "&&", 0, 100, false)]
    [InlineData("funA() funB() &&", "&&", 5, 9, false)]
    [InlineData("funA() && funB()", "&&", 7, 8, false)]
    [InlineData("funA() && funB()", "&&", 8, 9, false)]
    [InlineData("funA() && funB()", "&&", 7, 9, true)]
    [InlineData("funA() && funB() && funC()", "&&", 15, 24, true)]
    [InlineData("funA() && || funB()", "&&", 5, 11, true)]
    public void IsBetween_WhenInvokingWithIntParams_ReturnsCorrectResult(
        string thisString,
        string value,
        uint leftPos,
        uint rightPos,
        bool expected)
    {
        // Act
        var actual = thisString.IsBetween(value, leftPos, rightPos);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "", 0, 0, true)]
    [InlineData("", "", 0, 0, true)]
    [InlineData("funA()", null, 0, 0, true)]
    [InlineData("funA()", "", 0, 0, true)]
    [InlineData("funA() && funB()", "||", 4, 14, true)]
    [InlineData("funA() && funB() funC()", "&&", 14, 20, true)]
    [InlineData("funA() funB()", "&&", 0, 13, true)]
    [InlineData("funA() funB()", "&&", 0, 100, true)]
    [InlineData("funA() funB() &&", "&&", 5, 9, true)]
    [InlineData("funA() && funB()", "&&", 7, 8, true)]
    [InlineData("funA() && funB()", "&&", 8, 9, true)]
    [InlineData("funA() && funB()", "&&", 7, 9, false)]
    [InlineData("funA() && || funB()", "&&", 5, 11, false)]
    public void IsNotBetween_WhenInvokingWithIntParams_ReturnsCorrectResult(
        string thisString,
        string value,
        uint leftPos,
        uint rightPos,
        bool expected)
    {
        // Act
        var actual = thisString.IsNotBetween(value, leftPos, rightPos);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("funA('value')", '\'', '(', ')', true)]
    [InlineData("funA'value')", '\'', '(', ')', false)]
    [InlineData("funA('value'", '\'', '(', ')', false)]
    [InlineData("funA(123)", '\'', '(', ')', false)]
    [InlineData("fun'A(value')", '\'', '(', ')', false)]
    [InlineData("funA('value)'", '\'', '(', ')', false)]
    [InlineData("funA('value') && funB('other-value'", '\'', '(', ')', false)]
    [InlineData("funA('value') && funB'other-value')", '\'', '(', ')', false)]
    [InlineData("funA('value') && funB('other-value')", '\'', '(', ')', true)]
    [InlineData("funA('value') && funB(123)", '\'', '(', ')', true)]
    public void AllIsBetween_WhenInvoked_ReturnsCorrectResult(
        string thisString,
        char value,
        char leftChar,
        char rightChar,
        bool expected)
    {
        // Act
        var actual = thisString.AllIsBetween(value, leftChar, rightChar);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("funA('value')", '\'', '(', ')', false)]
    [InlineData("funA'value')", '\'', '(', ')', true)]
    [InlineData("funA('value'", '\'', '(', ')', true)]
    [InlineData("funA(123)", '\'', '(', ')', true)]
    [InlineData("fun'A(value')", '\'', '(', ')', true)]
    [InlineData("funA('value)'", '\'', '(', ')', true)]
    [InlineData("funA('value') && funB('other-value'", '\'', '(', ')', true)]
    [InlineData("funA('value') && funB'other-value')", '\'', '(', ')', true)]
    [InlineData("funA('value') && funB('other-value')", '\'', '(', ')', false)]
    [InlineData("funA('value') && funB(123)", '\'', '(', ')', false)]
    public void AnyNotBetween_WhenInvoked_ReturnsCorrectResult(
        string thisString,
        char value,
        char leftChar,
        char rightChar,
        bool expected)
    {
        // Act
        var actual = thisString.AnyNotBetween(value, leftChar, rightChar);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("123abc", false)]
    [InlineData("123-", false)]
    [InlineData("12.3", false)]
    [InlineData("123", true)]
    [InlineData("-123", true)]
    public void IsWholeNumber_WhenInvoked_ReturnsCorrectResult(string value, bool expected)
    {
        // Act
        var actual = value.IsWholeNumber();

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void GetMethod_WhenMethodDoesNotExist_ReturnsCorrectResult()
    {
        // Arrange
        var testClass = new SampleTestClass();

        // Act
        var actual = testClass.GetMethod("non-existing-method", typeof(bool), It.IsAny<string[]>());

        // Assert
        actual.result.Should().BeFalse();
        actual.msg.Should().Be("A function with the name 'non-existing-method' does not exist.");
        actual.method.Should().BeNull();
    }

    [Fact]
    public void GetMethod_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var argValues = new[] { "'string-value'" };

        var testClass = new SampleTestClass();

        // Act
        var actual = testClass.GetMethod(nameof(SampleTestClass.MethodWithSingleParam), typeof(bool), argValues);

        // Assert
        actual.result.Should().BeTrue();
        actual.msg.Should().Be(string.Empty);
        actual.method.Should().NotBeNull();
    }

    [Fact]
    public void GetMethod_WhenNoMethodsExistWithSameParamCount_ReturnsCorrectResult()
    {
        // Arrange
        var argValues = new[] { "'string-value-1'", "123" };

        var testClass = new SampleTestClass();

        // Act
        var actual = testClass.GetMethod(nameof(SampleTestClass.MethodWith3Params), typeof(bool), argValues);

        // Assert
        actual.result.Should().BeFalse();
        actual.msg.Should().Be($"No function with the name '{nameof(SampleTestClass.MethodWith3Params)}' with '2' parameters found.");
        actual.method.Should().BeNull();
    }

    [Fact]
    public void GetMethod_With2ParamsOfDiffTypes_ReturnsCorrectResult()
    {
        // Arrange
        var argValues = new[] { "'string-value-1'", "'string-value-2'" };

        var testClass = new SampleTestClass();

        // Act
        var actual = testClass.GetMethod(nameof(SampleTestClass.MethodWith2ParamsDiffTypes), typeof(bool), argValues);

        // Assert
        actual.result.Should().BeFalse();
        actual.msg.Should().Be("No function with the parameter type of 'string' found at parameter position '2'.");
        actual.method.Should().BeNull();
    }

    [Theory]
    [InlineData("123", "456", true, "")] // Function Example: isSectionNum(8, 10)
    [InlineData("123", "'-'", true, "")] // Function Example: isSectionNum(8, '-')
    // Validation would catch that quotes do not exist, but this is just checking the param value without caring about the analyzers
    [InlineData("123", "-", true, "")] // Function Example: isSectionNum(8, -)
    public void GetMethod_WithOverloadedMethods_ReturnsCorrectResult(
        string param1Value,
        string param2Value,
        bool expectedValidResult,
        string expectedMsgResult)
    {
        // Arrange
        var argValues = new[] { param1Value, param2Value };

        var testClass = new SampleTestClass();

        // Act
        var actual = testClass.GetMethod(nameof(SampleTestClass.MethodOverload), typeof(bool), argValues);

        // Assert
        actual.result.Should().Be(expectedValidResult);
        actual.msg.Should().Be(expectedMsgResult);
        actual.method.Should().NotBeNull();
    }
    #endregion
}
