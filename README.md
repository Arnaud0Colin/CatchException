# CatchException

CatchException (Manage Exception c#)
The library capable to catch Unhandled exception and write exception on logging file, in database and by email. Can also Write a trace file, Dump object.

I made this module to facilitate bug hunting, be really proactive, corrected the error before the user does not call you to report the problem.
This program has basically made for enterprise who developer software internally


## Usage example

```c#
    try
    {
 // Code
    }
    catch(Exception ex)
    {
        CatchMe.WriteException(ex).Where().Write();
    }
```

## Previous Release

[ReadMe of the Version 1](V1/readme.md)
