using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobMineDisplay
{
    public partial class Form1 : Form
    {
        /* Usual Heights
        Label: 13
        Textbox: 20
        ComboBox: 21
        */

        #region Controls
        string display_panel_name = "pDisplay";
        string dgv_display_name = "dgvDisplay";
        DataGridView dgv_display = null;

        TabControl tc_display = null;
        Button btn_hide = null;
        string tc_display_name = "tcDisplayDetails";
        string btn_hide_name = "btnHide";

        string cb_per_page_name = "cbPerPage";
        ComboBox cb_per_page = null;

        string tb_page_number_name = "tbNumberOfPages";
        TextBox tb_page_number = null;

        string btn_display_left_name = "btnDisplayLeft";
        Button btn_display_left = null;
        string tb_current_page_name = "tbCurrentPage";
        TextBox tb_current_page = null;
        string btn_display_right_name = "btnDisplayRight";
        Button btn_display_right = null;

        string btn_display_refresh_name = "btnDisplayRefresh";
        Button btn_display_refresh = null;
        #endregion

        List<string[]> data = new List<string[]>();
        int current_entry = 0;
        int current_page = 0;
        int page_num = 1;
        int results_per_page = 50;

        FieldDisplayer field_displayer = null;

        string[] dgv_columns = new string[14]{
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
            "rank"
        }; /**TO DO**/
        Dictionary<string, int> dgv_display_columns = new Dictionary<string, int>() {
            {"title", 1},
            {"employer", 2},
            {"location", 4}, 
            {"application_status", 5},
            {"available_openings", 6},
            {"last_day_to_apply", 7},
            {"app_num", 8}
        }; /**TO DO**/
        Dictionary<string, Dictionary<string, string>> detail_tabs = new Dictionary<string, Dictionary<string, string>>() {
            { "Main", new Dictionary<string, string>{
                    { "identifier", "tb" },
                    { "title", "tb" },
                    { "employer", "tb" },
                    { "unit_name", "tb" },
                    { "location", "tb" },
            
                    { "application_status", "tb" },
                    { "available_openings", "tb" },
                    { "last_day_to_apply", "tb" },
                    { "app_num", "tb" },
                    { "disciplines", "tb" },
            
                    { "levels", "tb" },
                    { "comments", "rtb - 5" },
                    { "timestamp",  "tb" },
                    { "rank",  "tb" }
                }
            }, 
            { "Description", new Dictionary<string, string>{
                    { "description", "**TO DO**" }
                }
            }, 
            { "Parsed", new Dictionary<string, string>{
                    { "parsed", "**TO DO**" }
                }
            }
        }; /**TO DO**/
        RichTextBox description = null;
        RichTextBox parsed_description = null;

        string current_db = "tblPastJobPosting";

        void getData() {
            try {
                data = jmd.selectQuery(cbSqlWhere.Text);
                current_db = cbSqlWhere.Text.IndexOf("tblPastJobPosting") > 0 ? "tblPastJobPosting" : "tblNewJobPosting";
            } catch { }

            /*
            data = new List<string[]>();
            for (int i = 0; i < 51; i++) {
                data.Add(new string[13]{
                    "274824 - " + i.ToString(), 
                    "Software Development - Machine Learning",
                    "A9.com (An Amazon Subsidiary)",
                    "yo",
                    "Palo Alto, CA, USA",
                    "N/A",
                    "1",
                    "29 SEP 2015",
                    "83",
                    "N/A",
                    "Intermediate, Senior",
                    "Please apply directly to: https://app.jobvite.com/j?cj=oxeM1fwi&s=University_Programs:_Waterloo AND to JobMine",
                    "23/12/2015 12:00:00 AM"
                });
            }
            */
        }



        public Panel loadDisplay(int x, int y, int width, int height, ContainerControl container) {
            Panel result = new Panel();
            result.BorderStyle = BorderStyle.FixedSingle;
            result.Name = display_panel_name;
            result.Location = new Point(x, y);
            result.Size = new Size(width, height);
            result.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);

            dgv_display = addDGV(dgv_display_name, 3, 3, width - 6, height - 46, result);
            dgv_display.CellClick += dgvDisplay_CellClicked;
            foreach (string column in dgv_display_columns.Keys) {
                dgv_display.Columns.Add(column, column);
            }

            tc_display = addTabControl(tc_display_name, detail_tabs.Keys.ToArray(),
                100, 30, width - 100, height - 70, result);
            tc_display.BringToFront();
            field_displayer = new FieldDisplayer("Main", detail_tabs["Main"]); /**TO DO**/
            List<Control> field_controls = field_displayer.getControls();
            foreach (Control c in field_controls) {
                tc_display.TabPages[0].Controls.Add(c);
            }
            int tab_width = tc_display.TabPages[1].Width;
            int tab_height = tc_display.TabPages[1].Height;
            
            description = new RichTextBox();
            description.Location = new Point(6, 6);
            description.Size = new Size(tab_width - 12, tab_height - 12);
            description.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            tc_display.TabPages[1].Controls.Add(description);

            parsed_description = new RichTextBox();
            parsed_description.Location = new Point(6, 6);
            parsed_description.Size = new Size(tab_width - 12, tab_height - 12);
            parsed_description.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            tc_display.TabPages[2].Controls.Add(parsed_description);

            btn_hide = addButton(btn_hide_name, "X", width - 20, 7, 20, 20, new EventHandler(btnHide_Click), result);
            btn_hide.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            btn_hide.BringToFront();
            hideDetails();

            cb_per_page = addComboBox(cb_per_page_name, new string[4] { "25", "50", "75", "100" }, results_per_page.ToString(), 70, height - 31, 70, result);
            cb_per_page.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            cb_per_page.SelectedIndexChanged += new EventHandler(cbPerPage_SelectionChanged);
            cb_per_page.DropDownStyle = ComboBoxStyle.DropDownList;

            addLabel("Per Page", 10, height - 27, result).Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            
            /*
            tb_page_number = addTextBox(tb_page_number_name, 300, height - 30, 40, 20, result);
            tb_page_number.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            tb_page_number.ReadOnly = true;
            tb_page_number.BorderStyle = BorderStyle.Fixed3D;
            addLabel("Number of Pages", 200, height - 27, result).Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            */

            btn_display_left = addButton(btn_display_left_name, "<", width - 123, height - 30, 35, 20, new EventHandler(btnDisplayLeft_Click), result);
            btn_display_left.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            tb_current_page = addTextBox(tb_current_page_name, width - 82, height - 30, 35, 20, result);
            tb_current_page.ReadOnly = true;
            tb_current_page.BorderStyle = BorderStyle.Fixed3D;
            tb_current_page.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            btn_display_right = addButton(btn_display_right_name, ">", width - 41, height - 30, 35, 20, new EventHandler(btnDisplayRight_Click), result);
            btn_display_right.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);

            btn_display_refresh = addButton(btn_display_refresh_name, "Refresh", 146, height - 31, 70, 21, btnDisplayRefresh_Click, result);
            btn_display_refresh.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);

            container.Controls.Add(result);

            showDetails(); //**********************

            return result;
        }

        #region And Controls
        public DataGridView addDGV(string name, int x, int y, int width, int height, ContainerControl container)
        {
            DataGridView dgv = new DataGridView();

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Location = new System.Drawing.Point(x, y);
            dgv.Name = name;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgv.Size = new System.Drawing.Size(width, height);
            dgv.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            // dgv.TabIndex = 48;
            container.Controls.Add(dgv);

            return dgv;
        }
        public DataGridView addDGV(string name, int x, int y, int width, int height, Panel container)
        {
            DataGridView dgv = new DataGridView();

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Location = new System.Drawing.Point(x, y);
            dgv.Name = name;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgv.Size = new System.Drawing.Size(width, height);
            dgv.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            // dgv.TabIndex = 48;
            container.Controls.Add(dgv);

            return dgv;
        }
        public TabControl addTabControl(string name, string[] tabs, int x, int y, int width, int height, Panel container)
        {
            TabControl result = new TabControl();
            result.Name = name;
            result.Location = new Point(x, y);
            result.Size = new Size(width, height);
            result.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            if (tabs != null)
            {
                for (int i = 0; i < tabs.Length; i++)
                {
                    result.TabPages.Add(tabs[i]);
                }
            }
            container.Controls.Add(result);

            return result;
        }
        public TextBox addTextBox(string name, int x, int y, int width, int height, Panel container)
        {
            TextBox result = new TextBox();
            result.Name = name;
            result.Location = new Point(x, y);
            result.Size = new Size(width, height);
            container.Controls.Add(result);
            return result;
        }
        public Label addLabel(string text, int x, int y, Panel container)
        {
            Label result = new Label();
            result.Text = text;
            result.Location = new Point(x, y);
            // result.Size = new Size(width, height);
            container.Controls.Add(result);
            return result;
        }
        public ComboBox addComboBox(string name, string[] items, string text, int x, int y, int width, Panel container)
        {
            ComboBox result = new ComboBox();
            result.Name = name;
            result.Text = text;
            result.Location = new Point(x, y);
            result.Size = new Size(width, result.Size.Height);

            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    result.Items.Add(items[i]);
                }
            }

            container.Controls.Add(result);
            return result;
        }
        public Button addButton(string name, string text, int x, int y, int width, int height, EventHandler eh, Panel container)
        {
            Button result = new Button();
            result.Name = name;
            result.Text = text;
            result.Location = new Point(x, y);
            result.Size = new Size(width, result.Size.Height);

            result.Click += eh;

            container.Controls.Add(result);
            return result;
        }
        #endregion

        public void showDetails()
        {
            tc_display.Show();
            btn_hide.Show();
            if (current_entry < data.Count) {
                field_displayer.enterInput(data[current_entry]);
                description.Text = tkvp.getValue("00" + data[current_entry][0]);
                parsed_description.Text = parser.parseDescription(description.Text);
            }
            Application.DoEvents();
        }
        public void hideDetails()
        {
            tc_display.Hide();
            btn_hide.Hide();
        }
        private void btnHide_Click(object sender, EventArgs e)
        {
            hideDetails();
        }

        private void refreshDGV() {
            dgv_display.Rows.Clear();
            int start = current_page * results_per_page;
            int end = Math.Min((current_page + 1) * results_per_page, data.Count);
            string[] row = null;
            int counter = 0;

            for (int i = start; i < end; i++) {
                row = new string[dgv_display_columns.Count];
                counter = 0;
                foreach (string key in dgv_display_columns.Keys) {
                    row[counter] = data[i][dgv_display_columns[key]];
                    counter++;
                }

                dgv_display.Rows.Add(row);
                dgv_display.Rows[i - start].DefaultCellStyle.BackColor = rankToColor(data[i][13]);
            }
        }

        private void btnDisplayRefresh_Click(object sender, EventArgs e) {
            getData();
            changeCurrentPage(0);
            page_num = (data.Count + results_per_page - 1) / results_per_page;
            refreshDGV();
        }

        private void cbPerPage_SelectionChanged(object sender, EventArgs e) {
            try {
                results_per_page = Convert.ToInt32(cb_per_page.SelectedItem.ToString());
                page_num = (data.Count + results_per_page - 1) / results_per_page;
                changeCurrentPage(0);
                refreshDGV();
                MessageBox.Show("You selected " + results_per_page.ToString());
            } catch { }
        }


        private void btnDisplayLeft_Click(object sender, EventArgs e) {
            if (current_page > 0) {
                changeCurrentPage(current_page - 1);
                refreshDGV();
            }
        }
        private void btnDisplayRight_Click(object sender, EventArgs e) {
            if (current_page < page_num - 1) {
                changeCurrentPage(current_page + 1);
                refreshDGV();
            }
        }

        private void dgvDisplay_CellClicked(object sender, EventArgs e) {
            var rows = dgv_display.SelectedRows;
            if (dgv_display.SelectedRows.Count > 0) {
                int dgv_index = dgv_display.SelectedRows[0].Index;
                current_entry = current_page * results_per_page + dgv_index;
                showDetails();
            }
        }

        public void changeCurrentPage(int new_page) {
            current_page = new_page;
            tb_current_page.Text = (current_page + 1).ToString();
        }


    }
}
