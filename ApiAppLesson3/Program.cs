
using ApiAppLesson3.Abstractions;
using ApiAppLesson3.Contexts;
using ApiAppLesson3.GraphQL;
using ApiAppLesson3.Services;
using ApiAppLesson3Market.Mappers;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;


namespace ApiAppLesson3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<StoreContext>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Services.AddSingleton<IProductServices, ProductService>().AddGraphQLServer().AddQueryType<Query>().AddMutationType<Mutation>();
            builder.Services.AddSingleton<ICategoryServices, CategoryService>();
            builder.Services.AddSingleton<IStoreServices, StoreService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddMemoryCache(m => m.TrackStatistics = true);

            var app = builder.Build();

            app.MapGraphQL();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}