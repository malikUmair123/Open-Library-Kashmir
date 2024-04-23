using AutoMapper;
using Open_Library_Kashmir.Models;
using System.Data.Entity.Infrastructure;

public static class AutoMapperConfig
{
    public static IMapper Initialize()
    {
        var config = new MapperConfiguration(cfg =>
        {
            // Configure mappings here
            cfg.CreateMap<RecipientViewModel, ApplicationUser>()
                  .ReverseMap(); //Making the Mapping Bi-Directional

        });

        return config.CreateMapper();
    }

    //Use like follows

    //ApplicationUser user = mapper.Map<RecipientViewModel, ApplicationUser>(recipientViewModel)

    //map properties
    //var config = new MapperConfiguration(cfg =>
    //{
    //    cfg.CreateMap<MyViewModel, MyModel>()
    //        .ForMember(dest => dest.DatabaseModelProperty, opt => opt.MapFrom(src => src.ViewModelProperty));
    //});
}
