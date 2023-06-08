using System;

namespace CarrinhoComprasAPI
{
    public class Item
    {
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal Peso { get; set; }
        public int Quantidade { get; set; }
    }

    public class CarrinhoCompras
    {
        private List<Item> itens;

        public CarrinhoCompras()
        {
            itens = new List<Item>();
        }

        public void AdicionarItem(Item item, int quantidade)
        {
            item.Quantidade = quantidade;
            itens.Add(item);
        }

        public decimal CalcularFrete()
        {
            decimal pesoTotal = itens.Sum(item => item.Peso * item.Quantidade);

            if (pesoTotal <= 2)
            {
                return 0;
            }
            else if (pesoTotal < 10)
            {
                return 2.00m * pesoTotal;
            }
            else if (pesoTotal < 50)
            {
                return 4.00m * pesoTotal;
            }
            else
            {
                return 7.00m * pesoTotal;
            }
        }

        public decimal CalcularDesconto()
        {
            decimal valorTotal = itens.Sum(item => item.Valor * item.Quantidade);
            decimal frete = CalcularFrete();

            decimal desconto = 0;

            if (itens.Count > 5)
            {
                desconto += 10.00m;
            }

            if (itens.GroupBy(item => item.Nome).Any(group => group.Sum(item => item.Quantidade) > 2))
            {
                desconto += frete * 0.05m;
            }

            if (valorTotal > 500.00m)
            {
                desconto += valorTotal * 0.10m;
            }

            if (valorTotal > 1000.00m)
            {
                desconto += valorTotal * 0.20m;
            }

            return desconto;
        }

        public decimal CalcularValorTotal()
        {
            decimal valorTotal = itens.Sum(item => item.Valor * item.Quantidade);
            decimal frete = CalcularFrete();
            decimal desconto = CalcularDesconto();

            return valorTotal + frete - desconto;
        }
    }

    public class CarrinhoComprasService
    {
        private CarrinhoCompras carrinho;

        public CarrinhoComprasService()
        {
            carrinho = new CarrinhoCompras();
        }

        public void AdicionarItem(Item item, int quantidade)
        {
            carrinho.AdicionarItem(item, quantidade);
        }

        public decimal CalcularValorTotal()
        {
            return carrinho.CalcularValorTotal();
        }
    }

    public class CarrinhoComprasController
    {
        private CarrinhoComprasService carrinhoService;

        public CarrinhoComprasController()
        {
            carrinhoService = new CarrinhoComprasService();
        }

        public void AdicionarItem(string nome, decimal valor, decimal peso, int quantidade)
        {
            var item = new Item { Nome = nome, Valor = valor, Peso = peso };
            carrinhoService.AdicionarItem(item, quantidade);
        }

        public decimal CalcularValorTotal()
        {
            return carrinhoService.CalcularValorTotal();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var carrinhoController = new CarrinhoComprasController();


            carrinhoController.AdicionarItem("Item 1", 100.00m, 1.5m, 2);
            carrinhoController.AdicionarItem("Item 2", 50.00m, 0.8m, 3);
            carrinhoController.AdicionarItem("Item 3", 200.00m, 2.2m, 1);

            decimal valorTotal = carrinhoController.CalcularValorTotal();
            Console.WriteLine($"Valor Total: R${valorTotal}");
        }
    }}

