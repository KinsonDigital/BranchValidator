﻿// <copyright file="ExpressionValidatorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BranchValidator;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="ExpressionValidatorService"/> class.
/// </summary>
public class ExpressionValidatorServiceTests
{
    // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
    private readonly Mock<IAnalyzerService> mockAnalyzerA;
    private readonly Mock<IAnalyzerService> mockAnalyzerB;
    // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
    private readonly ReadOnlyCollection<IAnalyzerService> mockedAnalyzers;

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

        this.mockedAnalyzers = new[] { this.mockAnalyzerA.Object, this.mockAnalyzerB.Object }.ToReadOnlyCollection();
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
            .WithMessage("The parameter must not be null. (Parameter 'analyzers')");
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

    [Theory]
    [InlineData(true, "expression valid")]
    [InlineData(false, "expression invalid")]
    public void Validate_WithDifferentAnalyzerResults_ReturnsCorrectResult(
        bool expectedValid,
        string expectedMsg)
    {
        // Arrange
        this.mockAnalyzerB.Setup(m => m.Analyze("test-expression"))
            .Returns((expectedValid, expectedMsg));
        var service = CreateService();

        // Act
        service.Validate("test-expression");
        var actual = service.Validate("test-expression");

        // Assert
        actual.isValid.Should().Be(expectedValid);
        actual.msg.Should().Be(expectedMsg);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ExpressionValidatorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private ExpressionValidatorService CreateService() => new (this.mockedAnalyzers);
}
