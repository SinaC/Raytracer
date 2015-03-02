using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MathUtils;
using RayTracer;
using RayTracer.Geometries;
using RayTracer.Lights;
using RayTracer.Pigments;

namespace RayTraceWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int RenderWidth = 200;
        private const int RenderHeight = 200;
        private readonly WriteableBitmap _writeableBitmap = new WriteableBitmap(RenderWidth, RenderHeight, 96, 96, PixelFormats.Bgra32, null);

        public MainWindow()
        {
            InitializeComponent();

            MainImage.Source = _writeableBitmap;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Scene scene = new Scene();
            SceneObject sphere1 = new SceneObject(new Sphere(new Vector3(2, 0, -5), 1), new Texture(Finish.BasicPhong, new Solid(RayTracer.Color.Red)));
            scene.AddObject(sphere1);
            SceneObject sphere2 = new SceneObject(new Sphere(new Vector3(-2, 0, -5), 1), new Texture(Finish.BasicDiffuse, new Solid(RayTracer.Color.White)));
            scene.AddObject(sphere2);
            SceneObject plane1 = new SceneObject(new Plane(new Vector3(0, 0, 1), 8), new Texture(Finish.BasicPhong, new Checkboard(1, Checkboard.Axes.XY)));
            scene.AddObject(plane1);

            DotLight light1 = new DotLight(new Vector3(0, 5, -5), RayTracer.Color.White);
            scene.AddLight(light1);
            DotLight light2 = new DotLight(new Vector3(0, -5, -5), RayTracer.Color.Blue);
            scene.AddLight(light2);

            Camera camera = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, -1));

            Tracer rayTracer = new Tracer(scene, camera, RenderWidth, RenderHeight);
            RayTracer.Color[,] bitmap = rayTracer.Render();

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
    }
}
