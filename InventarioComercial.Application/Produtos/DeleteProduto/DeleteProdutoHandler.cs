using InventarioComercial.Application.Common.Models;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Produtos.DeleteProduto
{
    public class DeleteProdutoHandler(ApplicationDbContext dbContext) : IRequestHandler<DeleteProdutoRequest, BaseResult>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async ValueTask<BaseResult> Handle(DeleteProdutoRequest request, CancellationToken cancellationToken)
        {
            var produto = await _dbContext.Produtos
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (produto == null)
            {
                return ResultData<object>.Error($"Categoria com Id: {request.Id} não encontrada.");
            }

            _dbContext.Produtos.Remove(produto);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultData<bool>.Success(true);
        }
    }
}
