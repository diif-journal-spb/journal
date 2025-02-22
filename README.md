Differential Equations and Control Processes
============================================


This repository contains sources of the sceintific journal as 
well as publication ```.pdf``` files and the tool 
we use to generate the journal


License
=======

Repository contains author publication papers which are licensed by it's authors. 

The tool for journal genertion is licensed under GNU GPL license.

Pather tool is also provided under GNU GPL license.

There are also third-party libraries that are licensed by their own licenses as well



How to publish next number
--------------------------

Creating New Journal Issue
==========================

Tools to have:
 - Git (2.10+)
 - Microsoft Visual Studio 2015 (.NET SDK)
 - Nant 
 - Microsoft Office / Word (for `.docx` file references processing, optional)
 - Google Chrome

To start with the number one should create stub files in `journalGenerator\data` folder:

 - `number<YEAR>-<ISSUE>.number`. Only root XML element should be there with correct year and issue
 - `number<YEAR>-<ISSUE>.authors`. Only root XML element should be there. 

As example, please see similar existing files from previous issues. Replase `<YEAR>` with 
current year (4 digits) and `<ISSUE>` with current issue (1 digit).

Execute in the `journalGenerator\data` folder 
 - `git add number<YEAR>-<ISSUE>.*`
 - `git commit number<YEAR>-<ISSUE>.* -m "Create next number stub"`



Authoring a publication
=======================

Execute editor UI
 - open console (cmd.exe) in the folder 
 - type `nant run-ui`

You'll see publication editor window. The tool to markup publicaion by selection parts of
text and assigning a role of it (e.g. names, abstracts, ...).
The tool is smart to process references and keywords from most of formats. Also it is capable
of title case for russian and english.

In the application click on `New EN` or `New EN/RU` action depending on the paper language.
Press `Open Files` and select all files from the folder for the publication you have. 
We expect references are provided in `.docx` format, paper publication is in `.pdf` format.
You should not select unrelevanted or non-text files.

Mark all publication information, for that
 - select meaningful part of text
 - use context menu to select role
 - releat


Author selection
================

There are two possibilities to define the publication author
 - fill all parameters manually
 - re-use existing author ID

To use exiting author you should search for athor name in all files under `journalGenerator\data`
folder, copy-paste author ID from the found entry and use `New Author | Add Exact Author` menu.


Keywords selection
==================
Keywords are automatically parsed and included. All you need it so select all keywords once and 
call ` Article | <LANG> | Set Keywords <LANG>` action

References selection
====================
Reference are automatically extracted from a `.docx` file. All you need to select all references
at once and use `Article | <LANG> | Set References <LANG>` action.


Selecting Article PDF file
==========================

Rename article pdf file to the name that does not overlap with existing files under `journalGenerator\pdf`.
Copy the new file to the folder. Call `git add <FILE>.pdf` from the `journalGenerator\pdf` folder.

Select the file name in the editor application and call `Article | Set pdf` action.


Publishing XML data
===================

Click `Copy Generated Article` button in the application. Paste the XML data into `number<YEAR>-<ISSUE>.number` file
under the root element of the XML.

Make sure you use `utf-8` encoding in the file. It's too easy to use `windows-1251` with incorrect encoding declaration
in the XML file. We have to stick to UTF-8.

Do the same with `Copy Generated Authors` and place it to `number<YEAR>-<ISSUE>.authors` file. You should not copy
entries for existing authors, which are also generated by the programs by mistake.


Call `git add number<YEAR>-<ISSUE>.*`.


Checking and Migrating Data
============================

Execute `nant migrate` command and make sure it succeeds. It may fail because of 
incorrect data or onknown organization. Of course it may fail because of something else. You may need to check
sources to figure out what went wrong.


The migrate task also create a `.text` file under the `journalGenerator\pdf.text` folder. So call
`git add journalGenerator\pdf.text\*.text` to include those changes.


Preview of the Journal
======================

Type `nant debug` to preview local version of the journal in `Google Chrome`.

Check if the publication looks good. Fix XML if necessary and repeat.


Commit change to Git repository
===============================

Check all changed files are included into Git, call `git status`. Use `git add` to add missing or not staged 
for commit files.

Call `git commit -m "<AUTHOR> <YEAR> <ISSUE>"` command to commit changes.

 

 










