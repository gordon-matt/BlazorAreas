using BlazorAreas.Data.Entities;
using Extenso.AspNetCore.OData;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace BlazorAreas.Infrastructure
{
    public class ODataRegistrar : IODataRegistrar
    {
        public void Register(ODataOptions options)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Person>("PersonApi");
            options.AddRouteComponents("odata", builder.GetEdmModel());
        }
    }
}