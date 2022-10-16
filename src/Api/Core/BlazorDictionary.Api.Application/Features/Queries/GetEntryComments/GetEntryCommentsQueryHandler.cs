using BlazorDictionary.Api.Application.Features.Queries.GetEntryComments;
using BlazorDictionary.Api.Application.Interfaces.Repositories;
using BlazorDictionary.Common.Infrastructure.Extensions;
using BlazorDictionary.Common.Models;
using BlazorDictionary.Common.Models.Page;
using BlazorDictionary.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorDictionary.Api.Application.Features.Queries.GetEntryComments;
public class GetEntryCommentsQueryHandler : IRequestHandler<GetEntryCommentsQuery, PagedViewModel<GetEntryCommentsViewModel>>
{

    private readonly IEntryCommentRepository entryCommentRepository;

    public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
    {
        this.entryCommentRepository = entryCommentRepository;
    }

    public async Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request, CancellationToken cancellationToken)
    {
        var query = entryCommentRepository.AsQueryable();

        query = query.Include(i => i.EntryCommentFavorites)
                     .Include(i => i.CreatedBy)
                     .Include(i => i.EntryCommentVotes)
                     .Where(i => i.EntryId == request.EntryId);

        var list = query.Select(i => new GetEntryCommentsViewModel()
        {
            Id = i.Id,
            Content = i.Content,
            IsFavorited = request.UserId.HasValue && i.EntryCommentFavorites.Any(j => j.CreatedById == request.UserId),
            FavoritedCount = i.EntryCommentFavorites.Count,
            CreatedDate = i.CreateDate,
            CreatedByUserName = i.CreatedBy.UserName,
            VoteType =
                request.UserId.HasValue && i.EntryCommentVotes.Any(j => j.CreatedById == request.UserId)
                ? i.EntryCommentVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                : VoteType.None
        });

        var entries = await list.GetPaged(request.Page, request.PageSize);

        return entries;
    }
}