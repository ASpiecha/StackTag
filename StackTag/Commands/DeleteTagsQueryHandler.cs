using MediatR;
using StackTag.Data;

namespace StackTag.Commands
{
    public class DeleteTagsQueryHandler : IRequestHandler<DeleteTagsQuery, string>
    {
        private readonly ILogger<DeleteTagsQueryHandler> _logger;
        private readonly IDataContext _dataContext;

        public DeleteTagsQueryHandler(ILogger<DeleteTagsQueryHandler> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public async Task<string> Handle(DeleteTagsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _dataContext.ExternalLinks.RemoveRange(_dataContext.ExternalLinks);
                _dataContext.Collectives.RemoveRange(_dataContext.Collectives);
                _dataContext.Tags.RemoveRange(_dataContext.Tags);
                await _dataContext.SaveChangesAsync(cancellationToken);
                return "Database cleared";
            }
            catch (Exception ex)
            {
                _logger.LogError("Deleting unsuccessful");
                throw;
            }
        }
    }
}
