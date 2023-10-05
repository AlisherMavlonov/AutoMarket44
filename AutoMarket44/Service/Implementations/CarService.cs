using AutoMarket44.Dal.Interfaces;
using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Enum;
using AutoMarket44.Domain.Extensions;
using AutoMarket44.Domain.Response;
using AutoMarket44.Domain.ViewModels.Car;
using AutoMarket44.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Service.Implementations
{
    public class CarService : ICarService
    {
        private readonly IBaseRepository<Car> carRepository;

        public CarService(IBaseRepository<Car> baseRepository)
        {
            carRepository = baseRepository;
        }

        public async Task<IBaseResponse<Car>> Create(CarViewModel model, byte[] imageData)
        {
            try
            {
                var car = new Car()
                {
                    Name = model.Name,
                    Model = model.Model,
                    Description = model.Description,
                    DateCreate = DateTime.Now,
                    Speed = model.Speed,
                    TypeCar = (TypeCar)Convert.ToInt32(model.TypeCar),
                    Price = model.Price,
                    Avatar = imageData
                };

                await carRepository.Create(car);

                return new BaseResponse<Car>()
                {
                    StatusCode = StatusCode.OK,
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
                        Description = $"Car not faund",
                        StatusCode = StatusCode.CarNotFound,
                        Data = false
                    };
                }

                await carRepository.Delete(car);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK
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

        public async Task<IBaseResponse<Car>> Edit(long Id, CarViewModel model)
        {
            try
            {
                var car = await carRepository.GetAll().FirstOrDefaultAsync(x => x.Id == Id);
                if (car == null)
                {
                    return new BaseResponse<Car>()
                    {
                        Description = $"Car not Found",
                        StatusCode = StatusCode.CarNotFound
                    };
                }
                car.Price = model.Price;
                car.Model = model.Model;
                car.Price = model.Price;
                car.Speed = model.Speed;
                car.DateCreate = DateTime.ParseExact(model.DateCreate, "yyyyMMdd HH:mm", null);

                await carRepository.Update(car);

                return new BaseResponse<Car>()
                {
                    Data = car,
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
                        Description = $"Car not Faund",
                        StatusCode = StatusCode.CarNotFound
                    };
                }
                var data = new CarViewModel()
                {
                    DateCreate = car.DateCreate.ToLongDateString(),
                    Description = car.Description,
                    Name = car.Name,
                    Price = car.Price,
                    TypeCar = car.TypeCar.GetDisplayName(),
                    Speed = car.Speed,
                    Model = car.Model,
                    Image = car.Avatar
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
                        DateCreate = x.DateCreate.ToLongDateString(),
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

        public IBaseResponse<List<Car>> GetCars()
        {
            try
            {
                var cars = carRepository.GetAll().ToList();
                if (!cars.Any())
                {
                    return new BaseResponse<List<Car>>()
                    {
                        Description = $"Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Car>>()
                {
                    Data = cars,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Car>>()
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
