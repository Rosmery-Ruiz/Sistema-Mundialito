using Microsoft.EntityFrameworkCore;
using Mundialito.Infrastructure.Persistence;
using Mundialito.Application.Common.Interfaces;
using Mundialito.Infrastructure.Persistence.UnitOfWork;
using Mundialito.Infrastructure.Repositories;
using Mundialito.Application.Team.Commands;
using Mundialito.Infrastructure.Persistence.Connections;
using Mundialito.Infrastructure.Queries;
using Mundialito.Application.Team.Queries;
using Mundialito.Application.Player.Commands;
using Mundialito.Application.Player.Queries;
using Mundialito.Application.Match.Commands;
using Mundialito.Application.Match.Queries;
using Mundialito.Application.Standings.Queries;
using Mundialito.Application.Scorers.Queries;
using Mundialito.Infrastructure.Persistence.Repositories;
using Mundialito.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MundialitoDbContext>
    (options =>options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<CreateTeamCommandHandler>();
builder.Services.AddScoped<IDbConnectionFactory>(provider =>
{
    var connectionString = builder.Configuration
        .GetConnectionString("DefaultConnection");

    return new SqlConnectionFactory(connectionString!);
});
builder.Services.AddScoped<ITeamQueryRepository, TeamQueryRepository>();
builder.Services.AddScoped<GetTeamsQueryHandler>();
builder.Services.AddScoped<UpdateTeamCommandHandler>();
builder.Services.AddScoped<DeleteTeamCommandHandler>();

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<CreatePlayerCommandHandler>();
builder.Services.AddScoped<IPlayerQueryRepository, PlayerQueryRepository>();
builder.Services.AddScoped<GetPlayersQueryHandler>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<CreateMatchCommandHandler>();
builder.Services.AddScoped<RegisterMatchResultCommandHandler>();
builder.Services.AddScoped<IMatchQueryRepository, MatchQueryRepository>();
builder.Services.AddScoped<GetMatchesQueryHandler>();
builder.Services.AddScoped<IStandingsQueryRepository, StandingsQueryRepository>();
builder.Services.AddScoped<GetStandingsQueryHandler>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IScorerQueryRepository, ScorerQueryRepository>();
builder.Services.AddScoped<GetScorersQueryHandler>();
builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("Frontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<MundialitoDbContext>();

    await DatabaseSeeder.SeedAsync(dbContext);
}

app.Run();
