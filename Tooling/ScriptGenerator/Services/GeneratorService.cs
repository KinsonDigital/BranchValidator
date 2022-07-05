// <copyright file="GeneratorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;
using BranchValidatorShared;
using BranchValidatorShared.Services;
using ScriptGenerator.Services.Interfaces;

namespace ScriptGenerator.Services;

/// <inheritdoc/>
public class GeneratorService : IGeneratorService
{
    private const string DestFileName = "ExpressionFunctions.cs";
    private const string FunctionCode = "//<function-code/>";
    private readonly IDirectory directory;
    private readonly IFile file;
    private readonly IPath path;
    private readonly IConsoleService consoleService;
    private readonly IMethodExtractorService methodExtractorService;
    private readonly IRelativePathResolverService pathResolver;
    private readonly IScriptTemplateService scriptTemplateService;
    private readonly IStringMutation[] scriptMutations;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneratorService"/> class.
    /// </summary>
    /// <param name="directory">Provides directory functionality.</param>
    /// <param name="file">Provides file functionality.</param>
    /// <param name="path">Provides path functionality.</param>
    /// <param name="consoleService">Writes messages to the console.</param>
    /// <param name="methodExtractorService">Extracts methods from <c>C#</c> source code.</param>
    /// <param name="pathResolver">Resolves paths.</param>
    /// <param name="scriptTemplateService">Creates script templates.</param>
    /// <param name="scriptMutations">A list of mutations to be performed on a script.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if any of the constructor parameters are null.
    /// </exception>
    public GeneratorService(
        IDirectory directory,
        IFile file,
        IPath path,
        IConsoleService consoleService,
        IMethodExtractorService methodExtractorService,
        IRelativePathResolverService pathResolver,
        IScriptTemplateService scriptTemplateService,
        IStringMutation[] scriptMutations)
    {
        EnsureThat.ParamIsNotNull(directory);
        EnsureThat.ParamIsNotNull(file);
        EnsureThat.ParamIsNotNull(path);
        EnsureThat.ParamIsNotNull(consoleService);
        EnsureThat.ParamIsNotNull(methodExtractorService);
        EnsureThat.ParamIsNotNull(pathResolver);
        EnsureThat.ParamIsNotNull(scriptTemplateService);
        EnsureThat.ParamIsNotNull(scriptMutations);

        this.directory = directory;
        this.file = file;
        this.path = path;
        this.consoleService = consoleService;
        this.methodExtractorService = methodExtractorService;
        this.pathResolver = pathResolver;
        this.scriptTemplateService = scriptTemplateService;
        this.scriptMutations = scriptMutations;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    ///     Occurs if any of the parameters are null or empty.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    ///     Occurs if the given <paramref name="destDir"/> does not exist.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    ///     Occurs if the file at the given <paramref name="srcFilePath"/> does not exist.
    /// </exception>
    public void GenerateScript(string srcFilePath, string destDir, string destFileName)
    {
        this.consoleService.WriteLine($"Original Source File Path: {srcFilePath}");
        this.consoleService.WriteLine($"Original Destination Directory Path: {destDir}");
        this.consoleService.WriteLine($"Original Destination File Name: {destFileName}");

        if (string.IsNullOrEmpty(srcFilePath))
        {
            throw new ArgumentNullException(nameof(srcFilePath), "The parameter must not be null or empty.");
        }

        srcFilePath = this.pathResolver.Resolve(srcFilePath);

        this.consoleService.WriteLine($"Resolving Source File Path To: {srcFilePath}");

        if (string.IsNullOrEmpty(destDir))
        {
            throw new ArgumentNullException(nameof(destDir), "The parameter must not be null or empty.");
        }

        destDir = this.pathResolver.Resolve(destDir);

        if (destDir.StartsWith($".{this.path.DirectorySeparatorChar}") ||
            destDir.StartsWith($".{this.path.AltDirectorySeparatorChar}"))
        {
            destDir = destDir[2..];
            destDir = $"{this.directory.GetCurrentDirectory()}{this.path.AltDirectorySeparatorChar}{destDir}";
        }

        if (destDir.EndsWith(this.path.DirectorySeparatorChar) || destDir.EndsWith(this.path.AltDirectorySeparatorChar))
        {
            destDir = destDir[..^1];
        }

        this.consoleService.WriteLine($"Resolving Destination Directory Path To: {destDir}");

        if (string.IsNullOrEmpty(destFileName))
        {
            throw new ArgumentNullException(nameof(destFileName), "The parameter must not be null or empty.");
        }

        if (this.directory.Exists(destDir) is false)
        {
            this.directory.CreateDirectory(destDir);
        }

        if (srcFilePath.StartsWith($".{this.path.DirectorySeparatorChar}") ||
            srcFilePath.StartsWith($".{this.path.AltDirectorySeparatorChar}"))
        {
            srcFilePath = srcFilePath[2..];
            srcFilePath = $"{this.directory.GetCurrentDirectory()}{this.path.AltDirectorySeparatorChar}{srcFilePath}";
        }

        if (this.file.Exists(srcFilePath) is false)
        {
            throw new FileNotFoundException("The source code file was not found.", srcFilePath);
        }

        var functionCode = this.methodExtractorService.Extract(srcFilePath);

        // Apply all of the mutations
        functionCode = this.scriptMutations.Aggregate(functionCode, (current, mutation) => mutation.Mutate(current));

        var script = this.scriptTemplateService.CreateTemplate();
        script = script.Replace(FunctionCode, functionCode);

        var fullDestFilePath = $"{destDir}{this.path.AltDirectorySeparatorChar}{DestFileName}";

        this.consoleService.WriteLine($"Resolving Full Destination File Path To: {fullDestFilePath}");

        this.file.WriteAllText(fullDestFilePath, script);
    }
}
