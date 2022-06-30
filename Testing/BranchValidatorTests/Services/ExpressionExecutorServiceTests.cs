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
    private readonly Mock<IEmbeddedResourceLoaderService<string>> mockResourceLoader;
    private readonly Mock<IScriptService<(bool result, string[] funcResults)>> mockScriptService;
    private readonly Mock<ICSharpMethodService> mockMethodService;

    public ExpressionExecutorServiceTests()
    {
        this.mockMethodService = new Mock<ICSharpMethodService>();
        this.mockScriptService = new Mock<IScriptService<(bool result, string[] funcResults)>>();

        this.mockResourceLoader = new Mock<IEmbeddedResourceLoaderService<string>>();
        this.mockResourceLoader.Setup(m => m.LoadResource("ExpressionFunctions.cs"))
            .Returns(string.Empty);
    }

    #region Method Tests
    [Fact]
    public void Execute_WithInvalidExpression_ReturnsCorrectResult()
    {
        // Arrange
        const string funcName = "funA";
        const string expression = $"{funcName}()";
        var expectedMsg = $"Function Results:{Environment.NewLine}\t{funcName}() -> true";

        this.mockScriptService.Setup(m => m.Execute(It.IsAny<string>()))
            .Returns((false, new[] { $"{expression} -> true" }));

        var service = CreateService();

        // Act
        var actual = service.Execute("funA()", "test-branch");

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be(expectedMsg);
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
    private ExpressionExecutorService CreateService()
        => new (this.mockMethodService.Object,
        this.mockResourceLoader.Object,
        this.mockScriptService.Object);
}
