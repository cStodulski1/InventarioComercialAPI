using InventarioComercial.Application.Categorias;
using InventarioComercial.Application.Common.Models;
using InventarioComercial.Domain.Models.Produtos;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.PostProduto
{
    public class PostProdutoHandler(ApplicationDbContext dbContext) : IRequestHandler<PostProdutoRequest, ResultData<ProdutoDto>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        public async ValueTask<ResultData<ProdutoDto>> Handle(PostProdutoRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Nome))
            {
                var badRequest = ResultData<ProdutoDto>.Error("Nome é obrigatório");
                return badRequest;
            }

            if (request.Nome.Length < 5)
            {
                var badRequest = ResultData<ProdutoDto>.Error("O campo nome precisa possuir no mínimo 5 caracteres");
                return badRequest;
            }

            var produtoComNomeJaExistente = await _dbContext.Produtos.AnyAsync(p => p.Nome == request.Nome, cancellationToken);
            if(produtoComNomeJaExistente)
            {
                var badRequest = ResultData<ProdutoDto>.Error("Já existe um produto com o nome digitado");
                return badRequest;
            }

            var categoria = await _dbContext.Categorias.FirstOrDefaultAsync(c => c.Id == request.CategoriaId, cancellationToken);
            if(categoria == null)
            {
                var badRequest = ResultData<ProdutoDto>.Error("Categoria selecionada não encontrada.");
                return badRequest;
            }

            var produto = new Produto(categoria.Id, request.Nome, request.Descricao, request.Preco);
            _dbContext.Produtos.Add(produto);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var produtoDto = new ProdutoDto(produto.Id, produto.Nome, produto.Descricao, produto.Preco, categoria.Id, categoria.Nome);

            return ResultData<ProdutoDto>.Success(produtoDto);
        }
    }
}
