# Trying out `WebView2` with Windows Forms and .NET 6 Preview 7

## To run:

- Install .NET 6.0.100-preview.7 SDK:<br> 
  https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-6.0.100-preview.7-windows-x64-installer
- Clone the repo:<br>
  `https://github.com/noseratio/WebView2WinFormsSysHotKeys .`
- Run:<br>
  `dotnet run`

## Problems

- <kbs>Alt+Space</kbd> doesn't work when the focus is inside `WebView2` (although <kbd>Alt+F4</kbd> does).
- App-specific WinForms accelerators (e.g., <kbd>Alt+X</kbd>) don't work when the focus is inside `WebView2`.
- Pressing and releasing <kbd>Alt</kbd> inside `WebView2` activates WinForms menu, but focus remains inside `WebView2`: 
  ![](https://i.stack.imgur.com/GREr6l.png)

## That's still much better than the current `BlazorWebView` behavior:

- https://github.com/noseratio/BlazorWebViewWinFormsApp