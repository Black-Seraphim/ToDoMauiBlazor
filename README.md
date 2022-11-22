# ToDoMauiBlazor

## Description
Just another version of my previous build ToDo-App.
This time I created it in Blazor using MAUI to work like an app.

The whole code is written in C#. The UI is created using HTML and CSS. No JS is needed.

## Summary
It looks like .NET MAUI or the combination of .NET MAUI and Blazor is still in its infancy in some places. Nevertheless, it is already showing itself as a serious alternative to simple WPF apps. The binding seems to behave much more expectedly. Functions that are not delivered can usually be solved by very simple and comprehensible workarounds.

## Database
SQLite is used in combination with Entity Framworke Core.
Due to some bugs, the EF Tools won't work in MAUI Apps, so I made an additional app, just to create an empty database and copied it to the project.

## Known Issues
Individual functions, such as setting the focus in an input field, are still not implementable with C#, but can be supplemented with classic JS code if required.
Currently there is no reasonable feedback of the methods to make the validation visible to the outside. Nothing unexpected happens, but you can't see why nothing happens either. This should happen on occasion through the integrated validation, so that error messages are classically routed through exceptions to the user level.

## Framework
.NET 6.0 LTS
.NET MAUI
.NET Blazor
HTML5
CSS3
