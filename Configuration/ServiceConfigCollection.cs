using System.Collections.Generic;

namespace FractalSolutions.Api.Configuration
{
    public class ServiceConfigCollection : Dictionary<string, ServiceConfig>, IServiceConfigCollection
    {
    }
}
