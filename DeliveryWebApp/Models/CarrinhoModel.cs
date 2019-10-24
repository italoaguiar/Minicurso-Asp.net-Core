using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Data;

namespace DeliveryWebApp.Models
{
    public class CarrinhoModel
    {
        public Guid Id { get; set; }
        public ItemCardapio Item { get; set; }
        public uint Quantidade { get; set; }
    }
}
