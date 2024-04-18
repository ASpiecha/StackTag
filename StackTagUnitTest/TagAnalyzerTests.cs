using StackTag.Data;
using StackTag;
using StackTag.Entities;
using Moq;

namespace StackTagUnitTests
{
    public class TagAnalyzerTests
    {
        private readonly Mock<ILoggerAdapter<TagAnalyzer>> _loggerMock = new Mock<ILoggerAdapter<TagAnalyzer>>();
        private readonly Mock<IDataContext> _dataContextMock = new Mock<IDataContext>();
        public TagAnalyzerTests()
        {

        }
        [Fact]
        public async Task Test_AnalyzeTags_EmptyDatabase()
        {
            var tagAnalyzer = new TagAnalyzer(_loggerMock.Object, _dataContextMock.Object);
            _dataContextMock.Setup(x => x.GetTags()).Returns(new List<Tag>());

            await tagAnalyzer.AnalyzeTags();

            _loggerMock.Verify(
                x => x.LogInformation("No tags in the database."));
        }
        [Fact]
        public async Task Test_AnalyzeTags_2Tags()
        {
            var tagAnalyzer = new TagAnalyzer(_loggerMock.Object, _dataContextMock.Object);
            _dataContextMock.Setup(x => x.GetTags()).Returns([new Tag(), new Tag()]);

            await tagAnalyzer.AnalyzeTags();

            _loggerMock.Verify(
                x => x.LogInformation("Found 2 tags in the database."));
        }
        [Fact]
        public async Task Test_AnalyzeTags_ExceptionThrown_LoggerShouldLogError()
        {
            _dataContextMock.Setup(x => x.GetTags()).Throws(new Exception("Test exception"));
            var tagAnalyzer = new TagAnalyzer(_loggerMock.Object, _dataContextMock.Object);

            await tagAnalyzer.AnalyzeTags();

            _loggerMock.Verify(
                x => x.LogError("Error occurred while analyzing tags: Test exception"),
                Times.Once);
        }
        [Fact]
        public void Test_CalculatePrecentage_3Tags()
        {
            var tagAnalyzer = new TagAnalyzer(_loggerMock.Object, _dataContextMock.Object);
            var expected = new List<double> { 12, 28, 60 };
            var tags = new List<Tag> {
                new() { Count = 12 },
                new() { Count = 28 },
                new() { Count = 60 }
            };
            var percentages = tagAnalyzer.CalculateTagPercentage(tags);

            Assert.All(expected.Zip(percentages, Tuple.Create), tuple => Assert.Equal(tuple.Item1, tuple.Item2, 6));
        }
    }
}
