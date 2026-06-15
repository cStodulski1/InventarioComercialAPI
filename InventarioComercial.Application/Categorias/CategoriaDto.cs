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
    //{
    //    public Guid Id {  get; set; }
    //    public string Nome { get; set; } = string.Empty;
    //    public string Descricao { get; set; } = string.Empty;
    //    public int QuantidadeDeProdutos { get; set; } = 0;
    //}
}
