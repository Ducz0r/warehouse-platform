namespace Warehouse.Server.Data.Handlers.Utils
{
    public class DataHandlerResponse<T>
    {
        public ResponseError? Error { get; set; }
        public bool IsSuccess { get => Error == null; }
        public bool IsError { get => Error != null; }
        public T Object { get; set; }
    }
}
