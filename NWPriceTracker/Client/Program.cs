var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
      .AddBlazorise(options =>
      {
          options.ChangeTextOnKeyPress = true;
          options.DelayTextOnKeyPress = true;
          options.DelayTextOnKeyPressInterval = 300;
      })
      .AddBootstrapProviders()
      .AddFontAwesomeIcons();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
