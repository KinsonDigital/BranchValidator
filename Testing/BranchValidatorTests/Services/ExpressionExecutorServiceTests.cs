// <copyright file="ExpressionExecutorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;
using TestingShared;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="ExpressionExecutorService"/> class.
/// </summary>
public class ExpressionExecutorServiceTests
{
    private readonly Mock<IEmbeddedResourceLoaderService<string>> mockResourceLoaderService;
    private readonly Mock<IScriptService<(bool result, string[] funcResults)>> mockScriptService;
    private readonly Mock<ICSharpMethodService> mockMethodService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionExecutorServiceTests"/> class.
    /// </summary>
    public ExpressionExecutorServiceTests()
    {
        this.mockMethodService = new Mock<ICSharpMethodService>();
        this.mockScriptService = new Mock<IScriptService<(bool result, string[] funcResults)>>();

        this.mockResourceLoaderService = new Mock<IEmbeddedResourceLoaderService<string>>();
        this.mockResourceLoaderService.Setup(m => m.LoadResource("ExpressionFunctions.cs"))
            .Returns(string.Empty);
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullCSharpMethodServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new ExpressionExecutorService(
                null,
                this.mockResourceLoaderService.Object,
                this.mockScriptService.Object);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'csharpMethodService')");
    }

    [Fact]
    public void Ctor_WithNullResourceLoaderServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new ExpressionExecutorService(
                this.mockMethodService.Object,
                null,
                this.mockScriptService.Object);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'resourceLoaderService')");
    }

    [Fact]
    public void Ctor_WithNullScriptServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new ExpressionExecutorService(
                this.mockMethodService.Object,
                this.mockResourceLoaderService.Object,
                null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'scriptService')");
    }
    #endregion

    #region Method Tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Execute_WithNullOrEmptyExpression_ReturnsCorrectInvalidResult(string expression)
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
    public void Execute_WithNullOrEmptyBranchName_ReturnsCorrectInvalidResult(string branchName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.Execute("test-expression", branchName);

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The branch name must not be null or empty.");
    }

    [Fact]
    public void Execute_WithInvalidScriptResult_ReturnsCorrectInvalidResult()
    {
        // Arrange
        const string funcName = "funA";
        const string expression = $"{funcName}()";
        var expectedMsg = $"Function Results:{Environment.NewLine}\t{funcName}() -> false";

        this.mockScriptService.Setup(m => m.Execute(It.IsAny<string>()))
            .Returns((false, new[] { $"{expression} -> false" }));

        var service = CreateService();

        // Act
        var actual = service.Execute("funA()", "test-branch");

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be(expectedMsg);
    }

    [Fact]
    public void Execute_WithValidScriptResult_ReturnsCorrectValidResult()
    {
        // Arrange
        const string branchInjectionPoint = "//<branch-name/>";
        const string expressionInjectionPoint = "//<expression/>";
        const string expFunAName = "funA";
        const string expFunBName = "funB";
        const string methodFunAName = "FunA";
        const string methodFunBName = "FunB";
        const string expression = $"{expFunAName}() && {expFunBName}('value')";
        const string funcAResult = $"{expFunAName}() -> true";
        const string funcBResult = $"{expFunBName}(\"value\") -> true";
        const string branch = "feature/123-test-branch";

        var scriptFuncResults = new[] { funcAResult, funcBResult };
        var expectedMsg = $"Function Results:{Environment.NewLine}\t{funcAResult}{Environment.NewLine}\t{funcBResult}";

        var scriptSrc = branchInjectionPoint;
        scriptSrc += Environment.NewLine;
        scriptSrc += expressionInjectionPoint;

        var expectedScriptSrc = branch;
        expectedScriptSrc += Environment.NewLine;
        expectedScriptSrc += $"return (ExpressionFunctions.{methodFunAName}() && ";
        expectedScriptSrc += $"ExpressionFunctions.{methodFunBName}(\"value\"), ExpressionFunctions.GetFunctionResults());";

        this.mockResourceLoaderService.Setup(m => m.LoadResource("ExpressionFunctions.cs"))
            .Returns(scriptSrc);
        this.mockMethodService.Setup(m => m.GetMethodNames(nameof(FunctionDefinitions)))
            .Returns(new[] { methodFunAName, methodFunBName });
        this.mockScriptService.Setup(m => m.Execute(It.IsAny<string>()))
            .Returns((true, scriptFuncResults));

        var service = CreateService();

        // Act
        var actual = service.Execute(expression, branch);

        // Assert
        actual.valid.Should().BeTrue();
        actual.msg.Should().Be(expectedMsg);
        this.mockScriptService.VerifyOnce(m => m.Execute(expectedScriptSrc));
    }
    #endregion

    /// <summary>
    /// Creates a new instance of the <see cref="ExpressionExecutorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ExpressionExecutorService CreateService()
        => new (this.mockMethodService.Object,
        this.mockResourceLoaderService.Object,
        this.mockScriptService.Object);
}
