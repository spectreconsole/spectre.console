# Documentation

To start contributing to the [Spectre.Console](https://github.com/spectresystems/spectre.console) documentation, you will need the [.NET Core SDK](https://dot.net) 3.1 or higher.

## Running Preview Site

The documentation site uses [Statiq](https://statiq.dev), a static site generator. To build the documentation site run the following in a command-line terminal.

```
> dotnet run preview --virtual-dir "spectre.console"
```

After the build is complete, you can navigate to [http://localhost:5080/spectre.console](http://localhost:5080/spectre.console).

**Note that the site runs under a virtual directory.**

## Editing Content

The documentation is written using [Markdown](https://www.markdownguide.org/basic-syntax/).

Markdown files can be found under the following directories:

- [/input](./input)
  - [/appendix](./input/appendix)
    
## Editing Layout

Layout and styling can also be found in the [input](./input) directory. Look for Sass, Css, and Images under the [assets](./input/assets) directory.
    
## Custom Build Features

The documentation site has custom enhancements to Statiq located under the [./src](./src) directory. Enhancements to the build process include:

- [Extension Methods](./src/Extensions)
- [Models](./src/Models)
- [Pipelines](./src/Pipelines)
- [Shortcodes](./src/Shortcodes)
- [Utilities](./src/Utilities)

## License

MIT License

Copyright (c) 2020 Spectre Systems AB

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.