// <copyright file="GeneratorServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;
using System.Text;
using BranchValidatorShared.Services;
using FluentAssertions;
using Moq;
using ScriptGenerator.Services;
using ScriptGenerator.Services.Interfaces;
using TestingShared;

namespace ScriptGeneratorTests.Services;

/// <summary>
/// Tests the <see cref="GeneratorService"/> class.
/// </summary>
public class GeneratorServiceTests
{
    private readonly Mock<IDirectory> mockDir;
    private readonly Mock<IFile> mockFile;
    private readonly Mock<IPath> mockPath;
    private readonly Mock<IConsoleService> mockConsoleService;
    private readonly Mock<IMethodExtractorService> mockMethodExtractorService;
    private readonly Mock<IRelativePathResolverService> mockRelativePathResolverService;
    private readonly Mock<IScriptTemplateService> mockScriptTemplateService;
    private readonly IStringMutation[] mockMutations;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneratorServiceTests"/> class.
    /// </summary>
    public GeneratorServiceTests()
    {
        this.mockDir = new Mock<IDirectory>();
        this.mockDir.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

        this.mockFile = new Mock<IFile>();
        this.mockFile.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

        this.mockPath = new Mock<IPath>();

        this.mockConsoleService = new Mock<IConsoleService>();

        this.mockMethodExtractorService = new Mock<IMethodExtractorService>();
        this.mockRelativePathResolverService = new Mock<IRelativePathResolverService>();

        this.mockScriptTemplateService = new Mock<IScriptTemplateService>();
        this.mockScriptTemplateService.Setup(m => m.CreateTemplate()).Returns(string.Empty);

        var mockMutationA = new Mock<IStringMutation>();
        mockMutationA.Setup(m => m.Mutate(It.IsAny<string>()))
            .Returns<string>(value => value);

        var mockMutationB = new Mock<IStringMutation>();
        mockMutationB.Setup(m => m.Mutate(It.IsAny<string>()))
            .Returns<string>(value => value);

        this.mockMutations = new[]
        {
            mockMutationA.Object,
            mockMutationB.Object,
        };
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullDirectoryParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                null,
                this.mockFile.Object,
                this.mockPath.Object,
                this.mockConsoleService.Object,
                this.mockMethodExtractorService.Object,
                this.mockRelativePathResolverService.Object,
                this.mockScriptTemplateService.Object,
                this.mockMutations);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'directory')");
    }

    [Fact]
    public void Ctor_WithNullFileParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                this.mockDir.Object,
                null,
                this.mockPath.Object,
                this.mockConsoleService.Object,
                this.mockMethodExtractorService.Object,
                this.mockRelativePathResolverService.Object,
                this.mockScriptTemplateService.Object,
                this.mockMutations);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'file')");
    }

    [Fact]
    public void Ctor_WithNullPathParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                this.mockDir.Object,
                this.mockFile.Object,
                null,
                this.mockConsoleService.Object,
                this.mockMethodExtractorService.Object,
                this.mockRelativePathResolverService.Object,
                this.mockScriptTemplateService.Object,
                this.mockMutations);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'path')");
    }

    [Fact]
    public void Ctor_WithConsoleServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                this.mockDir.Object,
                this.mockFile.Object,
                this.mockPath.Object,
                null,
                this.mockMethodExtractorService.Object,
                this.mockRelativePathResolverService.Object,
                this.mockScriptTemplateService.Object,
                this.mockMutations);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'consoleService')");
    }

    [Fact]
    public void Ctor_WithNullMethodExtractorServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                this.mockDir.Object,
                this.mockFile.Object,
                this.mockPath.Object,
                this.mockConsoleService.Object,
                null,
                this.mockRelativePathResolverService.Object,
                this.mockScriptTemplateService.Object,
                this.mockMutations);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'methodExtractorService')");
    }

    [Fact]
    public void Ctor_WithNullPathResolverParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                this.mockDir.Object,
                this.mockFile.Object,
                this.mockPath.Object,
                this.mockConsoleService.Object,
                this.mockMethodExtractorService.Object,
                null,
                this.mockScriptTemplateService.Object,
                this.mockMutations);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'pathResolver')");
    }

    [Fact]
    public void Ctor_WithNullScriptTemplateServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                this.mockDir.Object,
                this.mockFile.Object,
                this.mockPath.Object,
                this.mockConsoleService.Object,
                this.mockMethodExtractorService.Object,
                this.mockRelativePathResolverService.Object,
                null,
                this.mockMutations);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'scriptTemplateService')");
    }

    [Fact]
    public void Ctor_WithNullScriptMutationsParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GeneratorService(
                this.mockDir.Object,
                this.mockFile.Object,
                this.mockPath.Object,
                this.mockConsoleService.Object,
                this.mockMethodExtractorService.Object,
                this.mockRelativePathResolverService.Object,
                this.mockScriptTemplateService.Object,
                null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'scriptMutations')");
    }
    #endregion

    #region Method Tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GenerateScript_WithNullOrEmptySrcFilePath_ThrowsException(string srcFilePath)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.GenerateScript(srcFilePath, "test-dest-path", "test-file-name");

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null or empty. (Parameter 'srcFilePath')");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GenerateScript_WithNullOrEmptyFileDestinationDirectory_ThrowsException(string fileDestDir)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.GenerateScript("test-dest-path", fileDestDir, "test-file-name");

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null or empty. (Parameter 'destDir')");
    }

    [Fact]
    public void GenerateScript_WhenDirectoryDoesNotExist_CreatesDirectory()
    {
        // Arrange
        const string destDir = @"C:/dest-dir";
        const string srcFilePath = "C:/test-file.txt";
        this.mockDir.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);
        this.mockRelativePathResolverService.Setup(m => m.Resolve(srcFilePath))
            .Returns(srcFilePath);
        this.mockRelativePathResolverService.Setup(m => m.Resolve(destDir))
            .Returns(destDir);

        var service = CreateService();

        // Act
        service.GenerateScript(srcFilePath, destDir, "test-file.cs");

        // Assert
        this.mockDir.VerifyOnce(m => m.CreateDirectory(destDir));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GenerateScript_WithNullOrEmptyDestinationFileName_ThrowsException(string destFileName)
    {
        // Arrange
        const string destDir = "test-dir";
        this.mockRelativePathResolverService.Setup(m => m.Resolve(destDir))
            .Returns(destDir);
        var service = CreateService();

        // Act
        var act = () => service.GenerateScript("test-dest-path", destDir, destFileName);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null or empty. (Parameter 'destFileName')");
    }

    [Fact]
    public void GenerateScript_WhenSourceFileDoesNotExist_ThrowsException()
    {
        // Arrange
        const string srcFilePath = @"C:/test-dir/test-file.txt";
        this.mockFile.Setup(m => m.Exists(srcFilePath)).Returns(false);
        this.mockRelativePathResolverService.Setup(m => m.Resolve(It.IsAny<string>()))
            .Returns(srcFilePath);
        var service = CreateService();

        // Act
        var act = () => service.GenerateScript(srcFilePath, "test-dir", "test-file-name");

        // Assert
        var thrownException = act.Should().Throw<FileNotFoundException>()
            .WithMessage("The source code file was not found.").Subject.ToArray();
        thrownException[0].FileName.Should().Be(srcFilePath);
    }

    [Theory]
    [InlineData("", "C:/base-dir/test-file.cs", "C:/base-dir/destination-dir/", "C:/base-dir/destination-dir/ExpressionFunctions.cs")]
    [InlineData("", "C:/base-dir/test-file.cs", "C:/base-dir/destination-dir", "C:/base-dir/destination-dir/ExpressionFunctions.cs")]
    [InlineData("C:/base-dir", "C:/base-dir/test-file.cs", "./destination-dir", "C:/base-dir/destination-dir/ExpressionFunctions.cs")]
    [InlineData("C:/base-dir", "C:/base-dir/test-file.cs", @".\destination-dir", "C:/base-dir/destination-dir/ExpressionFunctions.cs")]
    [InlineData("C:/base-dir", "./test-file.cs", "C:/base-dir/destination-dir", "C:/base-dir/destination-dir/ExpressionFunctions.cs")]
    [InlineData("C:/base-dir", @".\test-file.cs", "C:/base-dir/destination-dir", "C:/base-dir/destination-dir/ExpressionFunctions.cs")]
    public void GenerateScript_WhenInvoked_ReturnsCorrectResult(
        string currentWorkingDir,
        string srcFilePath,
        string destDir,
        string expectedDestFilePath)
    {
        // Arrange
        const string destFileName = "ExpressionFunctions.cs";
        const string funcCodeInjectionPoint = "//<function-code/>";

#pragma warning disable SA1137
        var funcCodeStrBuilder = new StringBuilder();
        funcCodeStrBuilder.AppendLine("\tpublic static bool FuncA(string value)");
        funcCodeStrBuilder.AppendLine("\t{");
            funcCodeStrBuilder.AppendLine("\t\treturn value == this.branchName;");
        funcCodeStrBuilder.AppendLine("\t}");
#pragma warning restore SA1137

        var expectedFuncCode = funcCodeStrBuilder.ToString();

        var scriptTemplateStrBuilder = new StringBuilder();
        scriptTemplateStrBuilder.AppendLine("begin");
        scriptTemplateStrBuilder.AppendLine(funcCodeInjectionPoint);
        scriptTemplateStrBuilder.AppendLine("end");

        var expected = scriptTemplateStrBuilder.ToString();
        expected = expected.Replace(funcCodeInjectionPoint, expectedFuncCode);

        this.mockScriptTemplateService.Setup(m => m.CreateTemplate()).Returns(scriptTemplateStrBuilder.ToString);
        this.mockMethodExtractorService.Setup(m => m.Extract(It.IsAny<string>()))
            .Returns(expectedFuncCode);
        this.mockDir.Setup(m => m.GetCurrentDirectory()).Returns(currentWorkingDir);
        this.mockPath.SetupGet(p => p.AltDirectorySeparatorChar).Returns('/');
        this.mockRelativePathResolverService.Setup(m => m.Resolve(srcFilePath))
            .Returns(srcFilePath);
        this.mockRelativePathResolverService.Setup(m => m.Resolve(destDir))
            .Returns(destDir);
        var service = CreateService();

        // Act
        service.GenerateScript(srcFilePath, destDir, destFileName);

        // Assert
        this.mockFile.VerifyOnce(m => m.WriteAllText(expectedDestFilePath, expected));
        this.mockConsoleService.VerifyOnce(m => m.WriteLine($"Original Source File Path: {srcFilePath}"));
        this.mockConsoleService.VerifyOnce(m => m.WriteLine($"Original Destination Directory Path: {destDir}"));
        this.mockConsoleService.VerifyOnce(m => m.WriteLine($"Original Destination File Name: {destFileName}"));
        this.mockConsoleService.VerifyOnce(m => m.WriteLine("Resolving Destination Directory Path To: C:/base-dir/destination-dir"));
        this.mockMethodExtractorService.VerifyOnce(m => m.Extract("C:/base-dir/test-file.cs"));
        this.mockConsoleService.VerifyOnce(m => m.WriteLine($"Resolving Full Destination File Path To: {expectedDestFilePath}"));
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="GeneratorService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private GeneratorService CreateService()
        => new (this.mockDir.Object,
            this.mockFile.Object,
            this.mockPath.Object,
            this.mockConsoleService.Object,
            this.mockMethodExtractorService.Object,
            this.mockRelativePathResolverService.Object,
            this.mockScriptTemplateService.Object,
            this.mockMutations);
}
