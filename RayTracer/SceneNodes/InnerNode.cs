using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneNodes;
internal class InnerNode : ISceneNode
{
    public Matrix4x4 Transform;
    public List<ISceneNode> Nodes = new();
}
