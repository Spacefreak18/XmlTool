# Xmltool #

## Overview ##

Perform basic tasks on Xml files.

Usage:
    
    Xmltool -I [InputFile] [Options]
    
    Xmltool -I C:\somefile.xml -P -Q -X C:\somefile.xslt -O C:\output.xml
    
    Xmltool -I C:\xmldirs\*.xml -P -Q -X C:\somefile.xslt -O C:\output\


## Command Line Switches ##

These switches are fairly robust. Each line represents parameters which do the same thing. They all can be called using "/", "--", or "-".

1. __I, Input__
	- Input file on which to act upon. Supports very simple file globbing. If left blank reads from standard input. Can receive a list of files from using the cmd type command.
2. __P, Pretty, Pretty-Print, Prettify__
	- Apply pretty print to input Xml files.
3. __Q, Quiet__
	- Do not write to standard output.
4. __X, Xsl, Xslt XslFile__
	- Full path to Xsl file to apply to input.
5. __O, OutFolder, OutFile__
	- Location to folder to place modified files. Can also be a single file, which would be overwritten each time if multiple input files were found. A warning is given, but that does not work when reading from standard input.
6. __E, Ext__
	- Use an extension other than Xml for matching. This defaults to Xml if not specified. Just specify the extension without any . or *. If you use globbing in the input, this will have no effect.
7. __L, Log__
	- Full path name to file to dump output instead of standard output. Will turn quiet off, unless turned on.



## ToDo ##

1. Add functionality to add specified node to specified location using XPath.
