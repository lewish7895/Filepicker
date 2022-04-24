## Filepicker
Simple CLI filepicker.

To use simply add this repo to your project and use the Filepicker class with Select method:
```
Filepicker.Select(); //current location as starting point
Filepicker.Select(new string[] { "xml", "json" } ); //force select filetype
Filepicker.Select("C:\Location"); //selects location as starting point
Filepicker.Select("C:\Location", new string[] { "xml", "json" } ); //select location + force select filetype
```

### Possibly to add in future:
- Add ability to select directory (dirpicker?)