using InventarioComercial.Application.Common.Models;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias.DeleteCategoria
{
    public record DeleteCategoriaRequest(Guid Id) : IRequest<BaseResult>;
}
