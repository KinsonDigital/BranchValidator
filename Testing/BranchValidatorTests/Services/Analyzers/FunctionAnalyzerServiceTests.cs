// <copyright file="FunctionAnalyzerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Services.Analyzers;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services.Analyzers;

/// <summary>
/// Tests the <see cref="FunctionAnalyzerService"/> class.
/// </summary>
public class FunctionAnalyzerServiceTests
{
    private readonly Mock<IFunctionExtractorService> mockFunctionExtractorService;
    private readonly Mock<ICSharpMethodService> mockCSharpMethodService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionAnalyzerServiceTests"/> class.
    /// </summary>
    public FunctionAnalyzerServiceTests()
    {
        this.mockFunctionExtractorService = new Mock<IFunctionExtractorService>();
        this.mockCSharpMethodService = new Mock<ICSharpMethodService>();
    }

    #region Method Tests
    [Fact]
    public void Analyze_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        const string leftFuncSignature = "funA('a-value')";
        const string rightFuncSignature = "funB(123, 'b-value')";
        const string expression = $"{leftFuncSignature} && {rightFuncSignature}";
        const string methodAName = "FunA";
        const string methodBName = "FunB";
        this.mockFunctionExtractorService.Setup(m => m.ExtractNames(expression))
            .Returns(new[] { "funA", "funB" });
        this.mockFunctionExtractorService.Setup(m => m.ExtractArgDataTypes(leftFuncSignature))
            .Returns(new[] { typeof(string) });
        this.mockFunctionExtractorService.Setup(m => m.ExtractArgDataTypes(rightFuncSignature))
            .Returns(new[] { typeof(uint), typeof(string) });
        this.mockFunctionExtractorService.Setup(m => m.ExtractFunctions(expression))
            .Returns(new[] { leftFuncSignature, rightFuncSignature });
        this.mockCSharpMethodService.Setup(m => m.GetMethodNames(nameof(FunctionDefinitions)))
            .Returns(new[] { methodAName, methodBName });
        this.mockCSharpMethodService.Setup(m => m.GetMethodParamTypes(nameof(FunctionDefinitions), methodAName))
            .Returns(() => new Dictionary<string, Type[]>
            {
                { $"{methodAName}:1", new[] { typeof(string) } },
            });
        this.mockCSharpMethodService.Setup(m => m.GetMethodParamTypes(nameof(FunctionDefinitions), methodBName))
            .Returns(() => new Dictionary<string, Type[]>
            {
                // Overloaded methods
                { $"{methodBName}:1", new[] { typeof(uint), typeof(string) } },
                { $"{methodBName}:2", new[] { typeof(uint), typeof(uint) } },
            });
        var service = CreateService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().BeTrue();
        actual.msg.Should().Be(string.Empty);
    }

    [Fact]
    public void Analyze_WhenCSharpMethodImplementationForExpressionFunctionDoesNotExist_ReturnsCorrectInvalidResult()
    {
        // Arrange
        const string expression = "funA() && funB() && funC()";
        this.mockFunctionExtractorService.Setup(m => m.ExtractNames(expression))
            .Returns(new[] { "funA", "funB", "funC" });
        this.mockCSharpMethodService.Setup(m => m.GetMethodNames(nameof(FunctionDefinitions)))
            .Returns(new[] { "FunA", "FunB" });
        var service = CreateService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The expression function 'funC' is not a usable function.");
    }

    [Fact]
    public void Analyze_WithIncorrectFuncArgDataType_ReturnsCorrectInvalidResult()
    {
        // Arrange
        const string expression = "funA(123)";
        const string methodName = "FunA";
        this.mockFunctionExtractorService.Setup(m => m.ExtractNames(expression))
            .Returns(new[] { "funA" });
        this.mockFunctionExtractorService.Setup(m => m.ExtractFunctions(expression))
            .Returns(new[] { "funA(123)" });
        this.mockFunctionExtractorService.Setup(m => m.ExtractArgDataTypes(expression))
            .Returns(new[] { typeof(uint) });
        this.mockCSharpMethodService.Setup(m => m.GetMethodNames(nameof(FunctionDefinitions)))
            .Returns(new[] { methodName });
        this.mockCSharpMethodService.Setup(m => m.GetMethodParamTypes(nameof(FunctionDefinitions), methodName))
            .Returns(() => new Dictionary<string, Type[]>
            {
                { $"{methodName}:1", new[] { typeof(string) } },
            });
        var service = CreateService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The value at argument position '1' for the expression function 'funA' has an incorrect data type.");
    }

    [Fact]
    public void Analyze_WhenMissingArg_ReturnsCorrectInvalidResult()
    {
        // Arrange
        const string expression = "funA(123, 'value')";
        const string methodName = "FunA";
        this.mockFunctionExtractorService.Setup(m => m.ExtractNames(expression))
            .Returns(new[] { "funA" });
        this.mockFunctionExtractorService.Setup(m => m.ExtractFunctions(expression))
            .Returns(new[] { "funA(123, 'value')" });
        this.mockFunctionExtractorService.Setup(m => m.ExtractArgDataTypes(expression))
            .Returns(new[] { typeof(uint), typeof(string) });
        this.mockCSharpMethodService.Setup(m => m.GetMethodNames(nameof(FunctionDefinitions)))
            .Returns(new[] { methodName });
        this.mockCSharpMethodService.Setup(m => m.GetMethodParamTypes(nameof(FunctionDefinitions), methodName))
            .Returns(() => new Dictionary<string, Type[]>
            {
                { $"{methodName}:1", new[] { typeof(uint) } },
            });
        var service = CreateService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().BeFalse();
        actual.msg.Should().Be("The expression function is missing an argument.");
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="FunctionAnalyzerService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private FunctionAnalyzerService CreateService()
        => new (this.mockFunctionExtractorService.Object, this.mockCSharpMethodService.Object);
}
