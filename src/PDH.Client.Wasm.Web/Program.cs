using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PDH.Client.Wasm.Web;
using PDH.Client.Wasm.Web.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var config = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClients(config);
builder.Services.AddClientServices();
builder.Services.AddAuth(config);


await builder.Build().RunAsync();