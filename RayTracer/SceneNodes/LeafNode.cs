using RayTracer.Shapes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.SceneNodes;
internal class LeafNode : ISceneNode
{
    public List<Shape> Shapes = new();
}
