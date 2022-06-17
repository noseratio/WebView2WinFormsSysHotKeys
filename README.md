# Trying out `WebView2` with a .NET 6 Windows Forms app

## To run:

- Install .NET 6.0 SDK<br> 
  https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.301-windows-x64-installer
- Clone the repo:<br>
  `https://github.com/noseratio/WebView2WinFormsSysHotKeys .`
- Run:<br>
  `dotnet run`

## Problems

- <kbd>Alt+Space</kbd> doesn't work when the focus is inside `WebView2` (although <kbd>Alt+F4</kbd> does).
- App-specific WinForms accelerators (e.g., <kbd>Alt+X</kbd>) don't work when the focus is inside `WebView2`.
- Pressing and releasing <kbd>Alt</kbd> inside `WebView2` activates WinForms menu, but focus remains inside `WebView2`: 
  ![](https://i.stack.imgur.com/GREr6l.png)

## Linked `BlazorWebView` issue:

- https://github.com/dotnet/maui/issues/2341