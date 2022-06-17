// <copyright file="ExpressionExecutorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
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
     #endregion

    /// <summary>
    /// Creates a new instance of the <see cref="ExpressionExecutorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ExpressionExecutorService CreateService() => new (this.mockValidationService.Object, this.mockFunctionService.Object);
}
