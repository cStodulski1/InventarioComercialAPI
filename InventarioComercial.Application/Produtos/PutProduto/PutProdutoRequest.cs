using InventarioComercial.Application.Common.Models;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.PutProduto
{
    public record PutProdutoRequest() : IRequest<ResultData<ProdutoDto>>
    {
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
