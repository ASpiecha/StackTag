using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StackTag.Data;

namespace StackTagIntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<StackTagWebAppFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly ISender Sender;
        protected readonly DataContext dataContext;

        protected BaseIntegrationTest(StackTagWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();
            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
            dataContext = _scope.ServiceProvider.GetRequiredService<DataContext>();
        }
    }
}
