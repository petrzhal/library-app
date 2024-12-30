using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library.Application.DTOs.Book;
using MediatR;
using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Images;

namespace Library.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{BookId:int}")]
        public async Task<IActionResult> GetById([FromRoute] BookIdRequest request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));

        [HttpGet("{Isbn}")]
        public async Task<IActionResult> GetByIsbn([FromRoute] BookIsbnRequest request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));

        [HttpGet("author")]
        public async Task<IActionResult> GetByAuthor(AuthorBooksRequest request, CancellationToken token)
        {
            return Ok(await _mediator.Send(request, token));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BookListRequest request, CancellationToken token)
        {
            return Ok(await _mediator.Send(request, token));
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(AddBookRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return Created();
        }

        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return NoContent();
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteBookRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return NoContent();
        }

        [Authorize]
        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(BorrowBookRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return NoContent();
        }

        [Authorize]
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(ReturnBookRequest request, CancellationToken token)
        {
            await _mediator.Send(request, token);
            return NoContent();
        }

        [Authorize]
        [HttpGet("borrowed")]
        public async Task<IActionResult> GetBorrowedBooks([FromQuery] GetUsersBorrowedBooksRequest request, CancellationToken token)
        {
            return Ok(await _mediator.Send(request, token));
        }

        [HttpGet("{BookId:int}/image")]
        public async Task<IActionResult> GetImage([FromRoute] GetImageRequest request, CancellationToken token)
        {
            return Ok(await _mediator.Send(request, token));
        }
    }
}

