using InventarioComercial.Application.Common.Models;
using InventarioComercial.Domain.Models.Produtos;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.GetProduto
{
    public class GetProdutoHandler(ApplicationDbContext dbContext) : IRequestHandler<GetProdutoRequest, ResultData<PaginatedResponse<ProdutoDto>>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        public async ValueTask<ResultData<PaginatedResponse<ProdutoDto>>> Handle(GetProdutoRequest request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Produtos
                .Include(p => p.Categoria)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(p =>
                    p.Nome.Contains(request.SearchTerm) ||
                    (p.Descricao != null && p.Descricao.Contains(request.SearchTerm)) ||
                    (p.Categoria != null && p.Categoria.Nome.Contains(request.SearchTerm)) ||
                    (p.Categoria != null && p.CategoriaId.ToString().Contains(request.SearchTerm)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplyOrdering(query, request.OrderBy ?? "Nome", request.Descending);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProdutoDto(p.Id, p.Nome, p.Descricao, p.Preco, p.CategoriaId, 
                    p.Categoria != null ? p.Categoria.Nome : string.Empty))
                .ToListAsync(cancellationToken);

            var listaDeProdutos = new PaginatedResponse<ProdutoDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return ResultData<PaginatedResponse<ProdutoDto>>.Success(listaDeProdutos);
        }

        private IQueryable<Produto> ApplyOrdering(
            IQueryable<Produto> query,
            string orderBy,
            bool descending)
        {
            return orderBy.ToLower() switch
            {
                "nome" => descending ? query.OrderByDescending(c => c.Nome) : query.OrderBy(c => c.Nome),
                "descricao" => descending ? query.OrderByDescending(c => c.Descricao) : query.OrderBy(c => c.Descricao),
                _ => query.OrderBy(c => c.Nome)
            };
        }
    }
}
