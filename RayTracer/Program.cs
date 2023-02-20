using System;
using System.IO;

using Utils;


namespace RayTracer;

class Program
{
    private const string defaultConfigFile = "config.xml";
    private const string logFile = "log.txt";

    private static void InitLogger()
    {
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
        }

    }

    private static void Main(string[] args)
    {
        try
        {
            InitLogger();

            var config = Config.FromFile(defaultConfigFile);

            if (config == null)
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