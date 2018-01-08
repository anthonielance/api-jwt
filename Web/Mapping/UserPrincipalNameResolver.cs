using AutoMapper;
using Web.ApiModels;

namespace Web.Mapping
{
    public class UserPrincipalNameResolver : IValueResolver<object, IUserModel, string>
    {
        public string Resolve(object source, IUserModel destination, string destMember, ResolutionContext context)
        {
            return context.Items[ProfileConstants.UserPrincipalName] as string;
        }
    }
}
