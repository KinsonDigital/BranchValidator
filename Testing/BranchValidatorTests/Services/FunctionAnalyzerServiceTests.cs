// <copyright file="FunctionAnalyzerServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="FunctionAnalyzerService"/> class
/// </summary>
public class FunctionAnalyzerServiceTests
{
    private readonly Mock<IFunctionNamesExtractorService> mockFunctionNamesExtractorService;
    private readonly Mock<IMethodNamesService> mockMethodNamesService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionAnalyzerServiceTests"/> class.
    /// </summary>
    public FunctionAnalyzerServiceTests()
    {
        this.mockFunctionNamesExtractorService = new Mock<IFunctionNamesExtractorService>();
        this.mockMethodNamesService = new Mock<IMethodNamesService>();
    }

    #region Method Tests
    [Fact]
    public void Analyze_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        var expression = "funA() && funB()";
        this.mockFunctionNamesExtractorService.Setup(m => m.ExtractNames(expression))
            .Returns(new[] { "funA", "funB" });
        this.mockMethodNamesService.Setup(m => m.GetMethodNames(nameof(FunctionDefinitions)))
            .Returns(new[] { "FunA", "FunB" });
        var service = CreateService();

        // Act
        var actual = service.Analyze(expression);

        // Assert
        actual.valid.Should().Be(true);
        actual.msg.Should().Be("All Functions Found");
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="FunctionAnalyzerService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private FunctionAnalyzerService CreateService()
        => new (this.mockFunctionNamesExtractorService.Object, this.mockMethodNamesService.Object);

}
