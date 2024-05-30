Register the services

```c#
 private static IServiceCollection AddResponseCompression(this IServiceCollection services) {
        return services.AddResponseCompression(options =>
        {
          options.EnableForHttps = true;
          options.Providers.Add<GzipCompressionProvider>();
        }).Configure<GzipCompressionProviderOptions>(options =>
        {
          options.Level = CompressionLevel.Fastest;
        });
      }


```

Setup usage
```c#
builder.UseResponseCompression()
```
