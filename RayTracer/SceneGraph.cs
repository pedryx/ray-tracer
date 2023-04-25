using RayTracer.SceneNodes;

using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer;
/// <summary>
/// Represent graph of the scene.
/// </summary>
public class SceneGraph : IEnumerable<ScenePair>
{
    /// <summary>
    /// Root node of the scene.
    /// </summary>
    public InnerNode Root;

    public IEnumerator<ScenePair> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
