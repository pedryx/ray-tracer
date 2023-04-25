using System;
using System.IO;
using System.Xml.Serialization;

namespace RayTracer;
/// <summary>
/// Helper class for loading object from xml files.
/// </summary>
internal static class XmlLoader
{
    /// <summary>
    /// Error message which is shown when error occur during file loading.
    /// </summary>
    private const string errorString = "Error while loading file: ";

    /// <summary>
    /// Load object from xml file.
    /// </summary>
    /// <typeparam name="T">Type of object to load.</typeparam>
    /// <param name="file">File from which to load an object.</param>
    /// <returns>Loaded object or null when error occur.</returns>
    public static T Load<T>(string file)
        where T : class
    {
        if (!File.Exists(file))
        {
            Logger.WriteLine($"Config file \"{file}\" does not exist!", LogType.Error);
            return null;
        }

        var serializer = new XmlSerializer(typeof(T));

        serializer.UnknownAttribute += Serializer_UnknownAttribute;
        serializer.UnknownElement += Serializer_UnknownElement;
        serializer.UnknownNode += Serializer_UnknownNode;
        serializer.UnreferencedObject += Serializer_UnreferencedObject;

        try
        {
            using var reader = new StreamReader(file);
            var obj = (T)serializer.Deserialize(reader);
            Logger.WriteLine("Config file loaded");

            if (obj is Config config && config.Camera == null)
                Logger.WriteLine(errorString + "Camera has not been specified.", LogType.Error);

            return obj;
        }
        catch (Exception ex)
        when (ex is IOException || ex is InvalidOperationException)
        {
            Logger.WriteLine(errorString + $"{ex.Message}.", LogType.Error);
            return null;
        }
    }

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
}
