using System;
using System.IO;

using Utils;


namespace RayTracer;

class Program
{
    private const string defaultConfigFile = "config.xml";
    private const string logFile = "log.txt";
    private const string configFileArgument = "config";

    private static Config config;

    private static bool Init(string[] args)
    {
        // init logger
        try
        {
            if (!File.Exists(logFile))
                File.Create(logFile);

            var writter = new StreamWriter(logFile);
            Logger.AddOutput("Log File", writter);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Warning: Could not prepare log file: {ex.Message}");
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

            var floatImage = new FloatImage(config.ImageWidth, config.ImageHeight, 3);
            floatImage.SavePFM(config.OutputFile);

            Console.WriteLine("HDR image is finished.");
        }
        finally
        {
            Logger.CloseOutputs();
        }
    }
}