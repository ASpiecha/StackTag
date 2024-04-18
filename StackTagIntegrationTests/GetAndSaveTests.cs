using StackTag.Commands;
using StackTag.Entities;

namespace StackTagIntegrationTests
{
    public class GetAndSaveTests : BaseIntegrationTest
    {
        public GetAndSaveTests(StackTagWebAppFactory factory) : base(factory) { }


        /*************** not finished **************
         * 
         * 
         * [Fact]
        public async Task Test_ShouldGetAndSave_TagsToDatabase()
        {
            var command = new GetTagsAndSaveCommand();
            var response1 = await Sender.Send(command);
            var query = new GetTagsQuery()
            {
                Page = 1,
                PageSize = 1
            };

            var response2 = await Sender.Send(query);

            var product = dataContext.Tags.FirstOrDefault(t => t == new Tag());
            Assert.NotNull(product);*/

        
    }    
}
