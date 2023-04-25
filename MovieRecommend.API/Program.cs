using MovieRecommend.API.ML;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

builder.Services.AddScoped<IMLService, MLService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:7033")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:3000");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    await next.Invoke();
});

app.UseSwagger().UseSwaggerUI();
app.UseHttpsRedirection().UseAuthorization();
app.MapControllers();
app.UseCors();

app.Run();
