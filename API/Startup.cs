using API.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Models.TodoItems.Repositories;
using Models.Users.Repositories;

namespace API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO что лучше для БД?
            //services.AddScoped<ITodoRepository>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IUserRepository, MongoUserRepository>();
            services.AddSingleton<ITodoRepository, MongoTodoRepository>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            //TODO сохранять роуты где-то
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/v1/todo"),
                appBuilder => { appBuilder.UseMiddleware<AuthMiddleware>(); });

            app.UseMvc();
        }
    }
}