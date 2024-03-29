using System.Collections.Generic;
using System.Linq;
using RayTracer.Engine.Lights;

namespace RayTracer.Engine;

public class Scene
{
    private readonly List<SceneObject> _sceneObjects;
    private readonly List<LightBase> _lights;

    public IEnumerable<SceneObject> SceneObjects
    {
        get { return _sceneObjects; }
    }

    public IEnumerable<LightBase> Lights
    {
        get { return _lights; }
    }

    public Scene()
    {
        _sceneObjects = new List<SceneObject>();
        _lights = new List<LightBase>();
    }

    public void AddObject(SceneObject geometry)
    {
        _sceneObjects.Add(geometry);
    }

    public void RemoveObject(SceneObject geometry)
    {
        _sceneObjects.Remove(geometry);
    }

    public void AddLight(LightBase light)
    {
        _lights.Add(light);
    }

    public void RemoveLight(LightBase light)
    {
        _lights.Remove(light);
    }

    public SceneObject Intersects(Ray ray)
    {
        return _sceneObjects.FirstOrDefault(o => o.Geometry != null && o.Geometry.HasIntersections(ray));
        //foreach(SceneObject o in SceneObjects)
        //{
        //    float t;
        //    bool intersects = o.Geometry.ComputeNearestIntersection(ray, out t);
        //    if (intersects && t >= 0)
        //        return o;
        //}
        //return null;
    }

    public Intersection NearestIntersection(Ray ray)
    {
        return Intersections(ray).FirstOrDefault();
    }

    public IEnumerable<Intersection> Intersections(Ray ray)
    {
        return _sceneObjects
            .Select(o => ComputeIntersection(o, ray))
            .Where(i => i != null && i.T > 0)
            .OrderBy(i => i.T);
    }

    private Intersection ComputeIntersection(SceneObject o, Ray ray)
    {
        if (o == null || o.Geometry == null)
            return null;
        float t;
        bool intersects = o.Geometry.ComputeNearestIntersection(ray, out t);
        if (!intersects || t < 0)
            return null;
        return new Intersection(o, ray, t);
    }
}
