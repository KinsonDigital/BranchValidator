// <copyright file="TextFileLoaderServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;
using FluentAssertions;
using Moq;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="TextFileLoaderService"/> class.
/// </summary>
public class TextFileLoaderServiceTests
{
    private readonly Mock<IFile> mockFile;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextFileLoaderServiceTests"/> class.
    /// </summary>
    public TextFileLoaderServiceTests() => this.mockFile = new Mock<IFile>();

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullFileParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new TextFileLoaderService(null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'file')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public void Load_WhenInvoked_LoadsTextFileAsArrayOf()
    {
        // Arrange
        const string filePath = @"C:\test-file.txt";

        this.mockFile.Setup(m => m.ReadAllText(It.IsAny<string>()))
            .Returns("test-value");
        var service = new TextFileLoaderService(this.mockFile.Object);

        // Act
        var actual = service.Load(filePath);

        // Assert
        actual.Should().Be("test-value");
    }
    #endregion
}
