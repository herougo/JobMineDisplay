using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobMineDisplay
{
    public partial class Form1 : Form
    {
        JobMineData jmd = new JobMineData();
        Database db = new Database();
        TxtKeyValuePair tkvp = new TxtKeyValuePair();
        Panel panel = null;
        DescriptionParser parser = new DescriptionParser();

        public Form1()
        {
            InitializeComponent();
            // DataGridView dgv = addDGV("dgvYo", 12, 12, this.Size.Width - 30, this.Size.Height - 30, this);
            // dgv.SelectionChanged += new EventHandler(DataGridView1_SelectionChanged);
            // dgv.Columns.Add("Yo", "Yo");
            // dgv.Rows.Add(new string[1] { "ay" });
            // dgv.Rows.Add(new string[1] { "you" });

            // string[][] yo = new string[2][];
            // MessageBox.Show(yo.Length.ToString());
            

            // panel1.Hide();
            panel = loadDisplay(6, 40, 972, 500, this);
            altDisplay();
        }

        private void btnXmlToPastDatabase_Click(object sender, EventArgs e) {
            MessageBox.Show(jmd.xmlToPastDatabase());
        }

        private void btnXmlToNewDatabase_Click(object sender, EventArgs e) {
            MessageBox.Show(jmd.xmlToNewDatabase());
        }

        private void btnAltDisplay_Click(object sender, EventArgs e) {
            altDisplay();
        }
        public void altDisplay() {
            if (panel != null && panel.Width <= 720) {
                MessageBox.Show("Window width is too small");
            } else if (dgv_display != null) {
                dgv_display.Width = 710;
                dgv_display.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
                dgv_display.CellClick -= dgvDisplay_CellClicked;
                dgv_display.SelectionChanged += dgvDisplay_CellClicked;
                
                dgv_display.Columns[0].HeaderText = "title";
                dgv_display.Columns[0].Width = 200;
                dgv_display.Columns[1].HeaderText = "employer";
                dgv_display.Columns[1].Width = 150;
                dgv_display.Columns[3].HeaderText = "status";
                dgv_display.Columns[3].Width = 60;
                dgv_display.Columns[4].HeaderText = "openings";
                dgv_display.Columns[4].Width = 50;
                dgv_display.Columns[5].HeaderText = "apply_by";
                dgv_display.Columns[5].Width = 80;
                dgv_display.Columns[6].HeaderText = "app_num";
                dgv_display.Columns[6].Width = 50;                

                tc_display.Location = new Point(715, 0);
                tc_display.Size = new Size(panel.Width - 715, panel.Height - 40);
                tc_display.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);

                btn_hide.Location = new Point(panel.Width - 20, 0);

                parsed_description.Font = new Font("Arial", 16);
            }
        }


        /*
        private void btnRefresh_Click(object sender, EventArgs e) {
            // db.tableToDGV2("tblPastJobPosting", "identifier, title, location, application_status, available_openings, last_day_to_apply", dgv_display, 0, 50);
            Application.DoEvents();
        }
        */

        
    }
}
