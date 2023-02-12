using BlazorDictionary.Api.Application.Features.Commands.Entry.CreateFav;
using BlazorDictionary.Api.Application.Features.Commands.Entry.DeleteFav;
using BlazorDictionary.Api.Application.Features.Commands.EntryComment.CreateFav;
using BlazorDictionary.Api.Application.Features.Commands.EntryComment.DeleteFav;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDictionary.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : BaseController
    {
        private readonly IMediator mediator;

        public FavoriteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("entry/{entryId}")]
        public async Task<IActionResult> CreateEntryFav(Guid entryId)
        {
            var result = await mediator.Send(new CreateEntryFavCommand(entryId, UserId));

            return Ok(result);
        }

        [HttpPost]
        [Route("entrycomment/{entrycommentId}")]
        public async Task<IActionResult> CreateEntryCommentFav(Guid entrycommentId)
        {
            var result = await mediator.Send(new CreateEntryCommentFavCommand(entrycommentId, UserId.Value));

            return Ok(result);
        }


        [HttpPost]
        [Route("deleteentryfav/{entryId}")]
        public async Task<IActionResult> DeleteEntryFav(Guid entryId)
        {
            var result = await mediator.Send(new DeleteEntryFavCommand(entryId, UserId.Value));

            return Ok(result);
        }

        [HttpPost]
        [Route("deleteentrycommentfav/{entrycommentId}")]
        public async Task<IActionResult> DeleteEntryCommentFav(Guid entrycommentId)
        {
            var result = await mediator.Send(new DeleteEntryCommentFavCommand(entrycommentId, UserId.Value));

            return Ok(result);
        }
    }
}
