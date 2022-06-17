using System.Diagnostics;




var toolPath = @"K:\SOFTWARE DEVELOPMENT\PERSONAL\BranchValidator\Tooling\ScriptGenerator\bin\Debug\net6.0\ScriptGenerator.exe";
var sourceFilePath = @"K:\SOFTWARE DEVELOPMENT\PERSONAL\BranchValidator\BranchValidator\FunctionDefinitions.cs";
var destDirPath = @"K:\SOFTWARE DEVELOPMENT\PERSONAL\BranchValidator\BranchValidator\RuntimeScripts";
var fileName = $"ExpressionScripts.cs";

var process = new Process();
process.StartInfo.FileName = toolPath;
process.StartInfo.Arguments = $"--source-file \"{sourceFilePath}\" --dest-dir-path \"{destDirPath}\" --file-name \"{fileName}\"";
process.Start();
