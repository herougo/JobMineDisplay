using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobMineDisplay {
    class H {
        // get file names in folder_path then find next valid file name
        // For example, files are in the format hello - 1.txt, hello - 2.txt, hello - 3.txt, ...
        // let's say that file with the highest number is 9 (note that hello - 8.txt might be missing)
        // the output would be 10
        public static int getLastFileNameNumber(string prefix, string suffix, string folder_path) {
            string[] files = Directory.GetFiles(folder_path);
            int max_num = 0;
            int prefix_pointer = 0;
            int suffix_pointer = 0;
            int suffix_index = 0;
            int current_index = 0;

            foreach (string file in files) {
                if (file.Length <= prefix.Length + suffix.Length) {
                    continue;
                }

                // check prefix
                for (prefix_pointer = 0; prefix_pointer < prefix.Length; prefix_pointer++) {
                    if (file[prefix_pointer] != prefix[prefix_pointer]) {
                        continue;
                    }
                }

                // check suffix
                suffix_index = file.Length - suffix.Length;
                for (suffix_pointer = file.Length - 1; suffix_pointer >= suffix_index; suffix_pointer--) {
                    if (file[suffix_pointer] != suffix[suffix_pointer - suffix_index]) {
                        continue;
                    }
                }

                try {
                    current_index = Convert.ToInt32(file.Substring(prefix_pointer, suffix_index - prefix_pointer));
                    if (current_index > max_num) {
                        max_num = current_index;
                    }
                } catch { }
            }

            return max_num;

            // Tests:
            // MessageBox.Show(getNextFileNameNumber("hello - ", ".txt", AppDomain.CurrentDomain.BaseDirectory).ToString());
            // files = new string[4] { "hello - 1.txt", "hello - 5txt", "hello - 3.txt", "hello -5.txt" };
        }

        public static string fileToString(string file_path) {
            string result = "";
            
            FileStream fs = new FileStream(file_path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            result = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            
            return result;
        }

        public static void stringToFile(string text, string file_path) {
            FileStream fs = new FileStream(file_path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(text);
            sw.Close();
            fs.Close();
        }

        public static string getFileName(string path) {
            if (path == null || path.Length == 0) { return path; }
            
            int start_index = path.LastIndexOf('\\');
            int end_index = path.LastIndexOf('.');

            if (start_index == -1 && end_index == -1) {
                return path;
            } else if (start_index == -1) {
                return path.Substring(0, end_index);
            } else if (end_index == -1) {
                // Example yeo\\hello -> length 9
                //         0123 456789 ... -> Substring(4, 5)
                return path.Substring(start_index + 1, path.Length - (start_index + 1));
            } else {
                // Example yeo\\hello.txt -> length 13
                //         0123 456789 ... -> Substring(4, 5)
                return path.Substring(start_index + 1, end_index - (start_index + 1));
            }
        }

        public static List<string> getFilesInFolders(string root_path) {
            List<string> result = new List<string>();

            if (Directory.Exists(root_path)) {
                string[] folders = Directory.GetDirectories(root_path);

                foreach (string folder in folders) {
                    string[] files = Directory.GetFiles(folder);
                    foreach (string file in files) {
                        result.Add(file);
                    }
                }    
            }

            return result;
        }
    }
}
