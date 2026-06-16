using InventarioComercial.Application.Common.Models;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.GetProduto
{
    public record GetProdutoRequest() : IRequest<ResultData<PaginatedResponse<ProdutoDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "Nome";
        public bool Descending { get; set; } = false;
    }
}
