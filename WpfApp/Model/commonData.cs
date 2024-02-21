using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
namespace WpfApp.Model {
    public class commonData {
        public string? exePath { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int left { get; set; }
        public int top { get; set; }
    }
    //This is same to JsonObject.

    public class keyData {
        public Key startKey { get; set; }
        public Key stopKey { get; set; }
        public Key resetKey { get; set; }
        public string? description { get; set; }
        public string? path { get; set; }
    }

    [JsonObject]
    public class userData {
        [JsonProperty("startKey")]
        public Key startKey { get; set; }
        [JsonProperty("stopKey")]
        public Key stopKey { get; set; }
        [JsonProperty("resetKey")]
        public Key resetKey { get; set; }
        [JsonProperty("description")]
        public string? description { get; set; }
        [JsonProperty("path")]
        public string? path { get; set; }
    }
}
