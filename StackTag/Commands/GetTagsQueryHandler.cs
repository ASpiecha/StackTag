using MediatR;
using Microsoft.EntityFrameworkCore;
using StackTag.Data;
using StackTag.Entities;

namespace StackTag.Commands
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, List<Tag>>
    {
        private readonly ILogger<GetTagsQueryHandler> _logger;
        private readonly IDataContext _dataContext;

        public GetTagsQueryHandler(ILogger<GetTagsQueryHandler> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public async Task<List<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tags = await _dataContext.Tags.ToListAsync();

                switch (request.SortBy.ToLower())
                {
                    case "name":
                        tags = request.SortOrder.ToLower() == "asc" ? tags.OrderBy(t => t.Name).ToList() : tags.OrderByDescending(t => t.Name).ToList();
                        break;
                    case "percentage":
                        tags = request.SortOrder.ToLower() == "asc" ? tags.OrderBy(t => t.Percentage).ToList() : tags.OrderByDescending(t => t.Percentage).ToList();
                        break;
                    default:
                        tags = tags.OrderBy(t => t.Name).ToList();
                        break;
                }

                var paginatedTags = tags.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

                return paginatedTags;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving tags: {ex.Message}");
                throw; 
            }
        }
    }
}
