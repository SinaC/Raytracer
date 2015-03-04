using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MathUtils;
using RayTracer;
using RayTracer.Cameras;
using RayTracer.Geometries;
using RayTracer.Lights;
using RayTracer.Normals;
using RayTracer.Pigments;
using RayTracer.Turbulences;

namespace RayTraceWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int RenderWidth = 400;
        private const int RenderHeight = 400;
        private readonly WriteableBitmap _writeableBitmap = new WriteableBitmap(RenderWidth, RenderHeight, 96, 96, PixelFormats.Bgra32, null);

        private readonly Tracer _tracer;

        public MainWindow()
        {
            InitializeComponent();

            MainImage.Source = _writeableBitmap;

            Material glassTexture = new Material(
                new Finish
                    {
                        Diffuse = 0.0,
                        Reflection = 0.2,
                        Transmission = 0.8,
                    },
                new Solid(RayTracer.Color.White),
                null,
                new Interior
                    {
                        IndexOfRefraction = Interior.Glass,
                    });

            Scene scene = new Scene();
            //SceneObject sphere1 = new SceneObject(new Sphere(new Vector3(2, 0, -5), 1), new Texture(Finish.BasicPhong, new Solid(RayTracer.Color.Red)));
            //SceneObject sphere1 = new SceneObject(new Sphere(new Vector3(2, 0, -5), 1), new Texture(Finish.BasicPhong, new Noisy(2)));
            SceneObject sphere1 = new SceneObject(new Sphere(new Vector3(2, 0, -5), 1), new Material(Finish.BasicPhong, new Solid(RayTracer.Color.Red), new Bumps(2)));
            //SceneObject sphere1 = new SceneObject(
            //    new Sphere(new Vector3(2, 0, -5),1), glassTexture);
            scene.AddObject(sphere1);
            //SceneObject sphere2 = new SceneObject(new Sphere(new Vector3(-2, 0, -5), 1), new Texture(Finish.BasicDiffuse, new Checkboard(RayTracer.Color.Red, RayTracer.Color.Yellow)));
            SceneObject sphere2 = new SceneObject(new Sphere(new Vector3(-2, 0, -5), 1), glassTexture);
            //SceneObject sphere2 = new SceneObject(new Sphere(new Vector3(-2, 0, -5), 1), new Texture(new Finish
            //{
            //    Diffuse = 0.5,
            //    Reflection = 0.5
            //}, new Solid(RayTracer.Color.White)));
            scene.AddObject(sphere2);
            //SceneObject plane1 = new SceneObject(new Plane(new Vector3(0, 0, 1), 8), new Texture(Finish.BasicPhong, new Checkboard(1, Checkboard.Axes.XY)));
            SceneObject plane1 = new SceneObject(
                new Plane(new Vector3(0, 0, 1), 8),
                new Material(
                    new Finish
                        {
                            Diffuse = 0.5,
                            Reflection = 0.5,
                            Phong = 0.5,
                            PhongSize = 40,
                        },
                    new Checkboard(RayTracer.Color.White, RayTracer.Color.Black, 1, Checkboard.Axes.XY)));
            scene.AddObject(plane1);

            //SceneObject torus1 = new SceneObject(new Torus(2, 0.5), glassTexture);
            //SceneObject torus1 = new SceneObject(new Torus(2, 0.5), new Texture(Finish.BasicDiffuse, new Solid(RayTracer.Color.Cyan)));
            //scene.AddObject(torus1);

            DotLight light1 = new DotLight(new Vector3(0, 5, -5), RayTracer.Color.White);
            scene.AddLight(light1);
            DotLight light2 = new DotLight(new Vector3(0, -5, -5), RayTracer.Color.White);
            scene.AddLight(light2);
            //DotLight light3 = new DotLight(new Vector3(0, 0, 20), RayTracer.Color.White);
            //scene.AddLight(light3);
            //DotLight light1 = new DotLight(new Vector3(0, 5, 0), RayTracer.Color.White);
            //scene.AddLight(light1);

            //Camera camera = new Camera(new Vector3(0, 0, 10), new Vector3(0, 0, -5));
            Camera camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -5));
            //Camera camera = new Camera(new Vector3(0, 1, 0), new Vector3(0, 1, -5));
            //Camera camera = new Camera(new Vector3(0, 1, 0), new Vector3(0, 0, -5));
            //Camera camera = new Camera(new Vector3(5, 0, -5), new Vector3(1, 0, -5));
            //Camera camera = new Camera(new Vector3(0, 0, -5), new Vector3(-1, 0, -5));

            _tracer = new Tracer(scene, camera, RenderWidth, RenderHeight);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            RayTracer.Color[,] bitmap = _tracer.Render();
            sw.Stop();

            MainText.Text = String.Format("{0} ms Color:{1} Vector3:{2}", sw.ElapsedMilliseconds, RayTracer.Color.AllocationCount, Vector3.AllocationCount);
            DisplayBitmap(bitmap);
        }

        private void DisplayBitmap(RayTracer.Color[,] bitmap)
        {
            // Create an array of pixels to contain pixel color values
            uint[] pixels = new uint[RenderWidth * RenderHeight];
            int offset = 0;
            for (int y = 0; y < RenderHeight; y++)
            {
                for (int x = 0; x < RenderWidth; x++)
                {
                    RayTracer.Color color = bitmap[x, y];

                    //float value = SimplexNoise.Generate((float)x/10, (float)y/10) * 128 + 128;
                    //byte red = (byte)value;

                    byte red = ConvertColorComponent(color.R);
                    byte green = ConvertColorComponent(color.G);
                    byte blue = ConvertColorComponent(color.B);
                    byte alpha = 255;

                    pixels[offset] = (uint)((alpha << 24) + (red << 16) + (green << 8) + blue);
                    offset++;
                }
            }

            // apply pixels to bitmap
            _writeableBitmap.WritePixels(new Int32Rect(0, 0, RenderWidth, RenderHeight), pixels, RenderWidth * _writeableBitmap.Format.BitsPerPixel / 8, 0);
        }

        private static byte ConvertColorComponent(double c)
        {
            return (byte) (((int) Math.Max(0, Math.Min(255, c*255))) & 255);
        }

        private void MainImage_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(MainImage);
            int x = (int) point.X;
            int y = _tracer.Height - (int) point.Y;
            Vector3 direction = _tracer.Camera.ComputeRayDirection(_tracer.Width, _tracer.Height, x, y);
            Ray ray = new Ray(_tracer.Camera.Eye, direction);
            RayTracer.Color color = _tracer.TraceRay(ray, 0, 1.0);
        }
    }
}
