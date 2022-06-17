namespace ScriptGenerator.Services;

public interface IGeneratorService
{
    // TODO: Code docs should explain that if the destination directory does not exist, it is created

    void GenerateScript(string srcFilePath, string destDir, string destFileName);
}
