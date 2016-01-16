using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobMineDisplay {
    public class DescriptionParser {
        string stars = "*******************";

        public string parseDescription(string description) {
            string result = "";

            List<string> past_titles = new List<string>();

            string[] description_array = description.Trim().Replace("\r", String.Empty).Replace("\n\n", "\n").Split('\n');
            Dictionary<string, string> basic_info = new Dictionary<string, string>();
            basic_info["Summary"] = "";
            basic_info["Responsibilities"] = "";
            basic_info["Required Skills"] = "";
            basic_info["Assets"] = "";
            basic_info["Bonus"] = "";

            string current_title = "Intro";
            int counter = 0;
            string interpretation = "";
            string temp = "";

            if (description_array.Length > 0 && isTitle(description_array[0])) {
                current_title = description_array[0].TrimEnd(':');
            }

            for (int i = 1; i < description_array.Length; i++) {
                interpretation = interpretTitle(current_title);

                counter = 0;
                while (i + counter < description_array.Length && !isTitle(description_array[i + counter])) {
                    counter++;
                }

                temp = "";
                for (int j = i; j < i + counter; j++) {
                    if (description_array[j].Length > 0) { temp += "\n" + description_array[j]; }
                }
                if (interpretation == "") {
                    result += "\n\n" + current_title + stars + temp;
                    past_titles.Add(current_title);
                } else {
                    basic_info[interpretation] += "\n\n" + current_title + temp;
                }

                i += counter;
                if (i < description_array.Length) {
                    current_title = description_array[i].TrimEnd(':');
                }
            }

            result =
                ("Required Skills" + new String('*', 100) + basic_info["Required Skills"]
                + "\n\nAssets" + new String('*', 100) + basic_info["Assets"]
                + "\n\nResponsibilities" + new String('*', 100) + basic_info["Responsibilities"]
                + "\n\nSummary" + new String('*', 100) + basic_info["Summary"]
                + "\n\nBonus" + new String('*', 100) + basic_info["Bonus"])
                + "\n\n" + new String('*', 100)
                + result;

            /* Uncomment when debugging */
            result += "\n";
            foreach (string title in past_titles) {
                result += "\n            { \"" + title.ToLower() + "\", \"\" },";
            }

            return result;
        }

        bool isTitle(string line) {
            string temp = line.Trim().TrimEnd(':').ToLower();
            return (temp.Length > 0 && temp.Length < 40 && line[0] != '*' && line[0] != '-' && line[0] != 'o')
                || interpretTitle(temp) != "";
        }

        string interpretTitle(string title) {
            string result = null;
            known_titles.TryGetValue(title.ToLower(), out result);
            if (result == null) { result = ""; }
            return result;
        }

        // provides mapping between titles and their associated heading
        // ie company profile goes to summary
        #region Known Titles Dictionary
        Dictionary<string, string> known_titles = new Dictionary<string, string>()
        {
            { "summary", "Summary" },
            { "responsibilities",  "Responsibilities" },
            { "required skills",  "Required Skills" },
            { "assets",  "Assets" },
            { "bonus",  "Assets" },

            { "intro", "Summary" },

            { "company profile", "Summary" },
            { "training", "Summary" },
            { "health and wellness", "Summary" },
            { "social activities", "Bonus" },
            { "employee discounts", "Bonus" },
            { "free parking/bus access", "Summary" },
            { "both positions require", "Required Skills" },

            { "job highlights", "Summary" },
            { "requirements", "Required Skills" },
            { "specific accountabilities", "Responsibilities" },

            { "come play with us.", "Summary" },
            { "experience in one or more of", "Required Skills" },

            { "your impact", "Responsibilities" },
            { "(find out more: cards.kik.com)", "" },
            { "what we expect ", "Required Skills" },
            { "perks", "Bonus" },

            { "transportation and housing", "Summary" },
            { "compensation and benefits information", "Bonus" },


            { "position summary", "Summary" },
            { "skills & qualifications", "Required Skills" },
            { "additional assets", "Assets" },

            { "about us", "Summary" },
            { "web developer qualities", "Required Skills" },
            { "project description", "Responsibilities" },
            { "web developer responsibilities", "Responsibilities" },
            { "what we use", "Assets" },
            { "desired skills", "Assets" },

            { "job summary", "Summary" },
            { "job responsibilities", "Responsibilities" },
            { "career development and training", "Summary" },

            { "general accountability", "Responsibilities" },
            { "nature and scope", "Summary" },
            // { "development of new web applications.", "" },
            // { "documentation of web applications.", "" },
            { "technical skills", "" },




            { "proficiency with ruby, php, or python.", "Required Skills" },
            { "proficiency with html and css.", "Required Skills" },
            { "problem solving skills", "Required Skills" },
            { "communication skills", "Required Skills" },

            // { "are you a javascript guru?", "" },
            { "minimum requirements", "Required Skills" },
            { "preferred requirements", "Assets" },
            { "other job details", "Summary" },
            { "a few other things you should know", "Bonus" },
            { "tips for your application", "Required Skills" },

            { "the ideal candidates will have", "Required Skills" },
            { "some perks of working at 500px", "Bonus" },

            { "skills and experience", "Required Skills" },
            { "education", "Required Skills" },
            // { "software engineering or equivalent!", "" },
            { "why 360incentives?", "Summary" },
            { "the following skills/experience are assets for both positions", "Assets" },
            { "experience working with programming and scripting languages such as", "Required Skills" },
            { "microsoft canada inc. information", "Summary" },
            
            { "job posting", "Summary" },
            { "candidates should be", "Required Skills" },
            { "additional skills", "Assets" },

            { "at bidvine you will", "Responsibilities" },
            { "must have", "Required Skills" },
            { "nice to have", "Assets" },


            { "The following additional skills are considered strong assets", "Assets" },

            { "what would my main duties be?", "Responsibilities" },
            { "what is d2l looking for from me?", "Required Skills" },


            { "you will have", "Required Skills" },
            { "Development experience with any of the following would be considered an asset", "Assets" },

            { "knowledge & experience", "Required Skills" },

            { "hand-holding'", "Summary" },
            { "we are", "Summary" },
            { "looking for", "Required Skills" },
            { "example projects", "Responsibilities" },
            { "tools used", "Summary" },
            { "nice-to-have skills", "Assets" },

            { "benefits", "Bonus" },

            { "about the job", "Summary" },
            { "about you", "Required Skills" },

            { "the following are also pluses", "Assets" },

            { "how to apply", "Summary" },
            { "position overview", "Summary" },
            { "qualifications", "Required Skills" },
            { "about ea", "Summary" },

            { "description", "Summary" },
            { "bonus requirements", "Summary" }
        };
        #endregion

    }
}
