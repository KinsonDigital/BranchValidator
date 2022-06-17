// <copyright file="TextFileLinesLoaderServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;
using FluentAssertions;
using Moq;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="TextFileLinesLoaderService"/> class.
/// </summary>
public class TextFileLinesLoaderServiceTests
{
    private readonly Mock<IFile> mockFile;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextFileLinesLoaderServiceTests"/> class.
    /// </summary>
    public TextFileLinesLoaderServiceTests() => this.mockFile = new Mock<IFile>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullFileParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new TextFileLinesLoaderService(null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'file')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Load_WhenInvoked_LoadsTextFileAsArrayOfLines()
    {
        // Arrange
        const string filePath = @"C:\test-file.txt";

        this.mockFile.Setup(m => m.ReadAllLines(It.IsAny<string>()))
            .Returns(new[] { "line-1", "line-2" });
        var service = new TextFileLinesLoaderService(this.mockFile.Object);

        // Act
        var actual = service.Load(filePath);

        // Assert
        actual.Should().BeEquivalentTo("line-1", "line-2");
    }
    #endregion
}
