Title: Short title of the Widget being documented. Typically the class name
Order: 99999
Description: Description of the widget. This will be displayed on social card
Highlights:
- List features. 
- These will be used for the social card.
- Keep to three items.
Reference: 
- T:Spectre.Console.BarChart
Hidden: true    
---

<!---
Documentation steps
1. Edit front matter. Change all fields. Order dictates how it is sorted in the sidebar. Remove hidden attributes.
   Make sure to reference the appropriate XMLDOC page. You can find this by looking in the generated HTML
   of the API reference section. You can reference multiple items e.g. types, methods, etc that are related to the Widget. 
2. Remove comments as you edit the fields.
3. All widgets should have at minimum description and a usage section. 
-->

<!---
Short description of the widget. Can be the same as the description above
-->


<!---
Optional: Embed an asciicast. The cast parameter should be the base name of the cast. There are two files, 
one suffixed with -rich.cast and a second named -plain.cast. The cast attribute should be the name without
the suffix. 

To generate a new cast file, open the \resources\scripts\Generator\Generator.sln project and add a new sample in the
Commands/AsciiCast/Samples/ folder. If the widget is static such as a tree or a table, try and animate the widget
using the Live widget to change the content or styling. 

Running the generator project with by executing

dotnet run -- samples -l

and pick your sample. This will generate a new asciicast in the docs/input/assets/casts folder which can then be referenced via:

<?# AsciiCast cast="sample-name" /?>
-->



## Usage

### Basic usage

<!---
Code sample for a default output of the widget. Code Samples can be embedded with a markdown code block or
linked to via the Example snipped. The example snippet takes the XMLDOC reference of the snippet from the Examples
project that you want to reference.

If linking to a method it will, by default, only include the method body. Include BodyOnly="false" to include
the entire method including the declaration.

<?# Example symbol="M:Prompt.Program.AskConfirmation" /?>
-->

### Additional Styling

<!---
Include additional examples of styling or functionality
-->

### More styling and functions

<!---
Include additional examples of styling or functionality
-->