using OpenCvSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using WpfApp.Model;

namespace WpfApp.Model {
    public class myCapture {
        private static bool blackScreen;
        public static bool Cap(commonData? internalData) {
            if (internalData == null) {
                return false;
            }
            Bitmap screenshot = new Bitmap(
                internalData.width,
                internalData.height);
            Graphics graphics = Graphics.FromImage(screenshot as System.Drawing.Image);
            graphics.CopyFromScreen(
                internalData.left,
                internalData.top,
                0, 0,
                screenshot.Size);
            Image<Rgba32> image = myConverter.ConvertToImageSharpImage(screenshot);
            bool isLoad = lAnalyze.analyzeCapture(image);
            
            if(isLoad && timerController.rltState && !blackScreen) {
                analyzeCap(image);
            }
            else if(isLoad && blackScreen) {
                return false;
            }
            if(isLoad && !blackScreen) {
                return true;
            }
            else if(!isLoad){
                blackScreen = false;
            }
            return false;
        }

        private static bool analyzeCap(Image<Rgba32> image) {
            System.Windows.Point _point;
            windowsApiConnecter.GetCursorPos(out _point);
            double mouseLeft = _point.X;
            double windowMiddle = (double)(ModelConnecter.parentData.left + ModelConnecter.parentData.width / 2);
            
            if (mouseLeft < windowMiddle) {
                //Mouse Cursor is right position
                Image<Rgba32> newSC = CuttingRight(image);
                blackScreen = IsSingleColor(newSC);
            }
            else {
                //Mouse Cursor is left position
                Image<Rgba32> newSC = CuttingLeft(image);
                blackScreen = IsSingleColor(newSC); 
            }
            return blackScreen;
        }
        private static Image<Rgba32> CuttingLeft(Image<Rgba32> image) {
            int newWidth = image.Width / 4;
            int newHeight = image.Height / 4;

            image.Mutate(x => x.Crop(new SixLabors.ImageSharp.Rectangle(newWidth, newHeight, newWidth, newHeight)));
            return image;
        }

        private static Image<Rgba32> CuttingRight(Image<Rgba32> image) {
            int newWidth = image.Width / 2;
            int newHeight = image.Height / 4;

            image.Mutate(x => x.Crop(new SixLabors.ImageSharp.Rectangle(newWidth, newHeight, newWidth/2, newHeight)));
            return image;
        }

        private static bool IsSingleColor(Image<Rgba32> image) {
            Rgba32 referenceColor = image[0, 0];
            for (int y = 0; y < image.Height; y++) {
                for (int x = 0; x < image.Width; x++) {
                    if (image[x, y] != referenceColor) {
                        return false;
                    }
                }
            }
            if (isWhite(image)) {
                return false;
            }
            return true;
        }

        private static bool isWhite(Image<Rgba32> image) {
            Mat mat = myConverter.ConvertImageSharpToMat(image);

            Scalar averageColor = Cv2.Mean(mat);
            double threshold = 200;
            return averageColor.Val0 > threshold;
        }
    }
}