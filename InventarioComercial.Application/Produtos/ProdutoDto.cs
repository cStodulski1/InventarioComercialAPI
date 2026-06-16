using InventarioComercial.Application.Categorias;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos
{
    public record ProdutoDto(
        Guid Id,
        string Nome,
        string Descricao,
        decimal Preco,
        Guid CategoriaId,
        CategoriaDto? CategoriaDto
    );
}
