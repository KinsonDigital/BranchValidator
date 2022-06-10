// <copyright file="ExpressionExecutorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using BranchValidatorTests.Helpers;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

public class ExpressionExecutorServiceTests
{
    private readonly Mock<IExpressionValidatorService> mockValidationService;
    private readonly Mock<IFunctionService> mockFunctionService;

    public ExpressionExecutorServiceTests()
    {
        this.mockValidationService = new Mock<IExpressionValidatorService>();
        this.mockValidationService.Setup(m => m.Validate(It.IsAny<string>()))
            .Returns((true, "expression valid"));

        this.mockFunctionService = new Mock<IFunctionService>();
    }

    #region Method Tests
    [Fact]
    public void Execute_WithInvalidExpression_ReturnsCorrectResult()
    {
        // Arrange
        this.mockValidationService.Setup(m => m.Validate("test-expression"))
            .Returns((false, "expression invalid"));

        var service = CreateService();

        // Act
        var actual = service.Execute("test-expression", "test-branch");

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("expression invalid");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Execute_WithNullOrEmptyExpression_ReturnsCorrectResult(string expression)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.Execute(expression, It.IsAny<string>());

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The expression must not be null or empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Execute_WithNullOrEmptyBranchName_ReturnsCorrectResult(string branchName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.Execute("test-expression", branchName);

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The branch name must not be null or empty.");
    }

    [Theory]
    [InlineData("funA(8)", "8", "feature/123-test-branch", true, true)]
    [InlineData("funA(8,10)", "8,10", "feature/123-test-branch", true, true)]
    [InlineData("funA( 8,10)", "8,10", "feature/123-test-branch", true, true)]
    [InlineData("funA(8, 10)", "8,10", "feature/123-test-branch", true, true)]
    [InlineData("funA(8,10 )", "8,10", "feature/123-test-branch", true, true)]
    [InlineData("funA()", "", "feature/123-test-branch", true, true)]
    public void Execute_WithNoOperators_ReturnsCorrectResult(
        string expression,
        string paramStr, // Comma delimited list
        string branchName,
        bool expectedFuncValid,
        bool expectedExecutionValid)
    {
        // Arrange
        const string funcName = "funA";
        var expectedMsg = $"The function '{funcName}' returned a value of '{expectedFuncValid.ToString().ToLower()}'.";
        var argValues = new List<string>();
        argValues.AddRange(paramStr.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));

        this.mockFunctionService.Setup(m => m.Execute(funcName, argValues.ToArray()))
            .Returns((expectedFuncValid, expectedMsg));
        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        actual.valid.Should().Be(expectedExecutionValid);
        actual.msg.Should().Be(expectedMsg);
    }

    [Fact]
    public void Execute_WithSingleFunctionAndNoOperators_ReturnsCorrectResult()
    {
        // Arrange
        const string branchName = "test-branch";
        const string funcName = "funcA";
        const string expression = $"{funcName}('{branchName}')";
        const bool expectedFuncValid = true;
        var expectedMsg = $"The function '{funcName}' returned a value of '{expectedFuncValid.ToString().ToLower()}'.";

        this.mockFunctionService.Setup(m => m.Execute(funcName, It.IsAny<string[]>()))
            .Returns((expectedFuncValid, expectedMsg));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        actual.valid.Should().BeTrue();
        actual.msg.Should().Be(expectedMsg);
    }

    [Theory]
    [InlineData("funA('is-2-branch') || funB(3)", true, true, true, "branch valid")]
    [InlineData("funA('is-2-branch') || funB(3)", true, false, true, "branch valid")]
    [InlineData("funA('is-2-branch') || funB(3)", false, true, true, "branch valid")]
    [InlineData("funA('is-2-branch') || funB(3)", false, false, false, "branch invalid")]
    public void Execute_WithOnlyOrOperatorsAnd2Functions_ReturnsCorrectResult(
        string expression,
        bool funAValidResult,
        bool funBValidResult,
        bool expectedExpressionValidResult,
        string expectedExpressionMsgResult)
    {
        // Arrange
        const string leftFuncName = "funA";
        const string rightFuncName = "funB";
        const string branchName = "is-2-branch";

        this.mockFunctionService.Setup(m => m.Execute(leftFuncName, It.IsAny<string[]>()))
            .Returns((funAValidResult, string.Empty));
        this.mockFunctionService.Setup(m => m.Execute(rightFuncName, It.IsAny<string[]>()))
            .Returns((funBValidResult, string.Empty));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        this.mockFunctionService.VerifyOnce(m => m.Execute(leftFuncName, "'is-2-branch'"));
        this.mockFunctionService.VerifyOnce(m => m.Execute(rightFuncName, "3"));
        actual.valid.Should().Be(expectedExpressionValidResult);
        actual.msg.Should().Be(expectedExpressionMsgResult);
    }

    [Theory]
    [InlineData("funA('is-2-branch') && funB(3)", true, true, true, "branch valid")]
    [InlineData("funA('is-2-branch') && funB(3)", true, false, false, "branch invalid")]
    [InlineData("funA('is-2-branch') && funB(3)", false, true, false, "branch invalid")]
    [InlineData("funA('is-2-branch') && funB(3)", false, false, false, "branch invalid")]
    public void Execute_WithOnlyAndOperatorsAnd2Functions_ReturnsCorrectResult(
        string expression,
        bool funAValidResult,
        bool funBValidResult,
        bool expectedExpressionValidResult,
        string expectedExpressionMsgResult)
    {
        // Arrange
        const string leftFuncName = "funA";
        const string rightFuncName = "funB";
        const string branchName = "is-2-branch";

        this.mockFunctionService.Setup(m => m.Execute(leftFuncName, It.IsAny<string[]>()))
            .Returns((funAValidResult, string.Empty));
        this.mockFunctionService.Setup(m => m.Execute(rightFuncName, It.IsAny<string[]>()))
            .Returns((funBValidResult, string.Empty));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        this.mockFunctionService.VerifyOnce(m => m.Execute(leftFuncName, "'is-2-branch'"));
        this.mockFunctionService.VerifyOnce(m => m.Execute(rightFuncName, "3"));
        actual.valid.Should().Be(expectedExpressionValidResult);
        actual.msg.Should().Be(expectedExpressionMsgResult);
    }

    [Theory(Skip = "Waiting for implementation of script execution with C#")]
    [InlineData(true, false, true, false, true, true, "branch valid")]
    [InlineData(true, false, false, false, false, false, "branch invalid")]
    [InlineData(true, false, false, true, false, false, "branch invalid")]
    [InlineData(true, true, true, false, true, true, "branch invalid")]
    public void Execute_WithBothOperatorTypesAnd2Functions_ReturnsCorrectResult(
        bool funcAValidResult,
        bool funcBValidResult,
        bool funcCValidResult,
        bool funcDValidResult,
        bool funcEValidResult,
        bool expectedExecuteResult,
        string expectedExecuteMsg)
    {
        // Arrange
        const string expression = "funA('value-A') && funB(10) || funC('value-C') || funD(20) && funE('value-E')";
        const string branchName = "test-branch";
        const string expectedMsg = "branch valid";

        this.mockFunctionService.Setup(m => m.Execute("funA", It.IsAny<string[]>()))
            .Returns((funcAValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funB", It.IsAny<string[]>()))
            .Returns((funcBValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funC", It.IsAny<string[]>()))
            .Returns((funcCValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funD", It.IsAny<string[]>()))
            .Returns((funcDValidResult, expectedMsg));
        this.mockFunctionService.Setup(m => m.Execute("funE", It.IsAny<string[]>()))
            .Returns((funcEValidResult, expectedMsg));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branchName);

        // Assert
        this.mockFunctionService.VerifyOnce(m => m.Execute("funA", "'value-A'"));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funB", "10"));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funC", "'value-C'"));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funD", "20"));
        this.mockFunctionService.VerifyOnce(m => m.Execute("funE", "'value-E'"));
        actual.valid.Should().Be(expectedExecuteResult);
        actual.msg.Should().Be(expectedExecuteMsg);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of the <see cref="ExpressionExecutorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ExpressionExecutorService CreateService() => new (this.mockValidationService.Object, this.mockFunctionService.Object);
}
