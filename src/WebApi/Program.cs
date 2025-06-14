using SchoolTripApi.Application;
using SchoolTripApi.Infrastructure;
using SchoolTripApi.WebApi;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddApplicationConfiguration(configuration);
builder.Services.AddInfrastructureConfiguration(configuration);
builder.Services.AddWebApiConfiguration(configuration);

var app = builder.Build();

var env = app.Environment;
if (env.IsDevelopment())
{
    app.UseExceptionHandler("/error-local-development");
    app.UseSwagger();
    app.UseSwaggerUI(opts => { opts.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"); });
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();