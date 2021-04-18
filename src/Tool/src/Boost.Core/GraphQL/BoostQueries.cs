using Boost.GraphQL;
using Boost.Infrastructure;
using HotChocolate.Types;

namespace Boost.Core.GraphQL
{
    [ExtendObjectType(Name = RootTypes.Query)]
    public class BoostQueries
    {
        private readonly IBoostApplicationContext _boostApplicationContext;

        public BoostQueries(IBoostApplicationContext boostApplicationContext)
        {
            _boostApplicationContext = boostApplicationContext;
        }

        public BoostApplication GetAppliation()
        {
            var app = new BoostApplication
            {
                WorkingDirectory = _boostApplicationContext.WorkingDirectory.FullName,
                Version = _boostApplicationContext.Version,
                ConfigurationRequired = true,
            };

            return app;
        }
    }
}
