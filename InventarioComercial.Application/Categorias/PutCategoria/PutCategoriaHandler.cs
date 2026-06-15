using InventarioComercial.Application.Common.Models;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias.PutCategoria
{
    public class PutCategoriaHandler(ApplicationDbContext dbContext) : IRequestHandler<PutCategoriaRequest, ResultData<CategoriaDto>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async ValueTask<ResultData<CategoriaDto>> Handle(PutCategoriaRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Nome))
            {
                var badRequest = ResultData<CategoriaDto>.Error("Nome é obrigatório");
                return badRequest;
            }

            if (request.Nome.Length < 5)
            {
                var badRequest = ResultData<CategoriaDto>.Error("O campo nome precisa possuir no mínimo 5 caracteres");
                return badRequest;
            }

            var query = _dbContext.Categorias.Include(c => c.Produtos).AsQueryable();
            var categoria = await query.FirstOrDefaultAsync(c => c.Id == request.CategoriaId, cancellationToken);

            if(categoria == null )
            {
                var badRequest = ResultData<CategoriaDto>.Error($"Categoria com Id: {request.CategoriaId} não encontrada");
                return badRequest;
            }

            int qntProdutos = categoria.Produtos.Count;

            categoria.AtualizarCategoria(request.Nome.Trim(), request.Descricao.Trim());
            var categoriaAtualizada = new CategoriaDto(categoria.Id, categoria.Nome, categoria.Descricao, qntProdutos);

            _dbContext.Categorias.Update(categoria);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultData<CategoriaDto>.Success(categoriaAtualizada);
        }
    }
}
