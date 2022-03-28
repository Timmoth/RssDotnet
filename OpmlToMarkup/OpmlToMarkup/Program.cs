using System.Text;
using HtmlAgilityPack;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddDelegate<Settings>("convert", Foo)
        .WithDescription("Convert a opml file into markup");
});

return app.Run(args);

static int Foo(CommandContext context, Settings settings)
{
    var fileInfo = new FileInfo(settings.FilePath);

    if (!fileInfo.Exists)
    {
        AnsiConsole.WriteLine($"File '{fileInfo.FullName}' does not exist.");
        return -1;
    }

    AnsiConsole.WriteLine($"Reading file '{fileInfo.FullName}'");

    var doc = new HtmlDocument();
    doc.Load(fileInfo.FullName);

    var nodes = doc.DocumentNode.SelectNodes(".//outline");

    var output = new StringBuilder();

    foreach (var node in nodes)
    {
        try
        {
            var name = node.Attributes["text"].Value;
            var url = node.Attributes["htmlUrl"].Value;
            output.AppendLine($"[![{name}](https://img.shields.io/badge/{name}-{name}-green)]({url})\n");
            AnsiConsole.WriteLine($"{name} - {url}\n");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    AnsiConsole.WriteLine("--------------------------------------");

    AnsiConsole.WriteLine(output.ToString());

    AnsiConsole.WriteLine("--------------------------------------");

    File.WriteAllLines("./output.txt", new List<string>(){output.ToString()});

    return 0;
}