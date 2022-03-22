Title: Best Practices
Order: 21
Description: Best practices when working with Spectre.Console and how to troubleshoot when things go wrong.
---

Spectre.Console works hard to make writing console applications easier for developers.
It is designed to be easy to use and to be flexible, but there are some things that can be a bit tricky.

## Output

Spectre.Console has a number of output methods and functions, but they rely on the user's console to render.

Many things make up what a console can render.

* It's output encoding.
* The console font.
* The size of the windows.
* The codepage.
* The terminal itself.
* The console's background color.

Many of these items can be auto-detected by Spectre.Console.
Some of these items can only be configured by the user and cannot be detected automatically.

Spectre.Console can detect the following items:

* Output encoding: the built-in widgets will use the encoding that is detected to fallback when needed when UTF-8 is not
  detected e.g. if a `Table` is configured to use a rounded border, but a
  user's output encoding does not support the extended ASCII characters then a fallback set of characters will be used.
* Size of the windows: at the time of writing to the screen, Spectre.Console will know the number of characters when
  writing full-width widths.
* Most terminals: Spectre.Console will try to detect the running Console and match
  their [capabilities](xref:T:Spectre.Console.Capabilities).

Things that cannot be detected automatically:

* Console font: Spectre.Console will assume a relatively modern and fixed width font. If a user's console font is not
  fixed width, then the output for some items such as `Table` will be incorrect.
  The supported characters can also vary between fonts, especially with some modern features like Powerline characters
  and NerdFonts.
* The background color and configured foreground colors.

### Output Best Practices

**Do** test your application in multiple terminals. On Windows machines, the built-in `cmd.exe` and `Windows Terminal`
both work well for a variety of capabilities.

**Do not** hard-code emojis and extended unicode characters in your code as default output strings.
There is no guarantee that the user's console will support them and Spectre.Console will not be able to fall back
automatically.
To ensure the widest level of support for various terminals, allow users to either opt in to the use of extended unicode
characters or, depending on the type of application,
allow the user to customize the output and configure those characters by hand.

**Do not** assume a user's background color is black.
It can be any color, including white.
While Spectre.Console allows you to use up to 24-bit colors, don't assume they'll look good when displayed on the user's
console. If you stick to the standard 16 ANSI colors,
Spectre.Console will tell your terminal to use the color that is configured in the user's terminal theme.
If you are using an 8 or 24-bit color for the foreground text, it is recommended that you also set an appropriate
background color to match.

**Do** escape data when outputting any user input or any external data via Markup using the [`EscapeMarkup`](xref:M:Spectre.Console.Markup.Escape(System.String)) method on the data. Any user input containing `[` or `]` will likely cause a runtime error while rendering otherwise.

**Consider** replacing `Markup` and `MarkupLine` with [`MarkupInterpolated`](xref:M:Spectre.Console.AnsiConsole.MarkupInterpolated(System.FormattableString)) and [`MarkupLineInterpolated`](xref:M:Spectre.Console.AnsiConsole.MarkupLineInterpolated(System.FormattableString)). Both these methods will automatically escape all data in the interpolated string holes. When working with widgets such as the Table and Tree, consider using [`Markup.FromInterpolated`](xref:M:Spectre.Console.Markup.FromInterpolated(System.FormattableString,Spectre.Console.Style)) to generate an `IRenderable` from an interpolated string.

### Live-Rendering Best Practices

Spectre.Console has a variety of [live-rendering capabilities](live) widgets. These widgets can be used to display data
that are updated and refreshed on the user's console.
To do so, the widget rendering has a render loop that writes to the screen and then erases the previous output before
writing again.

**Do** keep your rendering logic as simple as possible on a single thread. Rendering additional content while a live
rendering is in progress can cause the rendering to be corrupted.

**Do not** use multiple live-rendering widgets simultaneously. Like the previous tip, displaying a `Status` control and
a `Progress` is not supported and can cause rendering corruption.

**Do** create additional work threads within the `Start` method, **but** render on the main thread.
For items such as a progress bar, multiple threads or tasks can be created from the `Start` method for concurrent
execution. To ensure proper rendering, ensure that calls to update the widgets are done
on the main thread.

### Unit Testing Best Practices

For testing of console output, Spectre.Console has [`IAnsiConsole`](xref:T:Spectre.Console.IAnsiConsole) that can be
injected into your application.
The [Spectre.Console.Test](https://www.nuget.org/packages/Spectre.Console.Testing/) contains a set of utilities for
capturing the output for verification, either manually or via a tool such
as [Verify](https://github.com/VerifyTests/Verify).

### Analyzer for Best Practices

Spectre.Console has an [analyzer](https://www.nuget.org/packages/Spectre.Console.Analyzer) that helps prevent some
common errors in writing console output from above such as using multiple live rendering widgets simultaneously,
or using the static `AnsiConsole` class when `IAnsiConsole` is available.

### Configuring the Windows Terminal For Unicode and Emoji Support

Windows Terminal supports Unicode and Emoji. However, the shells such as Powershell and cmd.exe do not.
For the difference between the two,
see [What's the difference between a console,
a terminal and a shell](https://www.hanselman.com/blog/whats-the-difference-between-a-console-a-terminal-and-a-shell).

For PowerShell, the following command will enable Unicode and Emoji support. You can add this to your `profile.ps1`
file:

```powershell
[console]::InputEncoding = [console]::OutputEncoding = [System.Text.UTF8Encoding]::new()
```

For cmd.exe, the following steps are required to enable Unicode and Emoji support.

1. Run `intl.cpl`.
2. Click the Administrative tab
3. Click the Change system locale button.
4. Check the "Use Unicode UTF-8 for worldwide language support" checkbox.
5. Reboot.

You will also need to ensure that your Console application is configured to use a font that supports Unicode and Emoji,
such as Cascadia Code.  