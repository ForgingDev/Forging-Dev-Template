using Forging.Core.Interfaces.IRepositories;
using Forging.Core.Repositories;
using Forging.Core.UseCases.RoleUseCases;
using Forging.Core.UseCases.UserUseCases;

namespace Forging.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //* Role UseCases
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<GetRolesUseCase>();
            services.AddScoped<GetRoleByIdUseCase>();
            services.AddScoped<GetUserRolesUseCase>();
            services.AddScoped<CreateRoleUseCase>();
            services.AddScoped<AddUserRoleUseCase>();
            services.AddScoped<UpdateRoleUseCase>();
            services.AddScoped<DeleteRoleUseCase>();
            services.AddScoped<RemoveUserRoleUseCase>();

            //* User UseCases
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<GetAllUsersUseCase>();
            services.AddScoped<GetUserByIdUseCase>();
            services.AddScoped<CreateUserUseCase>();
            services.AddScoped<UpdateUserUseCase>();
            services.AddScoped<DeleteUserUseCase>();

            services.AddCors(opt =>
            {
                opt.AddPolicy(
                    "CorsPolicy",
                    policy =>
                    {
                        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                    }
                );
            });

            return services;
        }
    }
}
