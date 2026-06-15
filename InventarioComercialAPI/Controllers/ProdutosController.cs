using InventarioComercial.Application.Common.Models;
using InventarioComercial.Application.Produtos.GetProduto;
using InventarioComercial.Application.Produtos.PostProduto;
using InventarioComercial.Application.Produtos.PutProduto;
using Mediator;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace InventarioComercial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<BaseResult>> GetProdutos(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? orderBy = "Nome",
            [FromQuery] bool descending = false)
        {
            var query = new GetProdutoRequest
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
        public async Task<ActionResult<BaseResult>> PostProduto(PostProdutoRequest command)
        {
            var result = await _mediator.Send(command);

            if(!result.IsSuccess)
                return BadRequest(result.Message);

            string uri = Request.GetDisplayUrl();

            return Created(uri, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResult>> PutProduto(Guid id, [FromBody]PutProdutoRequest command)
        {
            command.ProdutoId = id;
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
