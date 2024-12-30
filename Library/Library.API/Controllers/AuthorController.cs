using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library.Application.DTOs.Authors;
using MediatR;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{AuthorId:int}")]
        public async Task<IActionResult> Get([FromRoute] AuthorIdRequest request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] AuthorListRequest request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddAuthorRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return Created();
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAuthorRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return NoContent();
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteAuthorRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return NoContent();
        }
    }
}
