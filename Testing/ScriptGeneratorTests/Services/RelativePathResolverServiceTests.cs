// <copyright file="RelativePathResolverServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;
using FluentAssertions;
using Moq;
using ScriptGenerator.Services;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="RelativePathResolverService"/> class.
/// </summary>
public class RelativePathResolverTests
{
    private readonly Mock<IDirectory> mockDir;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelativePathResolverTests"/> class.
    /// </summary>
    public RelativePathResolverTests() => this.mockDir = new Mock<IDirectory>();

    #region Method Tests
    [Theory]
    [InlineData("C:/working-dir", null, "")]
    [InlineData("C:/working-dir", "", "")]
    [InlineData("C:/working-dir", "C:/test-dir", "C:/test-dir")] // Forward slashes
    [InlineData("C:/working-dir", "C:/test-dir/test-file.txt", "C:/test-dir/test-file.txt")] // Forward slashes
    [InlineData("C:/working-dir", "./test-dir", "C:/working-dir/test-dir")] // Forward slashes
    [InlineData("C:/working-dir", "./test-dir/", "C:/working-dir/test-dir")] // Forward slashes
    [InlineData("C:/working-dir", " C:/test-dir", "C:/test-dir")] // Spaces on left of path
    [InlineData("C:/working-dir", "C:/test-dir ", "C:/test-dir")] // Spaces on right of path
    [InlineData(@"C:\working-dir", @"C:\test-dir", @"C:/test-dir")] // Back slashes
    [InlineData(@"C:\working-dir", @"C:\test-dir\test-file.txt", @"C:/test-dir/test-file.txt")] // Back slashes
    [InlineData(@"C:\working-dir", @".\test-dir", @"C:/working-dir/test-dir")] // Back slashes
    [InlineData(@"C:\working-dir", @".\test-dir\", @"C:/working-dir/test-dir")] // Back slashes
    [InlineData("C:/parent/childA/childB", "../../test-dir/", @"C:/parent/test-dir")]
    [InlineData("C:/parent/childA/childB", "../../test-dir/test-file.txt", @"C:/parent/test-dir/test-file.txt")]
    [InlineData("C:/parent/childA/childB", "../../test-dir", @"C:/parent/test-dir")]
    [InlineData("C:/parent/childA/childB", "../../", @"C:/parent/")]
    public void Resolve_WhenInvoked_ReturnsCorrectResult(
        string currentWorkingDir,
        string path,
        string expected)
    {
        // Arrange
        this.mockDir.Setup(m => m.GetCurrentDirectory())
            .Returns(currentWorkingDir);
        var service = CreateService();

        // Act
        var actual = service.Resolve(path);

        // Assert
        actual.Should().Be(expected);
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="RelativePathResolverService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private RelativePathResolverService CreateService()
        => new (this.mockDir.Object);
}
