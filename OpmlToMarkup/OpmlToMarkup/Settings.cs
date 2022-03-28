using System.ComponentModel;
using Spectre.Console.Cli;

public sealed class Settings : CommandSettings
{
    [Description("File path.")]
    [CommandArgument(0, "[file]")]
    public string FilePath { get; init; }
}