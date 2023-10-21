namespace AutoMarket44.Domain.Enum
{
    public enum StatusCode
    {
        UserNotFound = 0,
        UserAlreadyExists = 1,

        CarNotFound = 10,

        OrderNotFound = 20,
        EmptyCart = 30,

        OK = 200,
        InternalServerError = 500
    }
}
