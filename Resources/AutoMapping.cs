using AutoMapper;
using MyWebsite;
using project.Resources;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<UserResources,UserModel>(); // means you want to map from User to UserDTO
    }
}
