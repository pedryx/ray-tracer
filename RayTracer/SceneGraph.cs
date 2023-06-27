using OpenTK.Mathematics;

using RayTracer.SceneNodes;

using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer;
/// <summary>
/// Represent graph of the scene.
/// </summary>
public class SceneGraph : IEnumerable<ShapeNode>
{
    private const string TransformFormatError = "There is an error in one of your transforms.";

    /// <summary>
    /// Root node of the scene.
    /// </summary>
    public InnerNode Root { get; private set; }

    public SceneGraph(InnerNode root)
    {
        Root = root;
    }

    public IEnumerator<ShapeNode> GetEnumerator()
    {
        foreach (var node in Traverse(Root, Matrix4d.Identity, null))
        {
            yield return node;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Traverse subtree of scene graph.
    /// </summary>
    /// <param name="root">Root of subtree to traverse.</param>
    /// <param name="transform">Transformation of current subtree.</param>
    /// <param name="material">Material of current subtree.</param>
    private IEnumerable<ShapeNode> Traverse(InnerNode root, Matrix4d transform, string material)
    {
        foreach (var transformName in root.Transformations)
        {
            transform = GetTransformMatrix(transformName) * transform;
        }

        if (root.Material != null)
            material = root.Material;

        foreach (var node in root.Nodes)
        {
            if (node is LeafNode leafNode)
            {
                foreach (var shape in leafNode.Shapes)
                {
                    yield return new ShapeNode(shape, transform, material);
                }
            }
            
            if (node is InnerNode innerNode)
            {
                foreach (var shapeNode in Traverse(innerNode, transform, material))
                {
                    yield return shapeNode;
                }
            }
        }
    }

    /// <summary>
    /// Parse transformation matrix.
    /// </summary>
    /// <param name="transform">String representation of transformation matrix to parse.</param>
    /// <returns></returns>
    private static Matrix4d GetTransformMatrix(string transform)
    {
        string[] tokens = transform.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

        string name = tokens[0].ToLower();

        switch (name)
        {
            case "translate":
                return Matrix4d.CreateTranslation(ParseVector(tokens));
            case "rotatex":
                return Matrix4d.CreateRotationX(ParseNumber(tokens));
            case "rotatey":
                return Matrix4d.CreateRotationY(ParseNumber(tokens));
            case "rotatez":
                return Matrix4d.CreateRotationZ(ParseNumber(tokens));
            case "scale":
                return Matrix4d.Scale(ParseNumber(tokens));
            default:
                Logger.WriteLine(TransformFormatError, LogType.Error);
                throw new Exception(TransformFormatError);
        }
    }

    private static double ParseNumber(string[] tokens)
    {
        if (tokens.Length == 2 && double.TryParse(tokens[1], out double number))
            return number;

        Logger.WriteLine(TransformFormatError, LogType.Error);
        throw new Exception(TransformFormatError);
    }

    private static Vector3d ParseVector(string[] tokens)
    {
        if (
            tokens.Length != 4 ||
            !double.TryParse(tokens[1], out double x) ||
            !double.TryParse(tokens[2], out double y) ||
            !double.TryParse(tokens[3], out double z)
)
        {
            Logger.WriteLine(TransformFormatError, LogType.Error);
            throw new Exception(TransformFormatError);
        }

        return new Vector3d(x, y, z);
    }
}
