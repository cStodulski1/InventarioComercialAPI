using InventarioComercial.Application.Common.Models;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias.PostCategoria
{
    public record PostCategoriaRequest(string Nome, string Descricao) : IRequest<ResultData<CategoriaDto>>;
}
