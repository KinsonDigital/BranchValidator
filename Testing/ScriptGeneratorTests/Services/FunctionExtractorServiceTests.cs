// <copyright file="FunctionExtractorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using Moq;
using ScriptGenerator.Exceptions;
using ScriptGenerator.Services;
using ScriptGeneratorTests.Helpers;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="FunctionExtractorService"/> class.
/// </summary>
public class FunctionExtractorServiceTests
{
    private readonly Mock<IFileLoaderService<string[]>> mockFileLoaderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionExtractorServiceTests"/> class.
    /// </summary>
    public FunctionExtractorServiceTests() => this.mockFileLoaderService = new Mock<IFileLoaderService<string[]>>();

    #region Method Tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Extract_WithNullOrEmptyFilePath_ThrowsException(string filePath)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.Extract(filePath);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null or empty. (Parameter 'filePath')");
    }

    [Fact]
    public void Extract_WithNoFileData_ThrowsException()
    {
        // Arrange
        const string filePath = @"C:\scripts\script-functions.cs";

        this.mockFileLoaderService.Setup(m => m.Load(It.IsAny<string>()))
            .Returns(Array.Empty<string>());

        var service = CreateService();

        // Act
        var act = () => service.Extract(filePath);

        // Assert
        act.Should().Throw<FileDataDoesNotExistException>()
            .WithMessage($"The file located at '{filePath}' does not contain any data.");
    }

    [Fact]
    public void Extract_WithNoScriptFunctionStartStopTagPairs_ThrowsException()
    {
        // Arrange
        const string filePath = @"C:\scripts\script-functions.cs";

        this.mockFileLoaderService.Setup(m => m.Load(It.IsAny<string>()))
            .Returns(new[]
            {
                "public class FunctionDefinitions",
                "{",
                "}",
            });
        var service = CreateService();

        // Act
        var act = () => service.Extract(filePath);

        // Assert
        act.Should().Throw<InvalidScriptSourceException>()
            .WithMessage($"The contents of the script file '{filePath}' does not contain any function start and stop tag pairs.");
    }

    [Theory]
    [InlineData("//<script-function>", "", "A script function end tag '//</script-function>' is missing.")]
    [InlineData("", "//</script-function>", "A script function start tag '//<script-function>' is missing.")]
    public void Extract_WithMismatchedTags_ThrowsException(
        string startTag,
        string endTag,
        string expectedExceptionMsg)
    {
        // Arrange
        const string filePath = @"C:\scripts\script-functions.cs";

        this.mockFileLoaderService.Setup(m => m.Load(It.IsAny<string>()))
            .Returns(new[]
            {
#pragma warning disable SA1137
                "public class FunctionDefinitions",
                "{",
                    startTag,
                    "public bool FuncA()",
                    "{",
                    "}",
                    endTag,
                "}",
#pragma warning restore SA1137
            });
        var service = CreateService();

        // Act
        var act = () => service.Extract(filePath);

        // Assert
        act.Should().Throw<InvalidScriptSourceException>()
            .WithMessage(expectedExceptionMsg);
    }

    [Fact]
    public void Extract_WithSingleFunction_ReturnsCorrectResult()
    {
        // Arrange
        var expected =
                    "    public bool TestFunction(string value)" + Environment.NewLine;
        expected += "    {" + Environment.NewLine;
        expected += "        return value == this.branchName;" + Environment.NewLine;
        expected += "    }";

        const string testDataFileName = "SingleScriptFunction.txt";

        const string filePath = @"C:\scripts\test-script.cs";
        var sampleData = TestDataLoader.LoadFileDataAsLines(testDataFileName);
        this.mockFileLoaderService.Setup(m => m.Load(filePath))
            .Returns(sampleData);
        var service = CreateService();

        // Act
        var actual = service.Extract(filePath);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Extract_With2FunctionsAndOneToBeIgnoredBetween_ReturnsCorrectResult()
    {
        // Arrange
        var expected =
                    "    public bool FuncA(string value)" + Environment.NewLine;
        expected += "    {" + Environment.NewLine;
        expected += "        var simpleVar = \"this is the FuncA() body\";" + Environment.NewLine;
        expected += "        return true;" + Environment.NewLine;
        expected += "    }" + Environment.NewLine;
        expected += Environment.NewLine;
        expected += "    public bool FuncB(string value)" + Environment.NewLine;
        expected += "    {" + Environment.NewLine;
        expected += "        var simpleVar = \"this is the FuncB() body\";" + Environment.NewLine;
        expected += "        return true;" + Environment.NewLine;
        expected += "    }";

        const string testDataFileName = "2ScriptFunctions.txt";

        const string filePath = @"C:\scripts\test-script.cs";
        var sampleData = TestDataLoader.LoadFileDataAsLines(testDataFileName);
        this.mockFileLoaderService.Setup(m => m.Load(filePath))
            .Returns(sampleData);
        var service = CreateService();

        // Act
        var actual = service.Extract(filePath);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="FunctionExtractorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private FunctionExtractorService CreateService() => new (this.mockFileLoaderService.Object);
}
