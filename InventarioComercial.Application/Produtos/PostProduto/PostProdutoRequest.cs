using InventarioComercial.Application.Common.Models;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.PostProduto
{
    public record PostProdutoRequest() : IRequest<ResultData<ProdutoDto>>
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
