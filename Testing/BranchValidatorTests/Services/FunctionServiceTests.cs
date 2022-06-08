// <copyright file="FunctionServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using BranchValidatorTests.Helpers;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="FunctionService"/> class.
/// </summary>
public class FunctionServiceTests
{
    private readonly Mock<IMethodExecutor> mockMethodExecutor;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionServiceTests"/> class.
    /// </summary>
    public FunctionServiceTests() => this.mockMethodExecutor = new Mock<IMethodExecutor>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WhenInvoked_DoesNotThrowException()
    {
        // Arrange & Act
        var act = CreateService;

        // Assert
        act.Should().NotThrow("to check if function name and signature data is created correctly.");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void AvailableFunctions_WhenGettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.FunctionNames;

        // Assert
        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo("equalTo", "isCharNum");
    }
    #endregion

    #region Method Tests
    [Theory]
    [InlineData("", 0u)]
    [InlineData(null, 0u)]
    [InlineData("equalTo", 1u)]
    public void GetTotalFunctionParams_WhenInvoked_ReturnsCorrectResult(string funcName, uint expected)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.GetTotalFunctionParams(funcName);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetFunctionParamDataType_WithNullOrEmptyFunctionName_ThrowsException(string functionName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.GetFunctionParamDataType(functionName, It.IsAny<uint>());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null or empty. (Parameter 'functionName')");
    }

    [Fact]
    public void GetFunctionParamDataType_WhenFunctionIsNotFound_ThrowsException()
    {
        // Arrange
        const string functionName = "not-exist-function";
        var service = CreateService();

        // Act
        var act = () => service.GetFunctionParamDataType(functionName, 1);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage($"The function '{functionName}' was not found.");
    }

    [Fact]
    public void GetFunctionParamDataType_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.GetFunctionParamDataType("equalTo", 1);

        // Assert
        actual.Should().Be(DataTypes.String);
    }

    [Theory]
    [InlineData("my-branch", "my-branch", true)]
    [InlineData(null, "", true)]
    [InlineData("", null, true)]
    [InlineData("my-branch", "other-branch", false)]
    [InlineData(null, "my-branch", false)]
    [InlineData("", "my-branch", false)]
    [InlineData("my-branch", null, false)]
    [InlineData("my-branch", "", false)]
    public void EqualTo_WhenInvoked_ReturnsCorrectResult(
        string value,
        string branchName,
        bool expected)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.EqualTo(value, branchName);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(8, "", false)]
    [InlineData(4, "feature/123-my-branch", false)]
    [InlineData(400, "feature/123-my-branch", false)]
    [InlineData(8, "feature/123-my-branch", true)]
    public void IsCharNum_WhenInvoked_ReturnsCorrectResult(
        uint charPos,
        string branchName,
        bool expected)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.IsCharNum(charPos, branchName);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("equalTo", "test-branch", "EqualTo", "'test-branch'", true)]
    [InlineData("equalTo", "other-branch", "EqualTo", "'test-branch'", false)]
    public void Execute_WhenInvoked_ReturnsCorrectResult(
        string funcName,
        string branchName,
        string expectedMethodName,
        string paramList,
        bool actualValid)
    {
        // Arrange
        this.mockMethodExecutor.Setup(m => m.ExecuteMethod(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string[]>()))
            .Returns((actualValid, "test-msg"));

        var argValues = new List<string>();
        argValues.AddRange(paramList.Split(',', StringSplitOptions.TrimEntries));
        argValues.Add(branchName);

        var service = CreateService();

        // Act
        var actual = service.Execute(funcName, argValues.ToArray());

        // Assert
        actual.valid.Should().Be(actualValid);
        actual.msg.Should().Be("test-msg");

        this.mockMethodExecutor.VerifyOnce(m => m.ExecuteMethod(It.IsAny<object>(), expectedMethodName, argValues.ToArray()));
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="FunctionService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private FunctionService CreateService() => new (this.mockMethodExecutor.Object);
}
