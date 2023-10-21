using AutoMapper;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Enum;
using AutoMarket44.Domain.Extensions;
using AutoMarket44.Domain.ViewModels.Car;
using Microsoft.OpenApi.Extensions;

namespace AutoMarket44.Domain.Mapper
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Car, CarViewModel>()
                .ForMember(dst => dst.NameAndModel,
             opt => opt.MapFrom(src => src.Model + " " + src.Name))
                .ForMember(dst => dst.TypeCar,
                opt => opt.MapFrom(src => AutoMarket44.Domain.Extensions.EnumExtension.GetDisplayName(src.TypeCar)));

            this.CreateMap<CarViewModel, Car>()
                .ForMember(dst => dst.TypeCar,
                opt => opt.MapFrom(src => src.TypeCar));
                
        }
    }
}
