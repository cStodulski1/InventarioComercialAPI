using InventarioComercial.Domain.Produtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventarioComercial.Domain.Categorias
{
    [Table("Categorias")]
    public class Categoria(string descricao, string nome)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MinLength(5, ErrorMessage = "Nome deve ter no mínimo 5 caracteres")]
        [MaxLength(100)]
        public string Nome { get; private set; } = nome;

        [MaxLength(500)]
        public string Descricao { get; private set; } = descricao;
        public List<Produto> Produtos { get; private set; } = [];
    }
}