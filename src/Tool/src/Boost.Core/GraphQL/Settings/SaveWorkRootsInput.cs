using System.Collections.Generic;
using Boost.Settings;

namespace Boost.Core.GraphQL;

public record SaveWorkRootsInput(IEnumerable<WorkRoot> WorkRoots);
