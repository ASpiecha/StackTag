using Microsoft.AspNetCore.Mvc;
using MediatR;
using StackTag.Commands;

namespace StackTag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ILogger<TagController> _logger;
        private readonly IMediator _mediator;


        public TagController(
            ILogger<TagController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Retrieve tags from Stack Overflow API
        /// </summary>
        /// <returns> Number of collected tags</returns>
        [HttpGet("request")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401, Type = typeof(string))]
        public async Task<IActionResult> Get()
        {
            var jsonString = await _mediator.Send(new GetTagsAndSaveCommand());
            if (string.IsNullOrEmpty(jsonString))
            {
                _logger.LogError("Failed to retrieve tags from Stack Overflow API");
                return Unauthorized("Failed to retrieve tags from Stack Overflow API");
            }
            return Ok(jsonString);
        }
        /// <summary>
        /// Refresh tags by deleting existing tags and fetching new ones from Stack Overflow API
        /// </summary>
        /// <returns></returns>
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTags()
        {
            try
            {
                await _mediator.Send(new DeleteTagsQuery());
                var result = await _mediator.Send(new GetTagsAndSaveCommand());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while refreshing tags: {ex.Message}");
                return Unauthorized("Error occurred while refreshing tags");
            }
        }
        /// <summary>
        /// Get paginated tags
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginatedTags([FromQuery] GetTagsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while retrieving tags: {ex.Message}");
                return Unauthorized("Error occurred while retrieving tags");
            }
        }
    }
}
