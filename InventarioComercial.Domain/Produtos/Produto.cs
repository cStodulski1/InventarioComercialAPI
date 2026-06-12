using InventarioComercial.Domain.Categorias;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace InventarioComercial.Domain.Produtos
{
    public class Produto(Guid categoriaId, 
        string nome, string descricao, decimal preco)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nome é obrigatório")]
        [MinLength(5, ErrorMessage = "Nome deve ter no mínimo 5 caracteres")]
        [MaxLength(100)]
        public string Nome { get; private set; } = nome;

        [MaxLength(500)]
        public string Descricao { get; private set; } = descricao;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; private set; } = preco;

        [Required]
        public Guid CategoriaId { get; private set; } = categoriaId;

        [JsonIgnore]
        public Categoria? Categoria { get; private set; }
    }
}
