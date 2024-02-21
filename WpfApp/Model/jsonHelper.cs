using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp.Model {
    public class jsonHelper {
        private static string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        public static void write() {
            userData jsonObj = new userData() {
                startKey = ModelConnecter.keyData.startKey,
                stopKey = ModelConnecter.keyData.stopKey,
                resetKey = ModelConnecter.keyData.resetKey,
                description = ModelConnecter.keyData.description,
                path = ModelConnecter.keyData.path
            };
            string jsonStr = JsonConvert.SerializeObject(jsonObj);
            File.WriteAllText(ConfigFilePath, jsonStr);
            Console.WriteLine(jsonStr);
        }
        public static void read() {
            if (!File.Exists(ConfigFilePath)) {
                ModelConnecter.keyData.startKey = Key.U;
                ModelConnecter.keyData.stopKey = Key.K;
                ModelConnecter.keyData.resetKey = Key.O;
                ModelConnecter.keyData.description = "";
                ModelConnecter.keyData.path = "";
                return;
            }
            string jsonStr = File.ReadAllText(ConfigFilePath);
            userData jsonObj = JsonConvert.DeserializeObject<userData>(jsonStr);
            ModelConnecter.keyData = new keyData() {
                startKey = jsonObj.startKey,
                stopKey = jsonObj.stopKey,
                resetKey = jsonObj.resetKey,
                description= jsonObj.description,
                path = jsonObj.path,
            };
        }
    }
}
