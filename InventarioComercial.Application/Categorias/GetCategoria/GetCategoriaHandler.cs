using InventarioComercial.Application.Common.Models;
using InventarioComercial.Domain.Models.Categorias;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias.GetCategoria
{
    public class GetCategoriaHandler(ApplicationDbContext dbContext) : IRequestHandler<GetCategoriaRequest, PaginatedResponse<CategoriaDto>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        public async ValueTask<PaginatedResponse<CategoriaDto>> Handle(GetCategoriaRequest request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Categorias
                .Include(c => c.Produtos)
                .AsNoTracking()
                .AsQueryable();

            if(!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(c =>
                    c.Nome.Contains(request.SearchTerm) ||
                    (c.Descricao != null && c.Descricao.Contains(request.SearchTerm)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplyOrdering(query, request.OrderBy ?? "Nome", request.Descending);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao,
                    QuantidadeDeProdutos = c.Produtos.Count()
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<CategoriaDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        private IQueryable<Categoria> ApplyOrdering(
            IQueryable<Categoria> query,
            string orderBy,
            bool descending)
        {
            return orderBy.ToLower() switch
            {
                "nome" => descending ? query.OrderByDescending(c => c.Nome) : query.OrderBy(c => c.Nome),
                "descricao" => descending ? query.OrderByDescending(c => c.Descricao) : query.OrderBy(c => c.Descricao),
                "produtosCount" => descending ? query.OrderByDescending(c => c.Produtos.Count) : query.OrderBy(c => c.Produtos.Count),
                _ => query.OrderBy(c => c.Nome)
            };
        }
    }
}
