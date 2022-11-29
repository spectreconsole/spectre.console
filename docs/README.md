# Documentation

To start contributing to the [Spectre.Console](https://github.com/spectreconsole/spectre.console) documentation, you will need the [.NET Core SDK](https://dot.net) 7.0.100 or higher (as defined in the repository root `global.json` file).

## Running Preview Site

The documentation site uses [Statiq](https://statiq.dev), a static site generator. To build the documentation site run the following in a command-line terminal.

```
> Preview.ps1
```

After the build is complete, you can navigate to [http://localhost:5080](http://localhost:5080).

## Npm

The site uses some tools from the JavaScript ecosystem including npm. While Statiq will execute `npm install` and other commands as needed, you need to have [npm installed](https://www.npmjs.com/get-npm) before running a site build.

## Editing Content

The documentation is written using [Markdown](https://www.markdownguide.org/basic-syntax/).

Markdown files can be found under the following directories:

- [/input](./input)
- [/appendix](./input/appendix)
    
## Editing Layout

Layout and styling can also be found in the [input](./input) directory. Look for Sass, Css, and Images under the [assets](./input/assets) directory.
    
## Custom Build Features

The documentation site has custom enhancements to Statiq located under the [./src](./src) directory.

## License

MIT License

Copyright (c) 2020 Patrik Svensson, Phil Scott, Nils Andresen

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.