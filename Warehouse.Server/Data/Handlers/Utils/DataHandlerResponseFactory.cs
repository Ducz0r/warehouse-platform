namespace Warehouse.Server.Data.Handlers.Utils
{
    public static class DataHandlerResponseFactory<T>
    {
        public static DataHandlerResponse<T> SuccessResponse(T obj)
        {
            return new DataHandlerResponse<T>()
            {
                Object = obj
            };
        }

        public static DataHandlerResponse<T> ErrorResponse(ResponseError error)
        {
            return new DataHandlerResponse<T>()
            {
                Error = error
            };
        }
    }
}
