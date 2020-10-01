Title: Emojis
Order: 3
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

# Usage

```csharp
// Markup
AnsiConsole.MarkupLine("Hello :globe_showing_europe_africa:!");

// Constant
var hello = "Hello " + Emoji.Known.GlobeShowingEuropeAfrica;
```

# Replacing emojis in text

```csharp
var phrase = "Mmmm :birthday_cake:";
var rendered = Emoji.Replace(phrase);
```

# Emojis

_The images in the table below might not render correctly in your 
browser for the same reasons mentioned in the `Compatibility` section._

<div class="mb-3">
    <div class="form-inline d-flex">
      <i class="fas fa-search" aria-hidden="true"></i>
      <input id="emoji-search" 
        class="form-control form-control-sm ml-3 w-75" 
        type="text" placeholder="Search Emojis..." autocomplete="off" 
        aria-label="Search Emojis">
    </div> 
</div>

<?# EmojiTable /?>

<script type="text/javascript" src="../assets/js/emoji-search.js"></script>