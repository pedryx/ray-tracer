using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace RayTracer;
/// <summary>
/// Definition of scene's objects.
/// </summary>
public class SceneDefinition
{
    /// <summary>
    /// Background color of the scene.
    /// </summary>
    public Color BackgroundColor;
    /// <summary>
    /// Materials which can be used for shapes.
    /// </summary>
    public MaterialDictionary Materials = new();
    /// <summary>
    /// Contains sources of light.
    /// </summary>
    [XmlArrayItem(typeof(AmbientLightSource), ElementName = "Ambient")]
    [XmlArrayItem(typeof(PointLightSource), ElementName = "Point")]
    public List<LightSource> LightSources = new();

    /// <summary>
    /// Dictionary wrapper for materials.
    /// </summary>
    public class MaterialDictionary : Dictionary<string, Material>, IXmlSerializable
    {
        public XmlSchema GetSchema()
            => null;

        public void ReadXml(XmlReader reader)
        {
            var root = new XmlRootAttribute()
            {
                ElementName = nameof(Materials),
            };

            var serializer = new XmlSerializer(typeof(Material[]), root);
            var materials = (Material[])serializer.Deserialize(reader);
            foreach (var material in materials)
            {
                Add(material.Name, material);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            var materials = Values.ToList();
            var serializer = new XmlSerializer(materials.GetType());
            serializer.Serialize(writer, materials);
        }
    }
}