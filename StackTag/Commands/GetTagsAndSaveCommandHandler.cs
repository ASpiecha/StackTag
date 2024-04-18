using MediatR;
using Newtonsoft.Json;
using StackTag.Data;
using StackTag.Entities;

namespace StackTag.Commands
{
    public class GetTagsAndSaveCommandHandler : IRequestHandler<GetTagsAndSaveCommand, string>
    {
        private readonly IStackOverflowAPI _stackOverflowAPI;
        private readonly IDataContext _dataContext;
        private readonly ILogger<GetTagsAndSaveCommandHandler> _logger;
        private readonly ITagAnalyzer _tagAnalyzer;

        private const string query = "page={number}&order=desc&sort=popular&site=stackoverflow";

        public GetTagsAndSaveCommandHandler(IStackOverflowAPI stackOverflowAPI,
            IDataContext dataContext,
            ILogger<GetTagsAndSaveCommandHandler> logger,
            ITagAnalyzer tagAnalyzer)
        {
            _stackOverflowAPI = stackOverflowAPI;
            _dataContext = dataContext;
            _logger = logger;
            _tagAnalyzer = tagAnalyzer;
        }

        public async Task<string> Handle(GetTagsAndSaveCommand request, CancellationToken cancellationToken)
        {
            int collected = 0;
            int number = 1;
            while (collected < 1000)
            {
                string queryAdj = query.Replace("{number}", number.ToString());
                var x = await _stackOverflowAPI.GetTagsAsync(queryAdj);
                RootResponse? root = JsonConvert.DeserializeObject<RootResponse>(x);
                if (root?.Items != null)
                {
                    foreach (var tag in root.Items)
                    {
                        _logger.LogTrace($"added tag: {tag.Name}");
                        _dataContext.Tags.Add(tag);
                        collected++;
                    }
                }
                else { break; }
                await _dataContext.SaveChangesAsync(cancellationToken);
                number++;
            }

            await _tagAnalyzer.AnalyzeTags();
            return $"{collected} tags collected";
        }
    }
}
