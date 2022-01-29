using BlazorAreas.Data.Entities;
using BlazorAreas.RadzenHelpers.OData;

namespace BlazorAreas.Services
{
    public class PersonODataService : RadzenODataService<Person>
    {
        public PersonODataService(IHttpContextAccessor httpContextAccessor)
            : base("PersonApi", httpContextAccessor)
        {
        }
    }
}