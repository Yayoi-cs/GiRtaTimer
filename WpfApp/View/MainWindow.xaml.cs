﻿using System.Diagnostics;
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
using WpfApp.Model;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        readonly DispatcherTimer uiTimer = new DispatcherTimer();
        private string? _path;

        public MainWindow() {
            InitializeComponent();
            uiTimer .Interval = new TimeSpan(0,0,0,0,10);
            uiTimer.Tick += UiTimer_Tick;
            this.KeyDown += MainWindow_KeyDown;

        }

        private void UiTimer_Tick(object? sender, EventArgs e) {
            
            var basic = timerController.basicStopWatch.Elapsed;
            var rlt = timerController.rltStopWatch.Elapsed;
            rlt_h.Text = rlt.Hours == 0 ? "" : rlt.Hours.ToString() + ":";
            rlt_m.Text = (rlt.Minutes == 0 && rlt.Hours == 0) ? "" : rlt.Minutes.ToString().PadLeft(2, '0') + ":";
            rlt_s.Text = (rlt.Minutes == 0 && rlt.Hours == 0) ? rlt.Seconds.ToString() : rlt.Seconds.ToString().PadLeft(2, '0');
            rlt_ms.Text = (rlt.Milliseconds / 10).ToString().PadLeft(2, '0');

            basic_h.Text = basic.Hours == 0 ? "" : basic.Hours.ToString() + ":";
            basic_m.Text = (basic.Minutes == 0 && basic.Hours == 0) ? "" : basic.Minutes.ToString().PadLeft(2, '0') + ":";
            basic_s.Text = (basic.Minutes == 0 && basic.Hours == 0) ? basic.Seconds.ToString() : basic.Seconds.ToString().PadLeft(2, '0');
            basic_ms.Text = (basic.Milliseconds / 10).ToString().PadLeft(2,'0');
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.O) {
                
                if (ModelConnecter.init(_path==null?"":_path)) {
                    OpenDialogHost.IsEnabled = false;
                    uiTimer.Start();
                    ModelConnecter.misure();
                }
            }
            if(e.Key == Key.L) {
                timerController._stop();
                uiTimer.Stop();
            }
        }

        private void reSetTimer() {
            uiTimer.Stop();
            OpenDialogHost.IsEnabled = true;
        }


        private void OpenDialogHost_Click(object sender, RoutedEventArgs e) {
            SettingDialog.IsOpen = true;
        }

        private void SettingDialogClose_Click(object sender, RoutedEventArgs e) {
            Title.Text=Title_Input.Text;
            _path=Path_Input.Text;
            SettingDialog.IsOpen=false;
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