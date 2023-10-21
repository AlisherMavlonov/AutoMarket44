using AutoMapper;
using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Enum;
using AutoMarket44.Domain.Extensions;
using AutoMarket44.Domain.Mapper;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Car;
using AutoMarket44.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Service.Implementations
{
    public class CarService : ICarService
    {
        private readonly IBaseRepository<Car> carRepository;
        private readonly IMapper mapper;

        public CarService(IBaseRepository<Car> baseRepository, IMapper _mapper)
        {
            carRepository = baseRepository;
            mapper = _mapper;
        }

        public async Task<IBaseResponse<Car>> Create(CarViewModel model)
        {
            try
            {
                //var car = new Car();
                var car = mapper.Map<CarViewModel, Car>(model);
                //var car = new Car()
                //{
                //    Name = model.Name,
                //    Model = model.Model,
                //    Description = model.Description,
                //    DateCreate = model.DateCreate,
                //    Speed = model.Speed,
                //    TypeCar = (TypeCar)Convert.ToInt32(model.TypeCar),
                //    Price = model.Price,
                    
                //};

                await carRepository.Create(car);

                return new BaseResponse<Car>()
                {
                    StatusCode = StatusCode.OK,
                    Description = $"{car.Name} успешно добавлено в БД",
                    Data = car
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Car>()
                {
                    Description = $"[Create]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteCar(long id)
        {
            try
            {
                var car = await carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (car == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = $"Авто с таким Id:{id} нет в БД",
                        StatusCode = StatusCode.CarNotFound,
                        Data = false
                    };
                }

                await carRepository.Delete(id);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = $"{car.Name} успешно удалено из БД!!!"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteCar]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Car>> Edit(CarViewModel model)
        {
            try
            {
                var car = await carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == model.Id);
                if (car == null)
                {
                    return new BaseResponse<Car>()
                    {
                        Description = $"Авто с таким Id:{model.Id} нет в БД",
                        StatusCode = StatusCode.CarNotFound
                    };
                }
                var editCar = mapper.Map<CarViewModel, Car>(model);

                //car.Price = model.Price;
                //car.Model = model.Model;
                //car.Price = model.Price;
                //car.Speed = model.Speed;
                //car.DateCreate = model.DateCreate;

                await carRepository.Update(editCar);

                return new BaseResponse<Car>()
                {
                    Data = car,
                    Description = $"{car.Name} Успешно обнавлено из БД",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Car>()
                {
                    Description = $"[Edit]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<CarViewModel>> GetCar(long Id)
        {
            try
            {
                var car = await carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == Id);
                if (car == null)
                {
                    return new BaseResponse<CarViewModel>()
                    {
                        Description = $"Авто с таким Id:{Id} нет в БД",
                        StatusCode = StatusCode.CarNotFound
                    };
                }
                var data = new CarViewModel()
                {
                    Id = car.Id,
                    DateCreate = car.DateCreate,
                    Description = car.Description,
                    Name = car.Name,
                    Price = car.Price,
                    TypeCar = car.TypeCar.GetDisplayName(),
                    Speed = car.Speed,
                    Model = car.Model,
                    //Image = car.Avatar
                };
                return new BaseResponse<CarViewModel>()
                {
                    Data = data,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CarViewModel>()
                {
                    Description = $"[GetCar]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Dictionary<long, string>>> GetCar(string term)
        {

            try
            {
                var cars = await carRepository.GetAll()
                    .Select(x => new CarViewModel()
                    {
                        Id = x.Id,
                        Speed = x.Speed,
                        Name = x.Name,
                        Description = x.Description,
                        Model = x.Model,
                        DateCreate = x.DateCreate,
                        Price = x.Price,
                        TypeCar = x.TypeCar.GetDisplayName()
                    })
                    .Where(x => EF.Functions.Like(x.Name, $"%{term}%"))
                    .ToDictionaryAsync(x => x.Id, t => t.Name);

                return new BaseResponse<Dictionary<long, string>>()
                {
                    Data = cars,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<long, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<CarViewModel>>> GetCars()
        {
            try
            {

                //var cars = carRepository.GetAll().Select(x => new CarViewModel()
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    Description = x.Description,
                //    Model = x.Model,
                //    DateCreate = x.DateCreate,
                //    Price = x.Price,
                //    Speed = x.Speed,
                //    TypeCar = x.TypeCar.GetDisplayName(),
                //    NameAndModel = x.Name + " " +x.Model,

                //}).ToList();
                var carsDb = carRepository.GetAll().ToList();
                var cars = mapper.Map<List<Car>, List<CarViewModel>>(carsDb);

                if (cars.Count <= 0)
                {
                    return new BaseResponse<List<CarViewModel>>()
                    {
                        Description = $"Найдено 0 элементов",
                        StatusCode = StatusCode.CarNotFound
                    };
                }

                return new BaseResponse<List<CarViewModel>>()
                {
                    Data = cars,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<CarViewModel>>()
                {
                    Description = $"[GetCars]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<Dictionary<int, string>> GetTypes()
        {
            try
            {
                var types = ((TypeCar[])Enum.GetValues(typeof(TypeCar)))
                    .ToDictionary(k => (int)k, t => t.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = types,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
