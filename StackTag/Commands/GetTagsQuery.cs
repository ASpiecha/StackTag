using MediatR;
using StackTag.Entities;

namespace StackTag.Commands
{
    public class GetTagsQuery : IRequest<List<Tag>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// Sort by - name or percentage
        /// </summary>
        public string SortBy { get; set; } = "name";
        /// <summary>
        /// Sort order - asc or desc
        /// </summary>
        public string SortOrder { get; set; } = "asc";
    }
}
