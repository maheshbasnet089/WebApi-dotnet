using webAPIDevelopment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.\
// builder.Logging.ClearProviders(); 
// builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddScoped<IPostService,PostsService>();
builder.Services.AddSingleton<IDemoService,DemoService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// app.Run(async context=>{
//     await context.Response.WriteAsync("Hello world");
// });

app.Use(async (context,next)=>{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request Host:{context.Request.Host}"); 
    logger.LogInformation("My middlware - before"); 
    await next(context); 
    logger.LogInformation("My middleware - after"); 
    logger.LogInformation($"Response StatusCode : {context.Response.StatusCode}");

});

// using(var serviceScope = app.Services.CreateScope()){
//     var services = serviceScope.ServiceProvider;
//     var demoService = services.GetRequiredService<IDemoService>();
//     var message = demoService.SayHello();
//     Console.WriteLine(message);
// }
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
