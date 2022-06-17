namespace ScriptGenerator.Services;

public class RemoveInheritCodeDocs : IStringMutation
{
    public string Mutate(string value)
    {
        value = value.Replace("\r\n", "\n");
        value = value.Replace("\n\r", "\n");
        value = value.Replace("\r", "\n");

        var lines = value.Split('\n');

        var resultLines = new List<string>();

        foreach (var line in lines)
        {
            if (line.Contains("///") && line.Contains("<inheritdoc") && line.Contains("/>"))
            {
                continue;
            }

            resultLines.Add(line);
        }

        var result = string.Join(Environment.NewLine, resultLines);

        return result;
    }
}
