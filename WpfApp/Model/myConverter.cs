using OpenCvSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WpfApp.Model {
    public class myConverter {
        

        public static Image<Rgba32> ConvertToImageSharpImage(Bitmap bm) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                bm.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return SixLabors.ImageSharp.Image.Load<Rgba32>(memoryStream.ToArray());
            }
        }

        public static Mat ConvertImageSharpToMat(Image<Rgba32> image) {
            MemoryStream memoryStream = new MemoryStream();

            // ImageSharpのImageをPNG形式のバイト列にエンコード
            image.SaveAsPng(memoryStream);

            // バイト列からOpenCVSharp4のMatに変換
            Mat mat = Cv2.ImDecode(memoryStream.ToArray(), ImreadModes.Color);

            return mat;
        }
    }
}
