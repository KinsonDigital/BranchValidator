// <copyright file="TestDataLoader.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BranchValidatorTests.Helpers;

/// <summary>
/// Loads test data for testing purposes.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TestDataLoader
{
    private const string TestDataDirName = "TestData";
    private static readonly Dictionary<string, string> FileData = new ();
    private static readonly string BasePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\";

    /// <summary>
    /// Loads the test sample data in the test sample data directory with the given file name.
    /// </summary>
    /// <param name="fileName">The name of the file to load.</param>
    /// <returns>The data from the file.</returns>
    /// <remarks>
    ///     If the file does not exist, a failed assertion will be thrown.
    /// </remarks>
    /// <exception cref="Exception">
    ///     Thrown if something goes wrong with loading the sample test data.
    /// </exception>
    public static string LoadFileData(string fileName)
    {
        var fullFilePath = $@"{BasePath}{TestDataDirName}\{fileName}";

        if (FileData.ContainsKey(fullFilePath))
        {
            return FileData[fullFilePath];
        }

        if (File.Exists(fullFilePath) is false)
        {
            Assert.True(false, $"The sample test data file at the path '{fullFilePath}' does not exist.");
        }

        var fileData = string.Empty;

        try
        {
            fileData = File.ReadAllText(fullFilePath);

            FileData.Add(fullFilePath, fileData);
        }
        catch (Exception e)
        {
            Assert.True(false, $"There was an issue loading the sample test data file.NoVersionFoundException{e.Message}");
        }

        return fileData;
    }
}
