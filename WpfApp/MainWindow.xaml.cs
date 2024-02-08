using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Collections.Generic;
using System.IO;

namespace WpfApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        readonly Stopwatch rltStopWatch = new Stopwatch();
        readonly Stopwatch basicStopWatch = new Stopwatch();
        readonly DispatcherTimer timer = new DispatcherTimer();

        public MainWindow() {
            InitializeComponent();

            _IsLoadGl = false;
            isScaning = false;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            this.KeyDown += MainWindow_KeyDown;

        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.NumPad6) {
                initialSetting();
                reSetTimer();
                rltStopWatch.Start();
                basicStopWatch.Start();
                timer.Start();
                OpenDialogHost.IsEnabled = false;
            }
            if(e.Key == Key.NumPad4) {
                rltStopWatch.Stop();
                basicStopWatch.Stop();
                timer.Stop();
            }
        }
        /*
         private void Timer_Tick(object? sender, EventArgs e) {
            var basic = basicStopWatch.Elapsed;
            var rlt = rltStopWatch.Elapsed;
            rlt_h.Text = rlt.Hours == 0 ? "" : rlt.Hours.ToString()+":";
            rlt_m.Text = (rlt.Minutes == 0 && rlt.Hours == 0) ? "" : rlt.Minutes.ToString().PadLeft(2, '0')+":";
            rlt_s.Text = (rlt.Minutes == 0 && rlt.Hours == 0) ? rlt.Seconds.ToString() : rlt.Seconds.ToString().PadLeft(2, '0');
            rlt_ms.Text = (rlt.Milliseconds / 100).ToString();
            myCaptureScreen();
        }
         */

        private bool isScaning;
        private void Timer_Tick(object? sender, EventArgs e) {
            var basic = basicStopWatch.Elapsed;
            var rlt = rltStopWatch.Elapsed;
            rlt_h.Text = rlt.Hours == 0 ? "" : rlt.Hours.ToString() + ":";
            rlt_m.Text = (rlt.Minutes == 0 && rlt.Hours == 0) ? "" : rlt.Minutes.ToString().PadLeft(2, '0') + ":";
            rlt_s.Text = (rlt.Minutes == 0 && rlt.Hours == 0) ? rlt.Seconds.ToString() : rlt.Seconds.ToString().PadLeft(2, '0');
            rlt_ms.Text = (rlt.Milliseconds / 100).ToString();

            basic_h.Text = basic.Hours == 0 ? "" : basic.Hours.ToString() + ":";
            basic_m.Text = (basic.Minutes == 0 && basic.Hours == 0) ? "" : basic.Minutes.ToString().PadLeft(2, '0') + ":";
            basic_s.Text = (basic.Minutes == 0 && basic.Hours == 0) ? basic.Seconds.ToString() : basic.Seconds.ToString().PadLeft(2, '0');
            basic_ms.Text = (basic.Milliseconds / 100).ToString();

            if (isScaning) {
                myCaptureScreen();
                isScaning = false;
            }
            else {
                isScaning = true;
            }
        }
        private bool _IsLoadGl;
        //Global variable hold if capture is Loading or not
        //False :Not Loading  True :Loading

        private void TimerControler(bool isLoad) {
            if (isLoad && !_IsLoadGl) {
                //Loading from not NonLoading
                rltStopWatch.Stop();
            }
            else {
                //NonLoading from loading
                //whiteOut open
                rltStopWatch.Start();
            }
        }

        private void reSetTimer() {
            rltStopWatch.Stop();
            rltStopWatch.Reset();
            basicStopWatch.Stop();
            basicStopWatch.Reset();
            timer.Stop();
            OpenDialogHost.IsEnabled = true;
        }

        private bool isSetLect;
        private int width;
        private int height;
        private int left;
        private int top;
        
        string? name;
        private void initialSetting() {
            if (name == null) {
                MessageBox.Show("Please Set GenshinImpact.exe Path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                reSetTimer();
                return;
            }
            if (!File.Exists(name)) {
                reSetTimer();
                MessageBox.Show("Don't Exits" + name, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Process[] process = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(name));
            if (process.Length > 0) {
                IntPtr handle = process[0].MainWindowHandle;
                RECT rect;
                if (GetWindowRect(handle, out rect)) {
                    isSetLect = true;
                    left = rect.Left; top = rect.Top;
                    width = rect.Right - rect.Left;
                    height = rect.Bottom - rect.Top;
                }
                else {
                    isSetLect = false;
                }

            }
            else {
                MessageBox.Show("Cannot find process", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void myCaptureScreen() {

            if (isSetLect) {
                Bitmap screenshot = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(screenshot as System.Drawing.Image);
                graphics.CopyFromScreen(left, top, 0, 0, screenshot.Size);
                bool isLoad = lAnalyze.analyzeCapture(screenshot);
                TimerControler(isLoad);
            }
            else {
                basic_h.Text = "";
                basic_m.Text = "";
                basic_ms.Text = "";
                reSetTimer();
                return;
            }
        }

        private void OpenDialogHost_Click(object sender, RoutedEventArgs e) {
            SettingDialog.IsOpen = true;
        }

        private void SettingDialogClose_Click(object sender, RoutedEventArgs e) {
            Title.Text=Title_Input.Text;
            name=Path_Input.Text;
            SettingDialog.IsOpen=false;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT IpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left; public int Top; public int Right; public int Bottom;
        }

        private void Explore_Open_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "GenshinImpact";
            dialog.DefaultExt = ".exe";
            bool? result = dialog.ShowDialog();
            if (  result == true) {
                Path_Input.Text= dialog.FileName;
            }
        }
    }
}