SelfServe
====================

Repository for holding the SelfServe simple, standalone, HTTP server for .NET

# Use Cases

Feel free to use SelfServe however you wish. Some common use cases are file sharing or running a small personal website. 
I find it particulary useful for transferring large files between virtual machines in the cloud. 
I find the native RDP file transfer functionality to be rather slow. 
I also don't want to install IIS and FTP extensions on non-web facing machines (file and database servers). 
Whilst a FileZilla FTP server is trivial to install - I find it overkill for the task I need to perform (occasionally transferring files between cloud machines and my desktop).

# How To Use

* Compile source code.
* Drop SelfServe.exe into any folder and run.
* Navigate to http://localhost/ using any browser. SelfServe will automatically bind to localhost, 
but you can override this behaviour by starting SelfServe with a list of bindings from a command line, eg <b>SelfServe.exe http://localhost:80/ http://114.72.91.25/</b>
* Alternatively (for those who want to avoid cmd) you can rename the file in the following format <b>SelfServe.1222.1223.https.exe<b> in order to bind to https://localhost:1222/ and https://localhost:1223/
* Close SelfServe when done.