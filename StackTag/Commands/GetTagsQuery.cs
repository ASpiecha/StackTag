using MediatR;
using StackTag.Entities;

namespace StackTag.Commands
{
    public class GetTagsQuery : IRequest<List<Tag>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "name";
        public string SortOrder { get; set; } = "asc";
    }
}
