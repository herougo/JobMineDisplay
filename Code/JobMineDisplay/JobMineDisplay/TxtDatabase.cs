using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobMineDisplay {
    class TxtKeyValuePair {
        public static string text_file_location = AppDomain.CurrentDomain.BaseDirectory + "DatabaseTextFiles\\";
        
        List<string> file_paths {get; set;}

        public TxtKeyValuePair() {
            if (!Directory.Exists(text_file_location)) {
                Directory.CreateDirectory(text_file_location);
            }
            file_paths = Directory.GetFiles(text_file_location).ToList();
            file_paths.Sort();
        }

        public bool add(string key, string value)
        {
            H.stringToFile(value, text_file_location + key + ".txt");
            file_paths.Add(text_file_location + key + ".txt");

            return true;
        }

        public string getValue(string key)
        {
            try {
                return H.fileToString(text_file_location + key + ".txt");
            } catch {
                return "";
            }
        }

        public bool remove(string key) {
            try {
                File.Delete(text_file_location + key + ".txt");
                file_paths.Remove(text_file_location + key + ".txt");
                return true;
            } catch { }

            return false;
        }

        /************************* Tests *******************************
            TxtKeyValuePair kvp = new TxtKeyValuePair();
            kvp.add("1", "hello");
            kvp.add("2", "world");
            kvp.add("3", "world");
            kvp.remove("3");
            MessageBox.Show(kvp.getValue("1"));
        */
    }
}
