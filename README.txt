This program allows one to to parse and tag job description data to help with job searches.

Main Code Structure Features:
- Description Parser (DescriptionParser.cs) - This parses the job description and categorizes the information based on heading (ie required skills, summary, responsibilities, etc.). It is an ongoing process to implement this algorithm, so the output includes the categorization and possible headings to include in the algorithm. The backbone of the algorithm is a dictionary of mappings between a list of headings and its corresponding group. This is easy to append and maintain.
- Field Displayer (FieldDisplayer.cs) - When you need to list pairs of labels corresponding to textboxes (ie display features of a job), you can provide a dictionary with a simple format and the module will handle displaying all of them sequentially. When you want to take values in all of the textboxes as input, you call extractInput, which returns a dictionary with all of the values.

Other code features:
- Main Database (Database.cs) - most data goes into a SQL Server Compact Edition database
- Txt-Based Dictionary - JobMine descriptions can be arbitrarily large so they cannot fit in the one entry in the above database. This module is a dictionary that stores each entry as a text file where the key is the file name and the value is contents of the file.
- Utility Module (Henri.cs) - this provides functions such as loading contents of a file into a string
- JobMine Data (JobMineData.cs) - this handles migrating job description data from xml format to the database.
- UI Module (Displayer.cs) - see a screenshot for the layout of the program. This handles displaying the data in the observed format.
- Ranking (Rank.cs) - One can tag jobs using 1 of 4 colours. This module handles this UI logic.

UI/UX features:
- two-panel selector
- controls resize to fit the window
- option to enter custom sql search requests
- flip through each description by clicking the entry in the DataGridView or using the arrow keys.

* Code is in the folder path "Code > JobMineDisplay > JobMineDisplay". Actual description data has been omitted.
