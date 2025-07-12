using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ecommerce.Server.Extensions
{
    public static class ApiDescriptionExtension
    {
        public static List<string> GetAreaName(this ApiDescription description)
        {
            List<string> areaList = [];

            description.ActionDescriptor.RouteValues.TryGetValue("Area", out string? area);

            if (!string.IsNullOrEmpty(area))
            {
                areaList.Add(area);
            }
            else
            { 
                areaList.Add("Shared"); 
            }

            return areaList;
        }
    }
}
