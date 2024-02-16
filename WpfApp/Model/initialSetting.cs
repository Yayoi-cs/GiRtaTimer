using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Xml.Linq;
using System.IO;
using WpfApp.Model;

namespace WpfApp.Model {
    public class initialSetting {
        private static commonData? internalData;
        public static commonData? set(string? _path) {
            if (_path == null || _path==string.Empty) {
                MessageBox.Show("Please Set GenshinImpact.exe Path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            if (!File.Exists(_path)) {
                MessageBox.Show("Don't Exits" + _path, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            Process[] process = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(_path));
            if (process.Length > 0) {
                IntPtr handle = process[0].MainWindowHandle;
                
                windowsApiConnecter.RECT rect;
                internalData= new commonData();
                if (windowsApiConnecter.GetWindowRect(handle, out rect)) {
                    internalData.left = rect.Left;
                    internalData.top = rect.Top;
                    internalData.width = rect.Right - rect.Left;
                    internalData.height = rect.Bottom - rect.Top;
                    internalData.exePath = _path;
                    return internalData;
                }
                else {
                    return null;
                }
            }
            else {
                MessageBox.Show("Cannot find process", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
