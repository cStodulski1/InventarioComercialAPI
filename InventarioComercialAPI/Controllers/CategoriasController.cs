using InventarioComercial.Application.Categorias;
using InventarioComercial.Application.Categorias.GetCategoria;
using InventarioComercial.Application.Categorias.PostCategoria;
using InventarioComercial.Application.Categorias.PutCategoria;
using InventarioComercial.Application.Common.Models;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace InventarioComercial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<ResultData<PaginatedResponse<CategoriaDto>>>> GetCategorias(
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

        [HttpPost]
        public async Task<ActionResult<BaseResult>> PostCategoria(PostCategoriaRequest command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            string uri = Request.GetDisplayUrl();

            return Created(uri, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResult>> PutCategoria(Guid id, [FromBody] PutCategoriaRequest command)
        {
            command.CategoriaId = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
