using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace JobMineDisplay
{
    public class Database
    {
        public static string database_connection_str = "DataSource = " + AppDomain.CurrentDomain.BaseDirectory + "JobMineDisplay.sdf";
        public static string backup = AppDomain.CurrentDomain.BaseDirectory + "Xml Backup (Do Not Edit)\\";
        public static Dictionary<string, string[]> table_structure = new Dictionary<string, string[]>()
        {
            {"tblPastJobPosting", new string[14] { 
                "identifier",
                "title",
                "employer",
                "unit_name",
                "location",

                "application_status",
                "available_openings",
                "last_day_to_apply",
                "app_num",
                "disciplines",

                "levels",
                "comments",
                "timestamp",
                "rank" }},
            {"tblNewJobPosting", new string[14] { 
                "identifier",
                "title",
                "employer",
                "unit_name",
                "location",

                "application_status",
                "available_openings",
                "last_day_to_apply",
                "app_num",
                "disciplines",

                "levels",
                "comments",
                "timestamp",
                "rank" }}
        };

        #region Command getters
        public string equalsJoin(string[] arr1, string[] arr2)
        {
            string result = "";

            if (arr1 != null && arr2 != null && arr1.Length > 0 && arr2.Length > 0 && arr1.Length == arr2.Length)
            {
                result += arr1[0] + "='" + arr2[0] + "'";
                for (int i = 1; i < arr1.Length; i++)
                {
                    result += ", " + arr1[i] + "='" + arr2[i] + "'";
                }
            }

            return result;
        }
        public string getWhere(string[] where_keys, string[] where_values)
        {
            string result = " WHERE " + equalsJoin(where_keys, where_values);
            return result.Length == 7 ? "" : result;
        }
        public string getSelectCommand(string table_name, string columns, string[] where_keys, string[] where_values)
        {
            return "SELECT " + columns + " FROM " + table_name + getWhere(where_keys, where_values);
        }
        public string getInsertCommand(string table_name, string[] entry)
        {
            return "INSERT INTO " + table_name + " VALUES('" + String.Join("', '", entry) + "')"; ;
        }
        public string getUpdateCommand(string table_name, string[] columns, string[] column_values, string[] where_keys, string[] where_values)
        {
            string result = "UPDATE " + table_name + " SET ";

            if (columns.Length == 0 || column_values.Length == 0)
            {
                throw new Exception("empty entry or header");
            }
            else if (column_values.Length != columns.Length)
            {
                throw new Exception("invalid entry-header combination");
            }
            result += equalsJoin(columns, column_values);
            result += getWhere(where_keys, where_values);

            return result;
        }
        public string getDeleteCommand(string table_name, string[] where_keys, string[] where_values)
        {
            return "DELETE FROM " + table_name + getWhere(where_keys, where_values);
        }

        #endregion

        // selectOne
        public string[] selectOne(string table_name, string columns, int column_num, string[] where_keys, string[] where_values)
        {
            string[] result = new string[column_num];

            using (SqlCeConnection con = new SqlCeConnection(database_connection_str))
            {
                con.Open();

                using (SqlCeCommand com = new SqlCeCommand(getSelectCommand(table_name, columns, where_keys, where_values), con))
                {
                    SqlCeDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        result = new string[column_num];

                        try
                        {
                            for (int i = 0; i < column_num; i++)
                            {
                                result[i] = reader.GetString(i);
                            }
                            return result;
                        }
                        catch
                        {

                        }
                    }
                }
            }

            return result;
        }

        // select
        public List<string[]> select(string table_name, string columns, int column_num, string[] where_keys, string[] where_values)
        {
            List<string[]> result = new List<string[]>();
            string[] new_entry = null;

            using (SqlCeConnection con = new SqlCeConnection(database_connection_str))
            {
                con.Open();

                using (SqlCeCommand com = new SqlCeCommand(getSelectCommand(table_name, columns, where_keys, where_values), con))
                {
                    SqlCeDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        new_entry = new string[column_num];

                        try
                        {
                            for (int i = 0; i < column_num; i++)
                            {
                                new_entry[i] = reader.GetString(i);
                            }
                        }
                        catch
                        {

                        }

                        result.Add(new_entry);
                    }
                }
            }

            return result;
        }

        // insert
        public bool insert(string table_name, string[] entry)
        {
            int result = 0;

            using (SqlCeConnection con = new SqlCeConnection(database_connection_str))
            {
                con.Open();
                using (SqlCeCommand com = new SqlCeCommand(getInsertCommand(table_name, entry), con))
                {
                    result = com.ExecuteNonQuery();
                }
            }

            return result != 0;
        }

        // update
        public bool update(string table_name, string[] columns, string[] column_values, string[] where_keys, string[] where_values)
        {
            int result = 0;

            using (SqlCeConnection con = new SqlCeConnection(database_connection_str))
            {
                con.Open();
                using (SqlCeCommand com = new SqlCeCommand(getUpdateCommand(table_name, columns, column_values, where_keys, where_values), con))
                {
                    result = com.ExecuteNonQuery();
                }
            }

            return result != 0;
        }

        // upsert
        public bool upsert(string table_name, string[] columns, string[] column_values, string[] where_keys, string[] where_values)
        {
            int result = 0;
            bool exists = false;

            using (SqlCeConnection con = new SqlCeConnection(database_connection_str))
            {
                con.Open();
                using (SqlCeCommand com = new SqlCeCommand(getSelectCommand(table_name, String.Join(", ", columns), where_keys, where_values), con))
                {
                    SqlCeDataReader reader = com.ExecuteReader();
                    exists = reader.Read();
                }

                if (exists)
                {
                    using (SqlCeCommand com = new SqlCeCommand(getUpdateCommand(table_name, columns, column_values, where_keys, where_values), con))
                    {
                        result = com.ExecuteNonQuery();
                    }
                }
                else if (column_values.Length == table_structure[table_name].Length)
                {
                    using (SqlCeCommand com = new SqlCeCommand(getInsertCommand(table_name, column_values), con))
                    {
                        result = com.ExecuteNonQuery();
                    }
                }
            }

            return result == 0;
        }

        // delete
        public int delete(string table_name, string[] where_keys, string[] where_values)
        {
            int result = 0;

            using (SqlCeConnection con = new SqlCeConnection(database_connection_str))
            {
                con.Open();

                using (SqlCeCommand com = new SqlCeCommand(getDeleteCommand(table_name, where_keys, where_values), con))
                {
                    result = com.ExecuteNonQuery();
                }
            }

            return result;
        }



        // EXTRA *******************************************************

        // table to dgv
        // write entries from table to datagridview from start to end, inclusively, where start = 0, 1, ...
        public void tableToDGV2(string table_name, string columns, DataGridView dgv, int start, int end)
        {
            if (table_structure[table_name] == null) { throw new Exception(table_name + " is missing in table_structure"); }

            string[] new_entry = null;
            int counter = 0;
            dgv.Rows.Clear();
            string[] dgv_columns =  columns == "*"
                ? (string[])table_structure[table_name].Clone()
                : columns.Split(new string[1] {", "}, StringSplitOptions.None);
            
            if (dgv_columns.Length != dgv.Columns.Count) {
                dgv.Columns.Clear();
                for (int i = 0; i < dgv_columns.Length; i++) {
                    dgv.Columns.Add(dgv_columns[i], dgv_columns[i]);
                }
            }

            using (SqlCeConnection con = new SqlCeConnection(database_connection_str))
            {
                con.Open();
                using (SqlCeCommand com = new SqlCeCommand(getSelectCommand(table_name, columns, null, null), con))
                {
                    SqlCeDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        new_entry = new string[dgv_columns.Length];
                        if (start <= counter && counter <= end)
                        {
                            try
                            {
                                for (int i = 0; i < dgv_columns.Length; i++)
                                {
                                    new_entry[i] = reader.GetString(i);
                                }
                                dgv.Rows.Add(new_entry);
                            }
                            catch
                            {

                            }
                        }
                        counter++;
                    }
                }
            }
        }

        // -------------------
        // Code generating tables
        //--------------------

        // -------------------
        // XML
        // -------------------

        // EFFICIENCY *****************************************************
        // * open and close connection
        /*
        SqlCeConnection con = null;

        public void openConnection() {
            if (con != null) { return; }

            con = new SqlCeConnection(database_connection_str);
            con.Open();
        }

        public void closeConnection() {
            if (con == null) { return; }

            try {
                con.Dispose();
            } 
            finally {
                con = null;
            }
        }
        bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    closeConnection();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        */
        // * stack SQL statements then run all

        /* TESTS ******************************************/
        /*
        public Database() {
            db.delete("tblSnippets", null, null);

            List<string[]> nothing = db.select("tblSnippets", "*", 2,
                new string[1] { "name" },
                new string[1] { "blah" });


            bool insert = db.insert("tblSnippets", new string[2] { "yo", "maayyn" });
            string[] yo = db.selectOne("tblSnippets", "*", 2,
                new string[1] { "name" },
                new string[1] { "yo" });
            List<string[]> yo2 = db.select("tblSnippets", "*", 2,
                new string[1] { "name" },
                new string[1] { "yo" });
            string[] man = db.selectOne("tblSnippets", "*", 2,
                new string[1] { "name" },
                new string[1] { "maayyn" });
            List<string[]> man2 = db.select("tblSnippets", "*", 2,
                new string[1] { "name" },
                new string[1] { "maayyn" });

            bool update = db.update("tblSnippets",
                Database.table_structure["tblSnippets"],
                new string[2] { "yo", "dawg" },
                new string[1] { "name" },
                new string[1] { "yo" });
            List<string[]> yo3 = db.select("tblSnippets", "*", 2,
                new string[1] { "name" },
                new string[1] { "yo" });

            bool upsert = db.upsert("tblSnippets",
                Database.table_structure["tblSnippets"],
                new string[2] { "hi", "hello" },
                new string[1] { "name" },
                new string[1] { "hi" });
            // List<string[]> hi = db.select("tblSnippets", "*", 2, new string[1] { "name" }, new string[1] { "hi" });
            bool upsert2 = db.upsert("tblSnippets",
                Database.table_structure["tblSnippets"],
                new string[2] { "yo", "hello" },
                new string[1] { "name" },
                new string[1] { "yo" });
            List<string[]> yo5 = db.select("tblSnippets", "*", 2, null, null);
        }
        */
    }
}
