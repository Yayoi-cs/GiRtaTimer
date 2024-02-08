using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp
{
    public class lAnalyze
    {
        public static bool analyzeCapture(Bitmap img) {
            Image<Rgba32> image = ConvertToImageSharpImage(img);

            int quarterHeight = image.Height;

            var samplePoints = ExtractSamplePoints(image.Width, quarterHeight, 8);

            var sampleColors = GetSampleColors(image, samplePoints);

            bool isSingleColor = AreColorsSame(sampleColors);

            return isSingleColor;
        }

        private static Image<Rgba32> ConvertToImageSharpImage(Bitmap bm) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                bm.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return SixLabors.ImageSharp.Image.Load<Rgba32>(memoryStream.ToArray());
            }
        }

        private static SixLabors.ImageSharp.Point[] ExtractSamplePoints(int imageWidth, int quarterHeight, int numberOfPoints) {
            SixLabors.ImageSharp.Point[] points = new SixLabors.ImageSharp.Point[numberOfPoints * 2];

            for (int i = 0; i < numberOfPoints; i++) {
                int x = (int)((i + 0.5) * imageWidth / numberOfPoints);
                int y = quarterHeight / 4;
                points[i] = new SixLabors.ImageSharp.Point(x, y);
            }

            for (int i = numberOfPoints; i < numberOfPoints * 2; i++) {
                int x = (int)((i - numberOfPoints + 0.5) * imageWidth / numberOfPoints);
                int y = quarterHeight * 2 / 3;
                points[i] = new SixLabors.ImageSharp.Point(x, y);
            }
            return points;
        }

        private static Rgba32[] GetSampleColors(Image<Rgba32> image, SixLabors.ImageSharp.Point[] samplePoints) {

            Rgba32[] colors = samplePoints.Select(point => image[point.X, point.Y]).ToArray();

            return colors;
        }

        private static bool AreColorsSame(Rgba32[] colors) {
            int count = 0;
            for (int i = 1; i < colors.Length; i++) {
                if (colors[i] != colors[0]) {
                    count++;
                    if (count > 1) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
