using StackTag.Data;
using StackTag.Entities;

namespace StackTag
{
    public interface ITagAnalyzer
    {
        Task AnalyzeTags();
        List<double> CalculateTagPercentage(List<Tag> tags);
    }

    public class TagAnalyzer : ITagAnalyzer
    {
        private readonly ILoggerAdapter<TagAnalyzer> _logger ;
        private readonly IDataContext _dataContext;
        public TagAnalyzer(ILoggerAdapter<TagAnalyzer> logger, IDataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public async Task AnalyzeTags()
        {
            try
            {
                List<Tag> tags = _dataContext.GetTags();
                if (tags.Count == 0)
                {
                    _logger.LogInformation("No tags in the database.");
                    return;
                }
                CalculateTagPercentage(tags);
                await _dataContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation($"Found {tags.Count} tags in the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while analyzing tags: {ex.Message}");
            }
        }

        public List<double> CalculateTagPercentage(List<Tag> tags)
        {
            int totalTagCount = tags.Sum(t => t.Count ?? 0);
            var percentages = new List<double>();

            if (totalTagCount == 0)
            {
                _logger.LogInformation("Total tag count is 0.");
                return percentages;
            }
            foreach (var tag in tags)
            {
                double percentage = (double)(tag.Count ?? 0) / totalTagCount * 100;
                tag.Percentage = percentage;
                percentages.Add(percentage);
                _logger.LogInformation($"Tag: {tag.Name}, Percentage: {percentage:F2}%");
            }
            return percentages;
        }
    }
}
