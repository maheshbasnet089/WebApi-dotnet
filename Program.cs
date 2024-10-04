using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using webAPIDevelopment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.\
// builder.Logging.ClearProviders(); 
// builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddScoped<IPostService,PostsService>();
builder.Services.AddSingleton<IDemoService,DemoService>();

builder.Services.AddRateLimiter(_=>{
    _.AddFixedWindowLimiter(policyName:"fixed",options=>{
        options.PermitLimit = 5; 
        options.Window = TimeSpan.FromSeconds(10); 
        options.Window = TimeSpan.FromSeconds(10); 
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; 
        options.QueueLimit = 2; 
    }); 
}); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
builder.Services.AddRateLimiter(_=>{
    _.AddFixedWindowLimiter(policyName:"fixed",options=>{
        options.PermitLimit = 5; 
        options.Window = TimeSpan.FromSeconds(10); 
        options.Window = TimeSpan.FromSeconds(10); 
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; 
        options.QueueLimit = 2; 
    }); 
}); 
app.UseRateLimiter(); 
app.MapGet("/rate-limiting",()=>Results.Ok($"Hello {DateTime.Now.Ticks.ToString()}")).RequireRateLimiting("fixed");

// app.Map("/lottery",app=>{
//     var random = new Random(); 
//     var luckyNumber = random.Next(1,6); 
//     app.UseWhen(context=>context.Request.QueryString.Value==$"?{luckyNumber.ToString()}", app=>{
//         app.Run(async context=>{
//             await context.Response.WriteAsync($"You win! You go the luck Number {luckyNumber}");
//         });
//     });
//     app.UseWhen(context=>string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app=>{
//         app.Use(async (context,next)=>{
//             var number = random.Next(1,6); 
//             context.Request.Headers.TryAdd("number", number.ToString()); 
//             await next(context); 
//         }); 
//     });
//     app.UseWhen(context=>context.Request.Headers["number"]==luckyNumber.ToString(), app=>{
//         app.Run(async context=>{
//             await context.Response.WriteAsync($"You win! You got the luck number {luckyNumber}"); 
//         });
//     }); 
//     app.Run(async context=>{
//         var number = ""; 
//         if(context.Request.QueryString.HasValue){
//             number = context.Request.QueryString.Value?.Replace("?","");
//         }else{
//             number = context.Request.Headers["number"]; 
//         }
//         await context.Response.WriteAsync($"Your number is {number}. Try again!"); 
//     });
// });
// app.Run(async context=>{
//     await context.Response.WriteAsync($"User the /lottery URL to play. You can choose your number with the format /lotterl?1.");
// });

// app.Run(async context=>{
//     await context.Response.WriteAsync("Hello world");
// });

// app.Use(async (context,next)=>{
//     var logger = app.Services.GetRequiredService<ILogger<Program>>();
//     logger.LogInformation($"Request Host:{context.Request.Host}"); 
//     logger.LogInformation("My middlware - before"); 
//     await next(context); 
//     logger.LogInformation("My middleware - after"); 
//     logger.LogInformation($"Response StatusCode : {context.Response.StatusCode}");

// });

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
