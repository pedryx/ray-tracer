using System;
using System.IO;


namespace RayTracer;
class Program
{
    private const string defaultConfigFile = "config.xml";
    private const string logFile = "log.txt";
    private const string configFileArgument = "config";

    /// <summary>
    /// Program configuration.
    /// </summary>
    private static Config config;

    /// <summary>
    /// Initialize program.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>True on success, otherwise false.</returns>
    public static bool Init(string[] args)
    {
        // init logger
        Logger.AddOutput("Console", Console.Out, LogType.Message | LogType.Error, false);
        try
        {
            if (!File.Exists(logFile))
                File.Create(logFile);

            var writer = new StreamWriter(logFile);
            Logger.AddOutput("Log File", writer);
        }
        catch (IOException ex)
        {
            Logger.WriteLine($"Warning: Could not prepare log file: {ex.Message}", LogType.Error);
            return false;
        }

        // init parser
        var parser = new CLIParser();
        parser.AddNamedArgument(configFileArgument, false);
        if (!parser.Parse(args))
            return false;

        // init config
        config = Config.FromFile(parser.GetString(configFileArgument, defaultConfigFile));
        if (config == null)
            return false;

        return true;
    }

    private static void Main(string[] args)
    {
        try
        {
            if (!Init(args))
                return;

            var scene = new Scene(config);
            scene.Render();
        }
        finally
        {
            Logger.CloseOutputs();
        }
    }
}