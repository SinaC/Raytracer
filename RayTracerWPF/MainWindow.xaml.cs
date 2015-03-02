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

            Scene scene = new Scene();
            //SceneObject sphere1 = new SceneObject(new Sphere(new Vector3(2, 0, -5), 1), new Texture(Finish.BasicPhong, new Solid(RayTracer.Color.Red)));
            SceneObject sphere1 = new SceneObject(
                new Sphere(
                    new Vector3(2, 0, -5),
                    1),
                new Texture(
                    new Finish
                        {
                            Diffuse = 0,
                            Reflection = 0.5,
                            Transmission = 0.9,
                        },
                    new Solid(RayTracer.Color.White),
                    null,
                    new Interior
                        {
                            IndexOfRefraction = Interior.Glass,
                        }));
            scene.AddObject(sphere1);
            //SceneObject sphere2 = new SceneObject(new Sphere(new Vector3(-2, 0, -5), 1), new Texture(Finish.BasicDiffuse, new Solid(RayTracer.Color.Red)));
            SceneObject sphere2 = new SceneObject(new Sphere(new Vector3(-2, 0, -5), 1), new Texture(new Finish
            {
                Diffuse = 0.5,
                Reflection = 0.5
            }, new Solid(RayTracer.Color.White)));
            scene.AddObject(sphere2);
            SceneObject plane1 = new SceneObject(new Plane(new Vector3(0, 0, 1), 8), new Texture(Finish.BasicPhong, new Checkboard(1, Checkboard.Axes.XY)));
            //SceneObject plane1 = new SceneObject(new Plane(new Vector3(0, 0, 1), 8), new Texture(new Finish
            //{
            //    Diffuse = 0.5,
            //    Reflection = 0.5
            //}, new Checkboard(1, Checkboard.Axes.XY)));
            scene.AddObject(plane1);

            DotLight light1 = new DotLight(new Vector3(0, 5, -5), RayTracer.Color.White);
            scene.AddLight(light1);
            DotLight light2 = new DotLight(new Vector3(0, -5, -5), RayTracer.Color.White);
            scene.AddLight(light2);

            Camera camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -5));
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
            Vector3 direction = _tracer.Camera.ComputeRayDirection(_tracer.Width, _tracer.Height, (int)point.X, (int)point.Y);
            Ray ray = new Ray(_tracer.Camera.Eye, direction);
            RayTracer.Color color = _tracer.TraceRay(ray, 0, 1.0);
        }
    }
}
