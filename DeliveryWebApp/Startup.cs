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




        //Este método é utilizado para realizar a configuração
        //e inicialização dos serviços e funcionalidades utilizadas
        //pelo aplicativo.
        public void ConfigureServices(IServiceCollection services)
        {

            //configuração do banco de dados (SQL Server).
            //Esta configuração é valida para qualquer banco SQL Server, mas
            //neste projeto utilizados o LocalDB, conforme pode ser observado
            //no arquivo appsettings.json
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));


            //aqui adicionamos o sistema de contas de usuários e associamos
            //as regras de usuários (Roles) administradores e também associamos
            //o banco de dados utilizado via Entity Framework
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            //esta configuração determina a força da senha exigida pelo sistema
            //quando um novo usuário criar uma nova conta. Por motivos didáticos,
            //todas as opções estão desativadas (senha fraca).
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


            //adiciona o padrão MVC ao projeto
            services.AddControllersWithViews();


            //habilita a razor engine (permite usar c# dentro do HTML)
            services.AddRazorPages();


            //adiciona sessões de usuário para armazenar dados temporários
            services.AddSession();
        }

        // Este método é chamado assim que o app é iniciado para configurar quaisquer serviços necessários
        // ou realizar configurações adicionais
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {

            // se estiver em modo de desenvolvimento, apresenta telas de erro
            // e informações úteis para o desenvolvedor, como rastreamento de pilha de chamadas
            // do contrário, oculta isso do usuário final (ambiente de produção)
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

            //permite usar arquivos estáticos definidos na pasta wwwroot (.css, .js, .jpg)
            app.UseStaticFiles();

            // habilita o uso de rotas nos endereços das páginas
            // http://meusite.com?p=1&q=2&c=3 pode ser mapeado para http://meusite.com/1/2/3
            app.UseRouting();

            // habilita a sessão de usuário para armazenar dados temporários
            app.UseSession();

            //habilita a autenticação e autorização para que o usuário possa 
            //efetuar login no site e acessar somente as áreas que ele tem direito de acesso
            app.UseAuthentication();
            app.UseAuthorization();


            //configura a forma padrão com que as páginas são endereçadas
            // www.meusite.com/NomeDoControlador/NomeDaAção/Dados
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


            //cria um usuário administrador padrão
            //a senha pode ser mudada no formulário padrão do site
            if (userManager.FindByNameAsync("bruno@meusite.com").Result == null)
            {
                //cria um objeto que representa um novo usuário
                IdentityUser user = new IdentityUser
                {
                    UserName = "bruno@meusite.com",
                    Email = "bruno@meusite.com"
                };

                //registra o novo usuário no banco de dados e atribui uma senha para ele
                IdentityResult result = userManager.CreateAsync(user, "@pass123").Result;

                if (result.Succeeded)
                {
                    //se tudo correu bem, associe o usuário criado para uma conta de administrador
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }


        }
    }
}
