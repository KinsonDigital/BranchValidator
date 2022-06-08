// <copyright file="AnalyzerFactoryTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator.Factories;
using BranchValidator.Services;
using FluentAssertions;

namespace BranchValidatorTests.Factories;

/// <summary>
/// Tests the <see cref="AnalyzerFactory"/> class.
/// </summary>
public class AnalyzerFactoryTests
{
    #region Method Tests
    [Fact]
    public void CreateAnalyzers_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var factory = new AnalyzerFactory();

        // Act
        var actual = factory.CreateAnalyzers();

        // Assert
        actual.Should().HaveCount(4);
        actual[0].Should().BeOfType<ParenAnalyzerService>();
        actual[1].Should().BeOfType<QuoteAnalyzerService>();
        actual[2].Should().BeOfType<OperatorAnalyzerService>();
    }
    #endregion
}
