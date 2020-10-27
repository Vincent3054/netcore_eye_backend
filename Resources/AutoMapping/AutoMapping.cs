using AutoMapper;
using Models;
using project.Resources;
using project.Resources.Request;
using project.Resources.Response;

namespace project.Resources
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //<前,後>前要轉成後面的，<來源,欲修改> 
            #region  Members (MemberModel)
            CreateMap<RegisterResources, MemberModel>();//Register
            CreateMap<LoginResources, MemberModel>();//Login
            CreateMap<MemberModel, MembersAllResources>();//GetMembers
            CreateMap<EditResources, MemberModel>();
            #endregion

        }
    }

}
