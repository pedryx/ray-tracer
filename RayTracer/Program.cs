using RayTracer.SceneNodes;

using System;
using System.IO;


namespace RayTracer;
class Program
{
    private const string defaultConfigFile = "config.xml";
    private const string defaultGraphFile = "scene.xml";
    private const string logFile = "log.txt";
    
    private const string configFileArgument = "config";
    private const string graphFileArgument = "graph";

    /// <summary>
    /// Program configuration.
    /// </summary>
    private static Config config;

    /// <summary>
    /// Program scene graph.
    /// </summary>
    private static SceneGraph graph;

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
        config = XmlLoader.Load<Config>(parser.GetString(configFileArgument, defaultConfigFile));
        if (config == null)
            return false;

        // init scene graph
        InnerNode root = XmlLoader.Load<InnerNode>(parser.GetString(graphFileArgument, defaultGraphFile));
        if (root == null)
            return false;
        graph = new SceneGraph(root);

        double samplesSqrt = Math.Sqrt(config.SamplesPerPixel);
        if (samplesSqrt != (int)samplesSqrt)
        {
            Logger.WriteLine("Root of samples per pixel need to be integer.", LogType.Error);
            return false;
        }

        return true;
    }

    private static void Main(string[] args)
    {
        try
        {
            if (!Init(args))
                return;

            var scene = new Scene(config, graph);
            scene.Render();
        }
        finally
        {
            Logger.CloseOutputs();
        }
    }
}