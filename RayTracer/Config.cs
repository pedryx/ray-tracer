using System;
using System.IO;
using System.Xml.Serialization;


namespace RayTracer;
/// <summary>
/// Represent program configuration.
/// </summary>
public class Config
{
    private const string errorString = "Error while loading config file: ";

    /// <summary>
    /// File where output image should be stored.
    /// </summary>
    public string OutputFile = "demo.pfm";
    /// <summary>
    /// Definition of scene objects.
    /// </summary>
    public SceneDefinition Scene = new();
    public Camera Camera;
    public bool Shadows = true;
    public bool Reflections = true;
    public int maxDepth = 8;

    public static Config FromFile(string file)
    {
        if (!File.Exists(file)) 
        {
            Logger.WriteLine($"Config file \"{file}\" does not exist!", LogType.Error);
            return null;
        }

        var serializer = new XmlSerializer(typeof(Config));

        serializer.UnknownAttribute += Serializer_UnknownAttribute;
        serializer.UnknownElement += Serializer_UnknownElement;
        serializer.UnknownNode += Serializer_UnknownNode;
        serializer.UnreferencedObject += Serializer_UnreferencedObject;

        try
        {
            using var reader = new StreamReader(file);
            var config = (Config)serializer.Deserialize(reader);
            Logger.WriteLine("Config file loaded");

            if (config.Camera == null)
                Logger.WriteLine(errorString + "Camera has not been specified.", LogType.Error);

            return config;
        }
        catch (Exception ex)
        when(ex is IOException || ex is InvalidOperationException)
        {
            Logger.WriteLine(errorString + $"{ex.Message}.", LogType.Error);
            return null;
        }
    }

    #region Error Handling
    private static void Serializer_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        => Logger.WriteLine
        (
            errorString + $"Unreferenced object with ID: {e.UnreferencedId}.",
            LogType.Error
        );

    private static void Serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        => Logger.WriteLine
        (
            errorString + $"Unknown node with name: {e.Name}" +
            $" on ({e.LineNumber}, {e.LinePosition}).",
            LogType.Error
        );

    private static void Serializer_UnknownElement(object sender, XmlElementEventArgs e)
        => Logger.WriteLine
        (
            errorString + $"Unknown element, expected: {e.ExpectedElements}" +
            $" on ({e.LineNumber}, {e.LinePosition}).",
            LogType.Error
        );

    private static void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        => Logger.WriteLine
        (
            errorString + $"Unknown attribute, expected: {e.ExpectedAttributes}" +
            $" on ({e.LineNumber}, {e.LinePosition}).",
            LogType.Error
        );
    #endregion
}
