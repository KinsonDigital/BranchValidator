// <copyright file="AppInputsTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using CommandLine;
using FluentAssertions;
using ScriptGenerator;
using TestingShared;
// ReSharper disable UseObjectOrCollectionInitializer
namespace ScriptGeneratorTests;

/// <summary>
/// Tests the <see cref="AppInputs"/> class.
/// </summary>
public class AppInputsTests
{
    #region Contructor Tests
    [Fact]
    public void Ctor_WhenConstructed_PropsHaveCorrectDefaultValuesAndDecoratedWithAttributes()
    {
        // Arrange & Act
        var inputs = new AppInputs();

        // Assert
        inputs.SourceFilePath.Should().BeEmpty();
        typeof(AppInputs).GetProperty(nameof(AppInputs.SourceFilePath)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(AppInputs.SourceFilePath))
            .AssertOptionAttrProps(
                "s",
                "source-file",
                true,
                string.Empty,
                "The full file path to the source file.");

        inputs.DestinationDirPath.Should().BeEmpty();
        typeof(AppInputs).GetProperty(nameof(AppInputs.DestinationDirPath)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(AppInputs.DestinationDirPath))
            .AssertOptionAttrProps(
                "d",
                "dest-dir-path",
                true,
                string.Empty,
                "The full directory path of where to save the generated script file.");

        inputs.FileName.Should().BeEmpty();
        typeof(AppInputs).GetProperty(nameof(AppInputs.FileName)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(AppInputs.FileName))
            .AssertOptionAttrProps(
                "f",
                "file-name",
                true,
                string.Empty,
                "The name of the source generated script file.");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void SourceFilePath_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new AppInputs();

        // Act
        inputs.SourceFilePath = "test-branch";

        // Assert
        inputs.SourceFilePath.Should().Be("test-branch");
    }

    [Fact]
    public void DestinationDirPath_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new AppInputs();

        // Act
        inputs.DestinationDirPath = "test-logic";

        // Assert
        inputs.DestinationDirPath.Should().Be("test-logic");
    }

    [Fact]
    public void FileName_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new AppInputs();

        // Act
        inputs.FileName = "trim-test";

        // Assert
        inputs.FileName.Should().Be("trim-test");
    }
    #endregion
}
