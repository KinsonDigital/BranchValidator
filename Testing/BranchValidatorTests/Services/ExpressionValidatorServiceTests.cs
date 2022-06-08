// <copyright file="ExpressionValidatorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Factories;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="ExpressionValidatorService"/> class.
/// </summary>
public class ExpressionValidatorServiceTests
{
    private readonly Mock<IAnalyzerFactory> mockAnalyzerFactory;
    private readonly Mock<IAnalyzerService> mockAnalyzerA;
    private readonly Mock<IAnalyzerService> mockAnalyzerB;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionValidatorServiceTests"/> class.
    /// </summary>
    public ExpressionValidatorServiceTests()
    {
        this.mockAnalyzerA = new Mock<IAnalyzerService>();
        this.mockAnalyzerA.Setup(m => m.Analyze(It.IsAny<string>()))
            .Returns((true, string.Empty));

        this.mockAnalyzerB = new Mock<IAnalyzerService>();
        this.mockAnalyzerB.Setup(m => m.Analyze(It.IsAny<string>()))
            .Returns((true, string.Empty));

        this.mockAnalyzerFactory = new Mock<IAnalyzerFactory>();
        this.mockAnalyzerFactory.Setup(m => m.CreateAnalyzers())
            .Returns(() => new[] { this.mockAnalyzerA.Object, this.mockAnalyzerB.Object }.ToReadOnlyCollection());
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullAnalyzerFactoryParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new ExpressionValidatorService(null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'analyzerFactory')");
    }
    #endregion

    #region Method Tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithNullOrEmptyExpression_ReturnsCorrectResult(string value)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = service.Validate(value);

        // Assert
        act.isValid.Should().BeFalse();
        act.msg.Should().Be("The expression must not be null or empty.");
    }

    [Fact]
    public void Validate_WithMultiFunctionExpressions_ReturnsCorrectResult()
    {
        // Arrange
        var service = CreateService();

        // Act
        var actual = service.Validate("f1() && f2() || f3() && f4()");

        // Assert
        actual.isValid.Should().BeTrue();
        actual.msg.Should().Be("expression valid");
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ExpressionValidatorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ExpressionValidatorService CreateService() => new (this.mockAnalyzerFactory.Object);
}
