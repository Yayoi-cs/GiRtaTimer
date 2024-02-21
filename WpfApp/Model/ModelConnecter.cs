using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;

namespace WpfApp.Model {
    public class ModelConnecter {
        public static commonData? parentData = new commonData();
        public static keyData? keyData = new keyData();
        public static bool init(string path) {
            commonData? tmpData;
            tmpData = initialSetting.set(path);
            if(tmpData != null ) {
                parentData = tmpData;
                timerController._start();
                return true;
            }
            return false;
        }
        public static async void misure() {
            while (timerController.basicState) {
                Task<bool> capTask = Task.Run(() => myCapture.Cap(parentData));
                bool isLoad = await capTask;

                if (isLoad && timerController.rltState) {
                    
                    timerController._lStop();
                }
                else if (!isLoad && !timerController.rltState) {
                    timerController._lStart();
                }
                await Task.Delay(50);
            }
        }
    }
}
