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
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();


app.UseSwagger().UseSwaggerUI();
app.UseHttpsRedirection().UseAuthorization();
app.MapControllers();
app.UseCors();

app.Run();
