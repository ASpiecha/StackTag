using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using StackTag;
using StackTag.Commands;
using StackTag.Data;
using StackTag.Entities;

namespace StackTagUnitTests
{
    public class GetTagsAsyncTests
    {
        private readonly Mock<IStackOverflowAPI> _stackOverflowMock = new Mock<IStackOverflowAPI>();
        private readonly Mock<IDataContext> _dataContext = new Mock<IDataContext>();
        private Mock<ILogger<GetTagsAndSaveCommandHandler>> _logger = new Mock<ILogger<GetTagsAndSaveCommandHandler>>();
        private Mock<ITagAnalyzer> _tagAnalyzer = new Mock<ITagAnalyzer>();

        public GetTagsAsyncTests()
        {
        }

        [Fact]
        public async Task Test_GetTagsAndSaveCommandHandler_CollectsTags()
        {
            var cancellationToken = new CancellationToken();
            var request = new GetTagsAndSaveCommand();

            _stackOverflowMock
                .Setup(x => x.GetTagsAsync(It.IsAny<string>()))
                .ReturnsAsync("{\"items\":[{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":2529708,\"name\":\"javascript\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":2193874,\"name\":\"python\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":1917855,\"name\":\"java\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":1615664,\"name\":\"c#\"},{\"collectives\":[{\"tags\":[\"php\"],\"external_links\":[{\"type\":\"support\",\"link\":\"https://stackoverflow.com/contact?topic=15\"}],\"description\":\"A collective where developers working with PHP can learn and connect about the open source scripting language.\",\"link\":\"/collectives/php\",\"name\":\"PHP\",\"slug\":\"php\"}],\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":1464770,\"name\":\"php\"},{\"collectives\":[{\"tags\":[\"android\",\"ios\"],\"external_links\":[{\"type\":\"support\",\"link\":\"https://stackoverflow.com/contact?topic=15\"}],\"description\":\"A collective for developers who want to share their knowledge and learn more about mobile development practices and platforms\",\"link\":\"/collectives/mobile-dev\",\"name\":\"Mobile Development\",\"slug\":\"mobile-dev\"}],\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":1417531,\"name\":\"android\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":1187675,\"name\":\"html\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":1034817,\"name\":\"jquery\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":806954,\"name\":\"c++\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":804562,\"name\":\"css\"},{\"collectives\":[{\"tags\":[\"android\",\"ios\"],\"external_links\":[{\"type\":\"support\",\"link\":\"https://stackoverflow.com/contact?topic=15\"}],\"description\":\"A collective for developers who want to share their knowledge and learn more about mobile development practices and platforms\",\"link\":\"/collectives/mobile-dev\",\"name\":\"Mobile Development\",\"slug\":\"mobile-dev\"}],\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":687421,\"name\":\"ios\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":671046,\"name\":\"sql\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":662077,\"name\":\"mysql\"},{\"collectives\":[{\"tags\":[\"tidyverse\",\"r\",\"ggplot2\",\"stringr\",\"rstudio\",\"tidyr\",\"shiny\",\"quantmod\",\"dplyr\",\"purrr\",\"shinyapps\",\"shinydashboard\",\"readr\",\"dtplyr\",\"tibble\",\"r-raster\",\"shiny-server\",\"knitr\",\"r-caret\",\"plyr\",\"forcats\",\"data.table\",\"rvest\",\"zoo\",\"rlang\",\"lubridate\",\"r-package\"],\"external_links\":[{\"type\":\"support\",\"link\":\"https://stackoverflow.com/contact?topic=15\"}],\"description\":\"A collective where data scientists and AI researchers gather to find, share, and learn about R and other subtags like knitr and dplyr.\",\"link\":\"/collectives/r-language\",\"name\":\"R Language\",\"slug\":\"r-language\"}],\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":505957,\"name\":\"r\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":477211,\"name\":\"reactjs\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":472278,\"name\":\"node.js\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":416798,\"name\":\"arrays\"},{\"has_synonyms\":false,\"is_moderator_only\":false,\"is_required\":false,\"count\":404167,\"name\":\"c\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":374638,\"name\":\"asp.net\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":360448,\"name\":\"json\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":343781,\"name\":\"python-3.x\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":338081,\"name\":\"ruby-on-rails\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":338049,\"name\":\".net\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":334674,\"name\":\"sql-server\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":333569,\"name\":\"swift\"},{\"has_synonyms\":false,\"is_moderator_only\":false,\"is_required\":false,\"count\":311923,\"name\":\"django\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":304347,\"name\":\"angular\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":292328,\"name\":\"objective-c\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":286826,\"name\":\"pandas\"},{\"has_synonyms\":true,\"is_moderator_only\":false,\"is_required\":false,\"count\":286660,\"name\":\"excel\"}],\"has_more\":true,\"quota_max\":300,\"quota_remaining\":298}");

            var dbSetMock = new Mock<DbSet<Tag>>();
            _dataContext.Setup(x => x.Tags).Returns(dbSetMock.Object);

            var handler = new GetTagsAndSaveCommandHandler(_stackOverflowMock.Object, _dataContext.Object, _logger.Object, _tagAnalyzer.Object);
            var result = await handler.Handle(request, cancellationToken);
            Assert.NotNull(result);
            Assert.Equal("1020 tags collected", result);
        }
        [Fact]
        public async Task Test_GetTagsAndSaveCommandHandler_NoTags()
        {
            var cancellationToken = new CancellationToken();
            var request = new GetTagsAndSaveCommand();

            _stackOverflowMock
                .Setup(x => x.GetTagsAsync(It.IsAny<string>()))
                .ReturnsAsync("{}");

            var dbSetMock = new Mock<DbSet<Tag>>();
            _dataContext.Setup(x => x.Tags).Returns(dbSetMock.Object);

            var handler = new GetTagsAndSaveCommandHandler(_stackOverflowMock.Object, _dataContext.Object, _logger.Object, _tagAnalyzer.Object);
            var result = await handler.Handle(request, cancellationToken);
            Assert.NotNull(result);
            Assert.Equal("0 tags collected", result);
        }
    }
}
