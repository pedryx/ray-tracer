using RayTracer.Shapes;

using System.Collections.Generic;
using System.Xml.Serialization;

namespace RayTracer.SceneNodes;
/// <summary>
/// Leaf scene node, contains shapes which inherits all transformations and material specified by parent
/// nodes.
/// </summary>
public class LeafNode : SceneNode
{
    /// <summary>
    /// Contains shapes which belongs to this node.
    /// </summary>
    [XmlArrayItem(typeof(Plane))]
    [XmlArrayItem(typeof(Sphere))]
    public List<Shape> Shapes = new();
}
