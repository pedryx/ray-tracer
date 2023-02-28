using System;
using System.IO;

using RayTracer.Utils;


namespace RayTracer;
class Program
{
    private const string defaultConfigFile = "config.xml";
    private const string logFile = "log.txt";
    private const string configFileArgument = "config";

    private static Config config;

    public static bool Init(string[] args)
    {
        Logger.AddOutput("Console", Console.Out, LogType.Message | LogType.Error, false);
        try
        {
            if (!File.Exists(logFile))
                File.Create(logFile);

            var writter = new StreamWriter(logFile);
            Logger.AddOutput("Log File", writter);
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

    private static void GenerateImage()
    {
        var floatImage = new FloatImage(config.ImageWidth, config.ImageHeight, 3);
        var function = (float x) =>
        {
            float sin = MathF.Sin(x * 2.0f * MathF.PI / floatImage.Width);
            return (sin * config.SinCoeficient + 1.0f) / 2.0f;
        };

        // render background
        for (int y = 0; y < floatImage.Height; y++)
        {
            for (int x = 0; x < floatImage.Width; x++)
            {
                var color = new float[] { 0.0f, 0.0f, function(x) };

                floatImage.PutPixel(x, y, color);
            }
        }

        // render graph
        for (int x = 0; x < floatImage.Width; x++)
        {
            float value = 1.0f - function(x);
            var color = new float[] { value, 0.0f, 0.0f };
            int y = (int)(value * floatImage.Height);

            floatImage.PutPixel(x, y, color);
            floatImage.PutPixel(x - 1, y, color);
            floatImage.PutPixel(x + 1, y, color);
            floatImage.PutPixel(x, y - 1, color);
            floatImage.PutPixel(x, y + 1, color);
        }

        floatImage.SavePFM(config.OutputFile);
        Logger.WriteLine("HDR image is finished.", LogType.Message);
    }

    private static void Main(string[] args)
    {
        try
        {
            if (!Init(args))
                return;

            GenerateImage();
        }
        finally
        {
            Logger.CloseOutputs();
        }
    }
}