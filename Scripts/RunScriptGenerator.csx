#r "nuget: System.Diagnostics.Process, 4.3.0"


if (Args.Count <= 0)
{
    WriteLine("No arguments passed to the script.");
    WriteLine("Generator Tool Not Executed!!");
    return 0;
}

for (int i = 0; i < Args.Count; i++)
{
    WriteLine($"Arg 1 Value: {Args[i]}");
}

var toolPath = Args[0];

// If the file does not exist, exit with an error
if (File.Exists(toolPath))
{
    Console.WriteLine($"The generator tool path file '{toolPath}' does not exist.");
    return 100;
}

var sourceFilePath = @"../../../../../BranchValidator/FunctionDefinitions.cs";
var destDirPath = @"../../../../../BranchValidator/RuntimeScripts";
var fileName = $"ExpressionScripts.cs";

var process = new Process();
process.StartInfo.FileName = toolPath;
process.StartInfo.WorkingDirectory = Path.GetDirectoryName(toolPath);
process.StartInfo.Arguments = $"--source-file \"{sourceFilePath}\" --dest-dir-path \"{destDirPath}\" --file-name \"{fileName}\"";
process.Start();
