using System;
using System.IO;
using System.Xml.Serialization;


namespace RayTracer;

public class Config
{
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }
    public string OutputFile { get; set; }

    public static Config FromFile(string file)
    {
        if (!File.Exists(file)) 
        {
            Console.WriteLine($"Config file \"{file}\" does not exist!");
            return null;
        }

        var serializer = new XmlSerializer(typeof(Config));
        try
        {
            using var reader = new StreamReader(file);
            var config = (Config)serializer.Deserialize(reader);
            Logger.WriteLine("Config file loaded");
            return config;
        }
        catch (Exception ex)
        when(ex is IOException || ex is InvalidOperationException)
        {
            Console.WriteLine($"Error while loading config file: {ex.Message}.");
            return null;
        }
    }
}
