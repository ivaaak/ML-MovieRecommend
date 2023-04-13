using MovieRecommend.API.ML;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

builder.Services.AddScoped<IMLService, MLService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
          builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
}

app.UseHttpsRedirection().UseAuthorization();
app.MapControllers();

app.Run();
