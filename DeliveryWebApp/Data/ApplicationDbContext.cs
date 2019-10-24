using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        //este método configura a regra de acesso de administrador no DB
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            );
        }

        // determina a tabela de Items do cardápio
        public DbSet<ItemCardapio> ItemsCardapio { get; set; }

        //determina a tabela de associa os itens ao um pedido
        public DbSet<PedidoItem> PedidoItens { get; set; }

        //determina a tabela que armazena os pedidos
        public DbSet<Pedido> Pedidos { get; set; }
    }


    /// <summary>
    /// Determina a estrutura da tabela do banco de dados
    /// que será responsável por armazenar os itens de 
    /// cardápio. Ex: Hamburguer.
    /// </summary>
    public class ItemCardapio
    {

        /// <summary>
        /// Determina o Identificador (chave primária) da tabela.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        /// <summary>
        /// Determina o nome do item.
        /// O atributo [Required] valida este item tanto no formulário
        /// quanto no momento da gravação, não permitindo deixa-lo em branco.
        /// </summary>
        [Required(ErrorMessage = "O Campo Nome é obrigatório")]
        public string Nome { get; set; }


        /// <summary>
        /// Determina a descrição do item.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Determina o preço do item.
        /// </summary>
        [Required(ErrorMessage = "O Campo preço é obrigatório")]
        public double Preco { get; set; }


        /// <summary>
        /// Determina o caminho da imagem do item.
        /// </summary>
        [Required(ErrorMessage = "A Imagem é obrigatória")]
        public string Imagem { get; set; }
    }




    /// <summary>
    /// Esta classe faz a assiciação de um item do cardápio à sua quantidade
    /// </summary>
    public class PedidoItem
    {
        /// <summary>
        /// Id da associação
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        /// <summary>
        /// Id do item do cardápio
        /// </summary>
        public Guid ItemId { get; set; }


        /// <summary>
        /// Item do cardápio
        /// </summary>
        public ItemCardapio Item { get; set; }


        /// <summary>
        /// Quantidade do item
        /// </summary>
        public uint Quantidade { get; set; }
        
    }




    /// <summary>
    /// Esta classe determina a estrutura da tabela de pedidos
    /// </summary>
    public class Pedido
    {
        /// <summary>
        /// Id do Pedido
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        /// <summary>
        /// Id do Cliente
        /// </summary>
        [Required]
        public Guid ClientId { get; set; }


        /// <summary>
        /// Objeto que representa o cliente
        /// </summary>
        public IdentityUser Cliente { get; set; }


        /// <summary>
        /// Todos os itens do carrinho ou do pedido do usuário
        /// </summary>
        public ICollection<PedidoItem> Items { get; set; }

        /// <summary>
        /// Valor total do carrinho ou pedido
        /// </summary>
        public double total { get; set; }


        /// <summary>
        /// Data e hora do pedido
        /// </summary>
        public DateTime DataPedido { get; set; }

    }

    


}
