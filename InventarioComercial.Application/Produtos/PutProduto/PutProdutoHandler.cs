using InventarioComercial.Application.Common.Models;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.PutProduto
{
    public class PutProdutoHandler(ApplicationDbContext dbContext) : IRequestHandler<PutProdutoRequest, ResultData<ProdutoDto>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        public async ValueTask<ResultData<ProdutoDto>> Handle(PutProdutoRequest request, CancellationToken cancellationToken)
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

            var query = _dbContext.Produtos.Include(p => p.Categoria).AsQueryable();
            var produto = query.FirstOrDefault(p => p.Id == request.ProdutoId);

            if (produto == null)
            {
                var badRequest = ResultData<ProdutoDto>.Error($"Produto com Id: {request.ProdutoId} não encontrado.");
                return badRequest;
            }

            bool deveAlterarCategoriaId = request.CategoriaId != Guid.Empty;
            Guid categoriaId = deveAlterarCategoriaId ? request.CategoriaId : produto.CategoriaId;

            produto.AtualizarProduto(request.Nome, request.Descricao, request.Preco, categoriaId);
            var produtoAtualizado = new ProdutoDto(
                produto.Id,
                produto.Nome,
                produto.Descricao,
                produto.Preco,
                produto.CategoriaId,
                produto.Categoria != null ? produto.Categoria.Nome : string.Empty);

            _dbContext.Produtos.Update(produto);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultData<ProdutoDto>.Success(produtoAtualizado);
        }
    }
}
