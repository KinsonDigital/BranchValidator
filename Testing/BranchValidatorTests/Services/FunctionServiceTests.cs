// <copyright file="FunctionServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using BranchValidator;
using BranchValidator.Services;
using BranchValidator.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace BranchValidatorTests.Services;

/// <summary>
/// Tests the <see cref="FunctionService"/> class.
/// </summary>
public class FunctionServiceTests
{
    private readonly Mock<IJSONService> mockJSONService;
    private readonly Mock<IEmbeddedResourceLoaderService<string>> mockResourceLoaderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionServiceTests"/> class.
    /// </summary>
    public FunctionServiceTests()
    {
        this.mockJSONService = new Mock<IJSONService>();
        this.mockJSONService.Setup(m => m.Deserialize<Dictionary<string, DataTypes[]>>(It.IsAny<string>()))
            .Returns(() => new Dictionary<string, DataTypes[]>());

        this.mockResourceLoaderService = new Mock<IEmbeddedResourceLoaderService<string>>();
     }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullJSONServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new FunctionService(
                null,
                this.mockResourceLoaderService.Object);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'jsonService')");
    }

    [Fact]
    public void Ctor_WithNullResourceLoaderServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new FunctionService(
                this.mockJSONService.Object,
                null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'resourceLoaderService')");
    }

    [Fact]
    public void Ctor_WithNullValidFunctionData_ThrowsException()
    {
        // Arrange & Act
        this.mockJSONService.Setup(m => m.Deserialize<Dictionary<string, DataTypes[]>>(It.IsAny<string>()))
            .Returns(() => null);
        var act = () => _ = CreateService();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Loading of the function definition data was unsuccessful.");
    }

    [Fact]
    public void Ctor_WhenGettingValue_CorrectlySetsAvailableFunctionsProp()
    {
        // Arrange
        this.mockJSONService.Setup(m => m.Deserialize<Dictionary<string, DataTypes[]>>(It.IsAny<string>()))
            .Returns(() =>
            {
                return new Dictionary<string, DataTypes[]>
                {
                    { "funA:p1", new[] { DataTypes.String } },
                    { "funB:p2", new[] { DataTypes.Number } },
                };
            });
        var service = CreateService();

        // Act
        var actual = service.FunctionNames;

        // Assert
        actual.Should().HaveCount(2);
        actual.Should().BeEquivalentTo("funA", "funB");
    }

    [Theory]
    [InlineData("funA", "p1", new[] { DataTypes.String }, "funA(p1: string)")]
    [InlineData("funB", "p2", new[] { DataTypes.Number }, "funB(p2: number)")]
    [InlineData("funC", "p3,p4", new[] { DataTypes.Number, DataTypes.String }, "funC(p3: number, p4: string)")]
    [InlineData("funD", " p5 ,p6", new[] { DataTypes.Number, DataTypes.String }, "funD(p5: number, p6: string)")]
    [InlineData("funE", "p7 ,p8", new[] { DataTypes.Number, DataTypes.String }, "funE(p7: number, p8: string)")]
    [InlineData("funF", "p9, p10", new[] { DataTypes.Number, DataTypes.String }, "funF(p9: number, p10: string)")]
    [InlineData("funG", "p11, p12 ", new[] { DataTypes.Number, DataTypes.String }, "funG(p11: number, p12: string)")]
    [InlineData("funH", "", new[] { DataTypes.Number }, "funH()")]
    [InlineData(" funI", "", new[] { DataTypes.Number }, "funI()")]
    [InlineData("funJ ", "", new[] { DataTypes.Number }, "funJ()")]
    public void Ctor_WhenGettingValue_CorrectlySetsFunctionSignaturesProp(
        string funcName,
        string parameters,
        DataTypes[] paramTypes,
        string expectedFuncSignatureResult)
    {
        // Arrange
        var fullFuncSignature = $"{funcName}:{parameters}";
        this.mockJSONService.Setup(m => m.Deserialize<Dictionary<string, DataTypes[]>>(It.IsAny<string>()))
            .Returns(() => new Dictionary<string, DataTypes[]>
            {
                { fullFuncSignature, paramTypes },
            });

        var expected = new[] { expectedFuncSignatureResult };
        var service = CreateService();

        // Act
        var actual = service.FunctionSignatures;

        // Assert
        actual.Should().HaveCount(1);
        actual.Should().BeEquivalentTo(expected);
    }
    #endregion

    #region Method Tests
    [Theory]
    [InlineData("", 0u)]
    [InlineData(null, 0u)]
    [InlineData("funA", 1u)]
    public void GetTotalFunctionParams_WhenInvoked_ReturnsCorrectResult(string funcName, uint expected)
    {
        // Arrange
        this.mockJSONService.Setup(m => m.Deserialize<Dictionary<string, DataTypes[]>>(It.IsAny<string>()))
            .Returns(() =>
            {
                return new Dictionary<string, DataTypes[]>
                {
                    { "funA:p1", new[] { DataTypes.String } },
                    { "funB:p2", new[] { DataTypes.Number } },
                };
            });

        var service = CreateService();

        // Act
        var actual = service.GetTotalFunctionParams(funcName);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetFunctionParamDataType_WithNullOrEmptyFunctionName_ThrowsException(string functionName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.GetFunctionParamDataType(functionName, It.IsAny<uint>());

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null or empty. (Parameter 'functionName')");
    }

    [Fact]
    public void GetFunctionParamDataType_WhenFunctionIsNotFound_ThrowsException()
    {
        // Arrange
        const string functionName = "not-exist-function";
        var service = CreateService();

        // Act
        var act = () => service.GetFunctionParamDataType(functionName, 1);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage($"The function '{functionName}' was not found.");
    }

    [Fact]
    public void GetFunctionParamDataType_WhenInvoked_ReturnsCorrectResult()
    {
        // Arrange
        this.mockJSONService.Setup(m => m.Deserialize<Dictionary<string, DataTypes[]>>(It.IsAny<string>()))
            .Returns(() =>
            {
                return new Dictionary<string, DataTypes[]>
                {
                    { "funA:p1", new[] { DataTypes.String } },
                    { "funB:p2", new[] { DataTypes.Number } },
                };
            });

        var service = CreateService();

        // Act
        var actual = service.GetFunctionParamDataType("funA", 1);

        // Assert
        actual.Should().Be(DataTypes.String);
    }

    [Theory]
    [InlineData("funA", "test-branch", "FunA", "'test-branch'", true)]
    [InlineData("funA", "other-branch", "FunA", "'test-branch'", false)]
    public void Execute_WhenInvoked_ReturnsCorrectResult(
        string funcName,
        string branchName,
        string expectedMethodName,
        string paramList,
        bool actualValid)
    {
        // Arrange
        this.mockJSONService.Setup(m => m.Deserialize<Dictionary<string, DataTypes[]>>(It.IsAny<string>()))
            .Returns(() =>
            {
                return new Dictionary<string, DataTypes[]>
                {
                    { "funA:p1", new[] { DataTypes.String } },
                    { "funB:p2", new[] { DataTypes.Number } },
                };
            });

        var argValues = new List<string>();
        argValues.AddRange(paramList.Split(',', StringSplitOptions.TrimEntries));
        argValues.Add(branchName);

        var service = CreateService();

        // Act
        var actual = service.Execute(funcName, argValues.ToArray());

        // Assert
        actual.valid.Should().Be(actualValid);
        actual.msg.Should().Be("test-msg");
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="FunctionService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private FunctionService CreateService()
        => new (this.mockJSONService.Object,
            this.mockResourceLoaderService.Object);
}
