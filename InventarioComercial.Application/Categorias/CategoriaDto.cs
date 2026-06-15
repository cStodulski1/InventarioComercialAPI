using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias
{
    public record CategoriaDto(
        Guid Id,
        string Nome,
        string Descricao,
        int QuantidadeDeProdutos
    );
}
