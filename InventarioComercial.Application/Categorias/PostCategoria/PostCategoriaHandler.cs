using InventarioComercial.Application.Common.Models;
using InventarioComercial.Domain.Models.Categorias;
using InventarioComercial.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioComercial.Application.Categorias.PostCategoria
{
    public class PostCategoriaHandler(ApplicationDbContext dbContext) : IRequestHandler<PostCategoriaRequest, ResultData<CategoriaDto>>
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async ValueTask<ResultData<CategoriaDto>> Handle(PostCategoriaRequest request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.Nome))
            {
                var badRequest = ResultData<CategoriaDto>.Error("Nome é obrigatório");
                return badRequest;
            }

            if(request.Nome.Length < 5)
            {
                var badRequest = ResultData<CategoriaDto>.Error("O campo nome precisa possuir no mínimo 5 caracteres");
                return badRequest;
            }

            var categoriaComNomeJaExistente = await _dbContext.Categorias.AnyAsync(c => c.Nome == request.Nome, cancellationToken);

            if(categoriaComNomeJaExistente)
            {
                var badRequest = ResultData<CategoriaDto>.Error("Já existe uma categoria com o nome digitado");
                return badRequest;
            }

            var categoria = new Categoria(request.Descricao, request.Nome);
            _dbContext.Categorias.Add(categoria);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var categoriaCriadaDto = new CategoriaDto(categoria.Id, categoria.Nome, categoria.Descricao, 0);

            var result = ResultData<CategoriaDto>.Success(categoriaCriadaDto);

            return result;
        }
    }
}
