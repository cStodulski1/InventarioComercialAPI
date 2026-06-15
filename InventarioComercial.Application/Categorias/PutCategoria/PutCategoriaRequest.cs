using InventarioComercial.Application.Common.Models;
using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias.PutCategoria
{
    public record PutCategoriaRequest() : IRequest<ResultData<CategoriaDto>>
    {
        public Guid CategoriaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
