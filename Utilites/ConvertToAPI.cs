namespace FMSBay.Utilites
{
    public class ConvertToAPI
    {
        public static ApiResponse<T> ConvertResultToApiResonse<T>(T result)
        {
            var ApiResponse = new ApiResponse<T>();
            ApiResponse.Response = result;
            ApiResponse.Succeded = true;
            return ApiResponse;
        }

        public static ApiResponse<T> GetErrorResponse<T>(T Result, List<string> errors)
        {
            var errorObject = ConvertToAPI.ConvertResultToApiResonse(Result);
            errorObject.Errors = errors.ToArray();
            errorObject.Succeded = false;
            return errorObject;
        }
    }
}
