using AutoMapper;
using MyWebsite;
using project.Resources;
namespace project.Resources
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //<前,後>前要轉成後面的，<來源,欲修改> 
            CreateMap<UserResources, UserModel>();
            CreateMap<LoginResources, UserModel>(); 

        }
    }

}
