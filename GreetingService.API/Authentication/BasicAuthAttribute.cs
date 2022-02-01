
using Microsoft.AspNetCore.Mvc;

namespace GreetingService.API.Authentication
{
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthAttribute : TypeFilterAttribute
    {
        public BasicAuthAttribute(string realm = "Basic") : base(typeof(BasicAuthFilter))           //This is the important. Connect this Attribute with "BasicAuthFilter"
        {
            Arguments = new object[] { realm };
        }
    }
}