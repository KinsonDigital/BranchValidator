// <copyright file="GeneratorService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.IO.Abstractions;

namespace ScriptGenerator.Services;

public class GeneratorService : IGeneratorService
{
    private const string NullParamMsg = "The parameter must not be null.";
    private const string DestFileName = "ExpressionFunctions.cs";
    private const string FunctionCode = "//<function-code/>";
    private readonly IDirectory directory;
    private readonly IFile file;
    private readonly IPath path;
    private readonly IFunctionExtractorService funcExtractorService;
    private readonly IRelativePathResolverService pathResolver;
    private readonly IScriptTemplateService scriptTemplateService;
    private readonly IStringMutation[] scriptMutations;

    public GeneratorService(
        IDirectory directory,
        IFile file,
        IPath path,
        IFunctionExtractorService funcExtractorService,
        IRelativePathResolverService pathResolver,
        IScriptTemplateService scriptTemplateService,
        IStringMutation[] scriptMutations)
    {
        this.directory = directory ?? throw new ArgumentNullException(nameof(directory), NullParamMsg);
        this.file = file ?? throw new ArgumentNullException(nameof(file), NullParamMsg);
        this.path = path ?? throw new ArgumentNullException(nameof(path), NullParamMsg);
        this.funcExtractorService = funcExtractorService ?? throw new ArgumentNullException(nameof(funcExtractorService), NullParamMsg);
        this.pathResolver = pathResolver ?? throw new ArgumentNullException(nameof(pathResolver), NullParamMsg);
        // TODO: Add null check unit test
        this.scriptTemplateService = scriptTemplateService ?? throw new ArgumentNullException(nameof(scriptTemplateService), NullParamMsg);
        // TODO: Add null check unit test
        this.scriptMutations = scriptMutations ?? throw new ArgumentNullException(nameof(scriptMutations), NullParamMsg);
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
        if (string.IsNullOrEmpty(srcFilePath))
        {
            throw new ArgumentNullException(nameof(srcFilePath), "The parameter must not be null or empty.");
        }

        srcFilePath = this.pathResolver.Resolve(srcFilePath);

        if (string.IsNullOrEmpty(destDir))
        {
            throw new ArgumentNullException(nameof(destDir), "The parameter must not be null or empty.");
        }

        destDir = this.pathResolver.Resolve(destDir);

        if (destDir.StartsWith($".{this.path.DirectorySeparatorChar}") ||
            destDir.StartsWith($".{this.path.AltDirectorySeparatorChar}"))
        {
            destDir = destDir.TrimStart('.')
                .TrimStart(this.path.DirectorySeparatorChar)
                .TrimStart(this.path.AltDirectorySeparatorChar);
            destDir = $"{this.directory.GetCurrentDirectory()}{this.path.AltDirectorySeparatorChar}{destDir}";
        }

        destDir = destDir
            .TrimEnd(this.path.DirectorySeparatorChar)
            .TrimEnd(this.path.AltDirectorySeparatorChar);

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
            srcFilePath = srcFilePath.TrimStart('.')
                .TrimStart(this.path.DirectorySeparatorChar)
                .TrimStart(this.path.AltDirectorySeparatorChar);
            srcFilePath = $"{this.directory.GetCurrentDirectory()}{this.path.AltDirectorySeparatorChar}{srcFilePath}";
        }

        if (this.file.Exists(srcFilePath) is false)
        {
            throw new FileNotFoundException("The source code file was not found.", srcFilePath);
        }

        var functionCode = this.funcExtractorService.Extract(srcFilePath);

        // Apply all of the mutations
        functionCode = this.scriptMutations.Aggregate(functionCode, (current, mutation) => mutation.Mutate(current));

        var script = this.scriptTemplateService.CreateTemplate();

        script = script.Replace(FunctionCode, functionCode);

        var fullDestFilePath = $"{destDir}{this.path.AltDirectorySeparatorChar}{DestFileName}";
        this.file.WriteAllText(fullDestFilePath, script);
    }
}
