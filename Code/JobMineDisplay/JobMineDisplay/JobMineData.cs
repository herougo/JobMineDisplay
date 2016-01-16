using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JobMineDisplay {
    public class JobMineData {
        static string xml_folder = AppDomain.CurrentDomain.BaseDirectory + "Xml\\";
        static string xml_past_folder = AppDomain.CurrentDomain.BaseDirectory + "Xml\\Past\\";

        Database db = new Database();
        TxtKeyValuePair descriptions = new TxtKeyValuePair();
        string default_timestamp = "'23/12/2015 12:00:00 AM'";

        /* Dictionary<string, int> tags = new Dictionary<string, int>() {
            { "identifier", 0 },
            { "title", 1 },
            { "employer", 2 },
            { "unit_name", 3 },
            { "location", 4 },

            { "application_status", 5 },
            { "available_openings", 6 },
            { "last_day_to_apply", 7 },
            { "app_num", 8 },
            { "disciplines", 9 },

            { "levels", 10 },
            { "comments", 11 },
            { "timestamp", 12 }
        }; */
        private string updateHelper(ref List<string> columns, ref List<string> values) {
            int len = Math.Min(columns.Count, values.Count);
            string result = "";

            if (len <= 0) { result += " " + columns.ElementAt(0) + "='" + values.ElementAt(0) + "'"; }
            for (int i = 1; i < len; i++) {
                result += ", " + columns.ElementAt(i) + "='" + values.ElementAt(i) + "'";
            }

            return result;
        }
        public string xmlToPastDatabase() {
            int error_count = 0;
            // Clear database
            // Open the connection using the connection string.
            /*
            using (SqlCeConnection con = new SqlCeConnection(Database.database_connection_str))
            {
                con.Open();

                // Insert into the SqlCe table. ExecuteNonQuery is best for inserts.
                using (SqlCeCommand com = new SqlCeCommand("DELETE FROM tblPastJobPosting", con))
                {
                    try {
                        com.ExecuteNonQuery();
                    } catch { error_count++; }
                }
            }
            */
            /* Clear files in txt files
            try {
                string[] txt_files = Directory.GetFiles(TxtKeyValuePair.text_file_location);

                foreach (string txt_file in txt_files) {
                    File.Delete(txt_file);
                }
            } catch {}
            */

            List<string> xml_files = H.getFilesInFolders(xml_past_folder);
            Dictionary<string, bool> identifier_used = new Dictionary<string, bool>();
            List<string> sql_queries = new List<string>();

            foreach (string xml_filename in xml_files) {
                XmlDocument doc = new XmlDocument();
                FileStream lfile = new FileStream(xml_filename, FileMode.Open);
                doc.Load(lfile);

                foreach (XmlNode node in doc.DocumentElement) {
                    string sql_query = null;
                    List<string> columns = new List<string>();
                    List<string> values = new List<string>();

                    string identifier = null;
                    string description = null;
                    XmlNodeList tag_list = node.ChildNodes;
                    foreach (XmlNode tag in tag_list) {
                        switch (tag.Name) {
                            case "description":
                                description = tag.InnerText;
                                break;
                            case "identifier":
                                identifier = tag.InnerText;
                                break;
                            default:
                                columns.Add(tag.Name);
                                values.Add("'" + tag.InnerText.Replace("'", "\"") + "'");
                                break;
                        }
                    }
                    if (identifier == null) {
                        continue;
                    } else if (description != null) {
                        descriptions.add(identifier, description);
                    }

                    // Edit availableOpenings
                    bool has_timestamp = false;
                    for (int i = 0; i < columns.Count; i++) {
                        if (columns[i] == "availableOpenings") {
                            columns[i] = "available_openings";
                        } else if (columns[i] == "timestamp") {
                            has_timestamp = true;
                        }
                    }
                    // add timestamp
                    if (!has_timestamp) {
                        columns.Add("timestamp");
                        values.Add(default_timestamp);
                    }
                    columns.Add("rank");
                    values.Add("0");

                    bool used_before = false;
                    identifier_used.TryGetValue(identifier, out used_before);
                    if (used_before) {
                        sql_query = "UPDATE tblPastJobPosting SET" + updateHelper(ref columns, ref values) + " WHERE identifier = '" + identifier + "'";
                    } else {
                        columns.Add("rank");
                        values.Add("0");
                        columns.Add("identifier");
                        values.Add(identifier);
                        sql_query = "INSERT INTO tblPastJobPosting (" + String.Join(", ", columns) + ") VALUES(" + String.Join(", ", values) + ")";
                    }
                    identifier_used[identifier] = true;

                    sql_queries.Add(sql_query);
                }

                lfile.Close();
            }
            List<string> test = new List<string>();
            using (SqlCeConnection con = new SqlCeConnection(Database.database_connection_str))
            {
                con.Open();

                foreach (string query in sql_queries)
                {
                    using (SqlCeCommand com = new SqlCeCommand(query, con))
                    {
                        try {
                            com.ExecuteNonQuery();
                        } catch { error_count++; test.Add(query); }
                    }
                }
            }

            return sql_queries.Count.ToString() + " queries with " + error_count.ToString() + " errors";
        }
        public string xmlToNewDatabase() {
            int error_count = 0;
            // Clear database
            // Open the connection using the connection string.

            /*
            using (SqlCeConnection con = new SqlCeConnection(Database.database_connection_str)) {
                con.Open();

                // Insert into the SqlCe table. ExecuteNonQuery is best for inserts.
                using (SqlCeCommand com = new SqlCeCommand("DELETE FROM tblNewJobPosting", con)) {
                    try {
                        com.ExecuteNonQuery();
                    } catch { error_count++; }
                }
            }
            */

            List<string> xml_files = H.getFilesInFolders(xml_folder);
            Dictionary<string, bool> identifier_used = new Dictionary<string, bool>();
            List<string> sql_queries = new List<string>();

            foreach (string xml_filename in xml_files) {
                XmlDocument doc = new XmlDocument();
                FileStream lfile = new FileStream(xml_filename, FileMode.Open);
                doc.Load(lfile);

                foreach (XmlNode node in doc.DocumentElement) {
                    string sql_query = null;
                    List<string> columns = new List<string>();
                    List<string> values = new List<string>();

                    string identifier = null;
                    string description = null;
                    XmlNodeList tag_list = node.ChildNodes;
                    foreach (XmlNode tag in tag_list) {
                        switch (tag.Name) {
                            case "description":
                                description = tag.InnerText;
                                break;
                            case "identifier":
                                identifier = tag.InnerText;
                                break;
                            default:
                                columns.Add(tag.Name);
                                values.Add("'" + tag.InnerText.Replace("'", "\"") + "'");
                                break;
                        }
                    }
                    if (identifier == null) {
                        continue;
                    } else if (description != null) {
                        descriptions.add(identifier, description);
                    }

                    // Edit availableOpenings
                    bool has_timestamp = false;
                    for (int i = 0; i < columns.Count; i++) {
                        if (columns[i] == "availableOpenings") {
                            columns[i] = "available_openings";
                        } else if (columns[i] == "timestamp") {
                            has_timestamp = true;
                        }
                    }
                    // add timestamp
                    if (!has_timestamp) {
                        columns.Add("timestamp");
                        values.Add(default_timestamp);
                    }

                    bool used_before = false;
                    identifier_used.TryGetValue(identifier, out used_before);
                    if (used_before) {
                        sql_query = "UPDATE tblNewJobPosting SET" + updateHelper(ref columns, ref values) + " WHERE identifier = '" + identifier + "'";
                    } else {
                        columns.Add("rank");
                        values.Add("0");
                        columns.Add("identifier");
                        values.Add(identifier);
                        sql_query = "INSERT INTO tblNewJobPosting (" + String.Join(", ", columns) + ") VALUES(" + String.Join(", ", values) + ")";
                    }
                    identifier_used[identifier] = true;

                    sql_queries.Add(sql_query);
                }

                lfile.Close();
            }
            List<string> test = new List<string>();
            using (SqlCeConnection con = new SqlCeConnection(Database.database_connection_str)) {
                con.Open();

                foreach (string query in sql_queries) {
                    using (SqlCeCommand com = new SqlCeCommand(query, con)) {
                        try {
                            com.ExecuteNonQuery();
                        } catch { error_count++; test.Add(query); }
                    }
                }
            }

            return sql_queries.Count.ToString() + " queries with " + error_count.ToString() + " errors";
        }

        public List<string[]> selectQuery(string sql_query) {
            List<string[]> result = new List<string[]>();
            string[] new_entry = null;

            using (SqlCeConnection con = new SqlCeConnection(Database.database_connection_str)) {
                con.Open();

                using (SqlCeCommand com = new SqlCeCommand(sql_query, con)) {
                    SqlCeDataReader reader = com.ExecuteReader();
                    while (reader.Read()) {
                        new_entry = new string[14];

                        try {
                            for (int i = 0; i < 14; i++) {
                                new_entry[i] = reader.GetString(i);
                            }
                        } catch {

                        }

                        result.Add(new_entry);
                    }
                }
            }

            return result;
        }

        public void pastToNew() {

        }


    }
}
