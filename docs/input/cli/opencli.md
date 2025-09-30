Title: OpenCLI Integration
Order: 15
Description: OpenCLI integration
Highlights:
 - Generate OpenCLI descriptions
---

From version `0.52.0` and above, you will be able to generate [OpenCLI](https://opencli.org)
descriptions from your `Spectre.Console.Cli` applications.

Simply add the `--help-dump-opencli` option to your application, and an 
OpenCLI description will be written to stdout.

```shell
$ ./myapp --help-dump-opencli
```

If you want to save it to disk, pipe it to a file.

```shell
$ ./myapp --help-dump-opencli > myapp.openapi.json
```