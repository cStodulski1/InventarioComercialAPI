using InventarioComercial.Application.Common.Models;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias.DeleteCategoria
{
    public class DeleteCategoriaHandler(ApplicationDbContext dbContext) : IRequestHandler<DeleteCategoriaRequest, BaseResult>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        public async ValueTask<BaseResult> Handle(DeleteCategoriaRequest request, CancellationToken cancellationToken)
        {
            var categoria = await _dbContext.Categorias
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (categoria == null) 
            {
                return ResultData<object>.Error($"Categoria com Id: {request.Id} não encontrada.");
            }

            if (categoria.Produtos != null && categoria.Produtos.Count != 0)
            {
                return ResultData<object>.Error("Não é possível excluir uma categoria que possua produtos vinculados.");
            }

            _dbContext.Categorias.Remove(categoria);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultData<bool>.Success(true);
        }
    }
}
