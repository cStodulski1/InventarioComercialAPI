using InventarioComercial.Application.Categorias;
using InventarioComercial.Application.Categorias.GetCategoria;
using InventarioComercial.Application.Common.Models;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioComercial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CategoriaDto>>> GetCategorias(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? orderBy = "Nome",
            [FromQuery] bool descending = false)
        {
            var query = new GetCategoriaRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                OrderBy = orderBy,
                Descending = descending
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
