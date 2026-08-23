using System.Diagnostics;

namespace ModularMonolith.Tools.Migrator;

internal record ModuleInfo(string Name, string Context, string Project);

public static class Program
{
    private const string Version = "1.0.0";
    private const string StartupProject = "ModularMonolith.Web";

    private static readonly IReadOnlyList<ModuleInfo> Modules =
    [
        new("Template", "TemplateDbContext", "ModularMonolith.Module.Template.DataAccess.Sqlite"),
        new("Builder", "BuilderDbContext", "ModularMonolith.Module.Builder.DataAccess.Sqlite"),
    ];

    private static readonly string SolutionRoot = FindSolutionRoot();

    public static int Main(string[] args)
    {
        var showVersion = false;
        var update = false;
        var sql = false;

        foreach (var arg in args)
        {
            switch (arg)
            {
                case "-v":
                case "--version":
                    showVersion = true;
                    break;
                case "-u":
                case "--update":
                    update = true;
                    break;
                case "-s":
                case "--sql":
                    sql = true;
                    break;
                default:
                    Console.Error.WriteLine($"Unknown option: {arg}");
                    PrintUsage();
                    return 1;
            }
        }

        if (showVersion)
        {
            Console.WriteLine($"ModularMonolith.Tools.Migrator {Version}");
            return 0;
        }

        if (!update && !sql)
        {
            PrintUsage();
            return 1;
        }

        if (update)
        {
            Console.WriteLine("==> Applying migrations (Template -> Builder)");
            foreach (var module in Modules)
            {
                Console.WriteLine($"==> {module.Name} ({module.Context})");
                var exitCode = RunEf(
                    $"database update --context {module.Context} --project {module.Project} --startup-project {StartupProject}");
                if (exitCode != 0)
                {
                    Console.Error.WriteLine($"Migration failed for {module.Name} (exit code {exitCode}). Aborting.");
                    return exitCode;
                }
            }

            Console.WriteLine("==> All migrations applied.");
            return 0;
        }

        Console.WriteLine("==> Generating SQL scripts (Template -> Builder)");
        var sqlDir = Path.Combine(SolutionRoot, "ModularMonolith.Tools.Migrator", "Scripts", "Sql");
        Directory.CreateDirectory(sqlDir);

        foreach (var module in Modules)
        {
            var output = Path.Combine(sqlDir, $"{module.Name}.sql");
            Console.WriteLine($"==> {module.Name} ({module.Context}) -> {output}");
            var exitCode = RunEf(
                $"migrations script --context {module.Context} --project {module.Project} --startup-project {StartupProject} -o \"{output}\" --idempotent");
            if (exitCode != 0)
            {
                Console.Error.WriteLine($"SQL generation failed for {module.Name} (exit code {exitCode}). Aborting.");
                return exitCode;
            }
        }

        Console.WriteLine("==> All SQL scripts generated.");
        return 0;
    }

    private static int RunEf(string efArguments)
    {
        var startInfo = new ProcessStartInfo("dotnet", "ef " + efArguments)
        {
            WorkingDirectory = SolutionRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        using var process = Process.Start(startInfo)!;
        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                Console.WriteLine(e.Data);
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                Console.Error.WriteLine(e.Data);
        };

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        return process.ExitCode;
    }

    private static string FindSolutionRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "ModularMonolith.sln")))
            dir = dir.Parent;

        if (dir is null)
            throw new InvalidOperationException("ModularMonolith.sln not found while walking up from the executable.");

        return dir.FullName;
    }

    private static void PrintUsage()
    {
        Console.Error.WriteLine("Usage: ModularMonolith.Tools.Migrator [options]");
        Console.Error.WriteLine("  -v, --version   Show migrator version");
        Console.Error.WriteLine("  -u, --update    Apply migrations in order (Template -> Builder)");
        Console.Error.WriteLine("  -s, --sql       Generate idempotent SQL scripts per module (Template -> Builder)");
    }
}
