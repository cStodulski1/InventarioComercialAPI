using InventarioComercial.Application.Common.Models;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.DeleteProduto
{
    public record DeleteProdutoRequest(Guid Id) : IRequest<BaseResult>;
}
