using Microsoft.AspNetCore.Routing.Constraints;

namespace Web.Constraints
{
    public class EmailConstraint : RegexRouteConstraint
    {
        public EmailConstraint() : base(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        {
        }
    }
}
