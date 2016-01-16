/* DOCUMENTATION *************************************
string prefix = "Main";
Dictionary<string, string> map 
    = new Dictionary<string, string>{
        { "identifier", "tb" },
        { "title", "rtb - 5" }, // RichTextBox with 5 lines
        { "employer", "cb - 1 - 1, 2, 3, 4" }
                                // combo box with 1 as text and 1 .. 4 as items
    };

field_displayer = new FieldDisplayer(prefix, map);

// get created controls
List<Control> field_controls = field_displayer.getControls();

// extract all inputs from controls
Dictionary<string, string> input = extractInput();
******************************************************/

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
    public class FieldDisplayer {
        // Display all fields for details
        
        /*
        Label with textbox
         * label - left 6, top 9, (height 13)
         * textbox - left 120, top 6, height 20
        
        Label with ComboBox
         * 
        
        RichTextBox height formula:
         * height = lines * 13 + 5
        
        */

        string field_prefix;
        Dictionary<string, string> field_structure = new Dictionary<string, string>();
        List<Control> fields = new List<Control>();
        List<Control> other_controls = new List<Control>();

        public FieldDisplayer(string field_prefix_1, Dictionary<string, string> field_structure_1) {
            field_prefix = field_prefix_1;
            field_structure = field_structure_1;
            initControlLists();
        }

        // init control lists using the field_structure dictionary
        private void initControlLists() {
            int width = 300;
            int control_height = 20;
            int field_height = 26;
            int label_left = 6;
            int field_left = 120;
            int buffer = 5;

            int height_so_far = 0;

            foreach (string key in field_structure.Keys) {
                Control new_control = null;
                string[] field_split = split(field_structure[key], " - ");
                Label new_label = newLabel(key, label_left, height_so_far + 3 + buffer);

                other_controls.Add(new_label);

                switch (field_split[0]) {
                    case "tb":
                        new_control = newTextBox("tb" + field_prefix + key, field_left, height_so_far + buffer, width, control_height);
                        height_so_far += field_height + buffer;
                        break;
                    case "cb":
                        new_control = newComboBox("cb" + field_prefix + key, split(field_split[2], ", "), field_split[1], field_left, height_so_far + buffer, width);
                        height_so_far += field_height + buffer;
                        break;
                    default:
                        int lines = Convert.ToInt32(field_split[1]);
                        int rtb_height = 5 + lines * 13;
                        new_control = newRichTextBox("rtb" + field_prefix + key, field_left, height_so_far + buffer, width, rtb_height);
                        height_so_far += rtb_height + buffer;
                        break;
                }
                fields.Add(new_control);
            }
        }

        public void enterInput(string[] input) {
            if (input != null) {
                int len = Math.Min(input.Length, fields.Count);
                for (int i = 0; i < len; i++) {
                    fields[i].Text = input[i];
                }
            }
        }
        
        public List<Control> getControls() {
            List<Control> result = new List<Control>();
            foreach (Control c in fields) {
                result.Add(c);
            }
            foreach (Control c in other_controls) {
                result.Add(c);
            }
            return result;
        }
        // extract all input from all field controls
        public Dictionary<string, string> extractInput() {
            if (fields.Count == 0) {
                throw new Exception("No fields");
            }
            Dictionary<string, string> result = new Dictionary<string, string>();
            int counter = 0;
            foreach (string key in field_structure.Keys) {
                result[key] = fields[counter].Text;
                counter++;
            }
            return result;
        }

        private RichTextBox newRichTextBox(string name, int x, int y, int width, int height) {
            RichTextBox result = new RichTextBox();
            result.Name = name;
            result.Location = new Point(x, y);
            result.Size = new Size(width, height);
            return result;
        }
        private TextBox newTextBox(string name, int x, int y, int width, int height) {
            TextBox result = new TextBox();
            result.Name = name;
            result.Location = new Point(x, y);
            result.Size = new Size(width, height);
            return result;
        }
        private Label newLabel(string text, int x, int y) {
            Label result = new Label();
            result.Text = text;
            result.Location = new Point(x, y);
            // result.Size = new Size(width, height);
            return result;
        }
        private ComboBox newComboBox(string name, string[] items, string text, int x, int y, int width) {
            ComboBox result = new ComboBox();
            result.Name = name;
            result.Text = text;
            result.Location = new Point(x, y);
            result.Size = new Size(width, result.Size.Height);

            if (items != null) {
                for (int i = 0; i < items.Length; i++) {
                    result.Items.Add(items[i]);
                }
            }

            return result;
        }
        private string[] split(string str, string separator) {
            return str.Split(new string[1] { separator }, StringSplitOptions.None);
        }
    }
}
