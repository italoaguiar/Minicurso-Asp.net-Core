using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Data;
using DeliveryWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;



        public CarrinhoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        // GET: Carrinho
        public ActionResult Index()
        {

            //verifica se existe um carrinho de compras para o usuário atual
            var cart = SessionHelper.GetObjectFromJson<List<CarrinhoModel>>(HttpContext.Session, "cart");

            if(cart != null)
            {
                //busca no banco de dados os itens do carrinho e os coloca em uma lista
                var r = cart.Select(x => new CarrinhoModel()
                {
                    Id = x.Id,
                    Quantidade = x.Quantidade,
                    Item = _context.ItemsCardapio.Find(x.Id)
                }).ToList();


                //calcula o valor total do carrinho
                var total = r.Sum(x => x.Quantidade * x.Item.Preco);
                ViewBag.Total = total;

                return View(r);
            }

            //limpa o total do carrinho (carrinho vazio)
            ViewBag.Total = 0;

            return View();
        }




        //adiciona um item ao carrinho o usuário
        [HttpPost]
        public ActionResult Adicionar([Bind("Id,Quantidade")]CarrinhoModel model)
        {
            //se não houver erros no parãmetro do método
            if (ModelState.IsValid)
            {
                //se não existir nenhum dado armazenado na sessão do usuário atual
                if (SessionHelper.GetObjectFromJson<List<CarrinhoModel>>(HttpContext.Session, "cart") == null)
                {
                    //cria uma nova lista de itens do carrinho de compras
                    List<CarrinhoModel> cart = new List<CarrinhoModel>();

                    //adiciona o item informado no parâmetro para a lista
                    cart.Add(model);

                    //salva a lista na sessão do usuário
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    //obtém a lista de itens do carrinho do usuário
                    List<CarrinhoModel> cart = SessionHelper.GetObjectFromJson<List<CarrinhoModel>>(HttpContext.Session, "cart");
                    
                    //pesquisa se o item informado no parâmetro já existe no carrinho
                    CarrinhoModel item = cart.FirstOrDefault(x => x.Id == model.Id);

                    //se o item já existir, aumente a quantidade
                    if (item!= null)
                    {
                        item.Quantidade += model.Quantidade;
                    }
                    else
                    {
                        //se o item não existir, adicione-o ao carrinho
                        cart.Add(model);
                    }

                    //salve as alterações na sessão do usuário
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
            }
            

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Remover(Guid id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CarrinhoModel>>(HttpContext.Session, "cart");

            if (cart != null)
            {
                cart.Remove(cart.First(x => x.Id == id));
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }




        // salva o carrinho do usuário no banco de dados
        // o atributo [Authorize] exige que o usuário faça
        // login caso ainda não tenha feito.

        [Authorize]
        public async Task<ActionResult> Finalizar()
        {
            // verica se há itens no carrinho do usuário.
            // se não houver, não há nada a ser feito.
            var cart = SessionHelper.GetObjectFromJson<List<CarrinhoModel>>(HttpContext.Session, "cart");
            if(cart != null)
            {
                //cria um novo pedido
                Pedido p = new Pedido();

                //atribui o cliente que fez login ao pedido
                p.Cliente = await _userManager.GetUserAsync(User);

                //atribui o id do cliente que fez login ao pedido
                p.ClientId = new Guid(p.Cliente.Id);

                //atribui o horário atual ao pedido
                p.DataPedido = DateTime.Now;

                //cria uma nova lista de itens do pedido
                p.Items = new List<PedidoItem>();


                //busca no banco de dados todos os itens do carrinho do usuário
                var items = cart.Select(x => new CarrinhoModel()
                {
                    Id = x.Id,
                    Quantidade = x.Quantidade,
                    Item = _context.ItemsCardapio.Find(x.Id)
                }).ToList();

                //calcula o valor total do pedido
                p.total = items.Sum(x => x.Quantidade * x.Item.Preco);

                //para cada item no carrinho do usuário, adicione-o ao pedido
                foreach (var item in items)
                {
                    p.Items.Add(
                        new PedidoItem()
                        {
                            Item = item.Item,
                            Quantidade = item.Quantidade
                        }
                    );
                }

                //adicione o novo pedido à lista de transações do banco de dados
                _context.Pedidos.Add(p);

                //solicite ao banco de dados que salve os dados
                await _context.SaveChangesAsync();

                //limpe os dados temporários da sessão do usuário (esvaziar carrinho)
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);

                return RedirectToAction("Index", "Pedidos");
            }


            return RedirectToAction("Index");
        }

        
    }
}