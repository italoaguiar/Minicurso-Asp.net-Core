using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using DeliveryWebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeliveryWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }




        //Este m�todo � utilizado para realizar a configura��o
        //e inicializa��o dos servi�os e funcionalidades utilizadas
        //pelo aplicativo.
        public void ConfigureServices(IServiceCollection services)
        {

            //configura��o do banco de dados (SQL Server).
            //Esta configura��o � valida para qualquer banco SQL Server, mas
            //neste projeto utilizados o LocalDB, conforme pode ser observado
            //no arquivo appsettings.json
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));


            //aqui adicionamos o sistema de contas de usu�rios e associamos
            //as regras de usu�rios (Roles) administradores e tamb�m associamos
            //o banco de dados utilizado via Entity Framework
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            //esta configura��o determina a for�a da senha exigida pelo sistema
            //quando um novo usu�rio criar uma nova conta. Por motivos did�ticos,
            //todas as op��es est�o desativadas (senha fraca).
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });


            //adiciona o padr�o MVC ao projeto
            services.AddControllersWithViews();


            //habilita a razor engine (permite usar c# dentro do HTML)
            services.AddRazorPages();


            //adiciona sess�es de usu�rio para armazenar dados tempor�rios
            services.AddSession();
        }

        // Este m�todo � chamado assim que o app � iniciado para configurar quaisquer servi�os necess�rios
        // ou realizar configura��es adicionais
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {

            // se estiver em modo de desenvolvimento, apresenta telas de erro
            // e informa��es �teis para o desenvolvedor, como rastreamento de pilha de chamadas
            // do contr�rio, oculta isso do usu�rio final (ambiente de produ��o)
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //adiciona redirecionamento seguro (HTTPS)
            app.UseHttpsRedirection();

            //permite usar arquivos est�ticos definidos na pasta wwwroot (.css, .js, .jpg)
            app.UseStaticFiles();

            // habilita o uso de rotas nos endere�os das p�ginas
            // http://meusite.com?p=1&q=2&c=3 pode ser mapeado para http://meusite.com/1/2/3
            app.UseRouting();

            // habilita a sess�o de usu�rio para armazenar dados tempor�rios
            app.UseSession();

            //habilita a autentica��o e autoriza��o para que o usu�rio possa 
            //efetuar login no site e acessar somente as �reas que ele tem direito de acesso
            app.UseAuthentication();
            app.UseAuthorization();


            //configura a forma padr�o com que as p�ginas s�o endere�adas
            // www.meusite.com/NomeDoControlador/NomeDaA��o/Dados
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


            //cria um usu�rio administrador padr�o
            //a senha pode ser mudada no formul�rio padr�o do site
            if (userManager.FindByNameAsync("bruno@meusite.com").Result == null)
            {
                //cria um objeto que representa um novo usu�rio
                IdentityUser user = new IdentityUser
                {
                    UserName = "bruno@meusite.com",
                    Email = "bruno@meusite.com"
                };

                //registra o novo usu�rio no banco de dados e atribui uma senha para ele
                IdentityResult result = userManager.CreateAsync(user, "@pass123").Result;

                if (result.Succeeded)
                {
                    //se tudo correu bem, associe o usu�rio criado para uma conta de administrador
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }


        }
    }
}
