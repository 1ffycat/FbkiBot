using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

var app = builder.Build();
app.Run();