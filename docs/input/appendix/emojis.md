Title: Emojis
Order: 3
Description: "For terminals that support Emojis, *Spectre.Console* supports a simple syntax for rendering emojis."
Highlights:
    - ":rocket: becomes 🚀"
    - ":waning_gibbous_moon: becomes 🌖"
---

Please note that what emojis that can be used is completely up to 
the operating system and/or terminal you're using, and no guarantees
can be made of how it will look. Calculating the width of emojis
is also not an exact science in many ways, so milage might vary when
used in tables, panels or grids.

To ensure best compatibility, consider only using emojis introduced
before Unicode 13.0 that belongs in the `Emoji_Presentation` category
in the official emoji list at 
https://www.unicode.org/Public/UCD/latest/ucd/emoji/emoji-data.txt

## Usage

```csharp
// Markup
AnsiConsole.MarkupLine("Hello :globe_showing_europe_africa:!");

// Constant
var hello = "Hello " + Emoji.Known.GlobeShowingEuropeAfrica;
```

## Replacing emojis in text

```csharp
var phrase = "Mmmm :birthday_cake:";
var rendered = Emoji.Replace(phrase);
```

## Remapping or adding an emoji

Sometimes you want to remap an existing emoji, or 
add a completely new one. For this you can use the 
`Emoji.Remap` method. This approach works both with 
markup strings and `Emoji.Replace`.

```csharp
// Remap the emoji
Emoji.Remap("globe_showing_europe_africa", "😄");

// Render markup
AnsiConsole.MarkupLine("Hello :globe_showing_europe_africa:!");

// Replace emojis in string
var phrase = "Hello :globe_showing_europe_africa:!";
var rendered = Emoji.Replace(phrase);
```

## Emojis

_The images in the table below might not render correctly in your 
browser for the same reasons mentioned in the `Compatibility` section._


<input
    id="emojiSearch"
    type="search"
    oninput="search(this, 'emoji-results', 2)"
    placeholder="Search for emojis.."
    title="Type in an emoji name" />


<?# EmojiTable /?>