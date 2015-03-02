using System;
using System.IO;
using ImageUtils;
using MathUtils;
using RayTracer;
using RayTracer.Cameras;
using RayTracer.Geometries;
using RayTracer.Lights;
using RayTracer.Pigments;

namespace RayTraceConsole
{
    class Program
    {
        static void TestPlane()
        {
            Ray ray = new Ray(new Vector3(2,3,4), new Vector3(0.577,0.577,0.577));
            Plane plane = new Plane(new Vector3(1,0,0), -7);
            double t;
            bool i = plane.ComputeNearestIntersection(ray, out t);
            //t equals 8.66
        }

        static void TestSphere()
        {
            Ray ray = new Ray(new Vector3(0,0,0), new Vector3(0,0,-1));
            Sphere sphere = new Sphere(new Vector3(0,0,-10), 8);
            double t;
            bool i = sphere.ComputeNearestIntersection(ray, out t);
            //t equals 2
        }

        static void TestIntersections()
        {
            Vector3 location = new Vector3(0, 0, 0);
            Vector3 destination = new Vector3(0, 0, -1);
            Camera camera = new Camera(location, destination);

            Console.WriteLine("Camera"); System.Diagnostics.Debug.WriteLine("Camera");
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                {
                    Vector3 dir = camera.ComputeRayDirection(x, y);
                    Console.WriteLine("x:{0} y:{1} dir:{2}", x, y, dir);
                }

            Console.WriteLine("Sphere"); System.Diagnostics.Debug.WriteLine("Sphere");
            Vector3 center1 = new Vector3(0, 0, -10);
            Sphere sphere1 = new Sphere(center1, 8);
            for (int y = -5; y <= 5; y++)
            {
                for (int x = -5; x <= 5; x++)
                {
                    Vector3 direction = camera.ComputeRayDirection(x, y);
                    Ray ray = new Ray(camera.LookAt, direction);

                    //bool intersects = sphere.HasIntersections(ray);
                    double t;
                    bool intersects = sphere1.ComputeNearestIntersection(ray, out t);

                    Console.Write(intersects ? "1" : "0");
                    System.Diagnostics.Debug.WriteLine("{0},{1} => {2} {3}", x, y, intersects, t);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Plane"); System.Diagnostics.Debug.WriteLine("Plane");
            Vector3 direction1 = new Vector3(0, 1, 0);
            Plane plane1 = new Plane(direction1, 0);
            for (int y = -5; y <= 5; y++)
            {
                for (int x = -5; x <= 5; x++)
                {
                    Vector3 direction = camera.ComputeRayDirection(x, y);
                    Ray ray = new Ray(camera.LookAt, direction);

                    //bool intersects = plane1.HasIntersections(ray);
                    double t;
                    bool intersects = plane1.ComputeNearestIntersection(ray, out t);

                    Console.Write(intersects ? "1" : "0");
                    System.Diagnostics.Debug.WriteLine("{0},{1} => {2} {3}", x, y, intersects, t);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            //TestPlane();
            //TestSphere();
            //TestIntersections();

            const int width = 200;
            const int height = 200;

            Scene scene = new Scene();
            SceneObject sphere1 = new SceneObject(new Sphere(new Vector3(2, 0, -5), 1), new Texture(new Finish(), new Solid(new Color(1,0,0))));
            scene.AddObject(sphere1);
            SceneObject sphere2 = new SceneObject(new Sphere(new Vector3(-2, 0, -5), 1), new Texture(new Finish(), new Solid(new Color(1, 1, 1))));
            scene.AddObject(sphere2);
            SceneObject plane1 = new SceneObject(new Plane(new Vector3(0, 0, 1), 8), new Texture(new Finish(), new Solid(new Color(1, 1, 1))));
            scene.AddObject(plane1);

            DotLight light1 = new DotLight(new Vector3(0, 5, -5), new Color(1, 1, 1));
            scene.AddLight(light1);
            DotLight light2 = new DotLight(new Vector3(0, -5, -5), new Color(0, 0, 1));
            scene.AddLight(light2);

            Camera camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -1));

            Tracer rayTracer = new Tracer(scene, camera, width, height);
            Color[,] bitmap = rayTracer.Render();

            byte[] rgbBitmap = new byte[width*height*3];
            int offsetDest = 0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Color color = bitmap[x, y];
                    rgbBitmap[offsetDest + 0] = ConvertColorComponent(color.R);
                    rgbBitmap[offsetDest + 1] = ConvertColorComponent(color.G);
                    rgbBitmap[offsetDest + 2] = ConvertColorComponent(color.B);

                    offsetDest += 3;
                }
            using (Stream stream = new FileStream(@"d:\temp2\dump.tga", FileMode.Create))
                TgaWriter.Write(rgbBitmap, width, height, stream);
        }

        private static byte ConvertColorComponent(double c)
        {
            return (byte)(((int)Math.Max(0, Math.Min(255, c * 255))) & 255);
        }
    }
}
