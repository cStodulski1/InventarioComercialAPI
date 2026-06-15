using InventarioComercial.Application.Common.Models;
using InventarioComercial.Application.Produtos.PostProduto;
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


        [HttpPost]
        public async Task<ActionResult<BaseResult>> PostProduto(PostProdutoRequest command)
        {
            var result = await _mediator.Send(command);

            if(!result.IsSuccess)
                return BadRequest(result.Message);

            string uri = Request.GetDisplayUrl();

            return Created(uri, result);
        }
    }
}
