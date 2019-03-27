using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Mvc;
using BookStore.Application;
using BookStore.Domain.Models;
using BookStore.Repositories;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.DependencyInjection;

[assembly: OwinStartupAttribute(typeof(BookStore.Startup))]

namespace BookStore
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // We will use Dependency Injection for all controllers and other classes, so we'll need a service collection
            var services = new ServiceCollection();

            // configure all of the services required for DI
            ConfigureServices(services);

            ConfigureAuth(app);

            app.MapSignalR();

            // Create a new resolver from our own default implementation
            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());

            // Set the application resolver to our default resolver. This comes from "System.Web.Mvc"
            //Other services may be added elsewhere
            DependencyResolver.SetResolver(resolver);

            GlobalHost.DependencyResolver.Register(typeof(IBookStoreDb), ()=> resolver.GetService<IBookStoreDb>());
            GlobalHost.DependencyResolver.Register(typeof(IBookStoreApp), () => resolver.GetService<IBookStoreApp>());
        }

        public void ConfigureAuth(IAppBuilder app)
        {

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/")
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add the bookstore db as an IBookStoreDb - this can be instantiated with no parameters
            services.AddTransient(typeof(IBookStoreDb), typeof(BookStoreDb));

            // Add the Bookstore App as a singleton - it requires an IBookStoreDb
            services.AddSingleton(typeof(IBookStoreApp), typeof(BookStoreApp));

            // Add the owin authentication manager as a service
            services.AddTransient(typeof(Microsoft.Owin.Security.IAuthenticationManager), p => new OwinContext().Authentication);
            
            // Add all controllers as services
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Where(t => typeof(IController).IsAssignableFrom(t)
            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
        }
    }
}