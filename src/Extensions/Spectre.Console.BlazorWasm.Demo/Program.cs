using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Spectre.Console.BlazorWasm;
using Spectre.Console.BlazorWasm.Demo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register the BlazorConsoleService as a singleton
builder.Services.AddSingleton<BlazorConsoleService>();

await builder.Build().RunAsync();
