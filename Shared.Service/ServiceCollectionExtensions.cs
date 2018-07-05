using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection; 
using MediatR;
using Microsoft.AspNetCore.Builder; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; 

namespace Shared.Service
{ 

    public static class ServiceCollectionExtensions
    {
        public static void ResolveAllTypes(this IServiceCollection services, string solutionPrefix,
            params string[] projectSuffixes)
        {
            //solutionPrefix is my Solution name, to separate with another assemblies of Microsoft,...
            //projectSuffixes is my project what i want to scan and register
            //Note: To use this code u must reference your project to "projectSuffixes" projects.

            var allAssemblies = new List<Assembly>();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (var dll in Directory.GetFiles(path, "*.dll"))
                allAssemblies.Add(Assembly.LoadFile(dll));


            var types = new List<Type>();
            foreach (var assembly in allAssemblies)
            {
                if (assembly.FullName.StartsWith(solutionPrefix))
                {
                    foreach (var assemblyDefinedType in assembly.DefinedTypes)
                    {
                        if (projectSuffixes.Any(x => assemblyDefinedType.Name.EndsWith(x)))
                        {
                            types.Add(assemblyDefinedType.AsType());

                        }
                    }
                }
            }

            var implementTypes = types.Where(x => x.IsClass).ToList();
            foreach (var implementType in implementTypes)
            {
                //I default "AService" always implement "IAService", You can custom it
                var interfaceType = implementType.GetInterface("I" + implementType.Name);

                if (interfaceType != null)
                {
                    services.Add(new ServiceDescriptor(interfaceType, implementType,
                        ServiceLifetime.Scoped));
                }

            }

        }

        public static void ConfigureHandlers(this IServiceCollection services, params Type[] handlerAssemblyMarkerTypes)
        {
            services.AddMvc();

            services.AddMediatR(handlerAssemblyMarkerTypes);
        }


        public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
        {
            var context = app.ApplicationServices.GetService<T>();
            context.Database.Migrate();
        }
        public static void EnsureMigrationOfContext(this IApplicationBuilder app, DbContext context) 
        { 
            context.Database.Migrate();
        }
    }
}
