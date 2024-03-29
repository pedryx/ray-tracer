﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace RayTracer.SceneNodes;
/// <summary>
/// Inner scene node, child nodes in <see cref="Nodes"/> derives transformations and material.
/// </summary>
public sealed class InnerNode : SceneNode
{
    /// <summary>
    /// Transformation applied to child nodes.
    /// </summary>
    [XmlArrayItem("Transform")]
    public List<string> Transformations { get; init; } = new();
    /// <summary>
    /// Child nodes.
    /// </summary>
    [XmlArrayItem(typeof(InnerNode), ElementName = "Node")]
    [XmlArrayItem(typeof(LeafNode), ElementName = "Leaf")]
    public List<SceneNode> Nodes { get; init; } = new();
    /// <summary>
    /// Material applied to child nodes.
    /// </summary>
    public string Material { get; init; } = null;
}
