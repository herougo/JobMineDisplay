using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobMineDisplay {
    public partial class Form1 : Form {
        private void btnRank4_Click(object sender, EventArgs e) {
            rankJob(4);
        }
        private void btnRank3_Click(object sender, EventArgs e) {
            rankJob(3);
        }
        private void btnRank2_Click(object sender, EventArgs e) {
            rankJob(2);
        }
        private void btnRank1_Click(object sender, EventArgs e) {
            rankJob(1);
        }
        public void rankJob(int n) {
            try {
                string identifier = data[current_entry][0];
                if (db.update(current_db, new string[1] { "rank" },
                    new string[1] { n.ToString() },
                    new string[1] { "identifier" },
                    new string[1] { identifier })) {
                    dgv_display.Rows[current_entry - current_page * results_per_page].DefaultCellStyle.BackColor = rankToColor(n.ToString());
                }

            } catch { MessageBox.Show("An error occurred"); }
        }
        public Color rankToColor(string str) {
            try {
                int n = Convert.ToInt32(str);
                if (n == 1) {
                    return Color.LightGreen;
                } else if (n == 2) {
                    return Color.LightBlue;
                } else if (n == 3) {
                    return Color.Yellow;
                } else if (n == 4) {
                    return Color.Red; ;
                } else {
                    return Color.White;
                }
            } catch { }

            return Color.White;
        }
    }
}
