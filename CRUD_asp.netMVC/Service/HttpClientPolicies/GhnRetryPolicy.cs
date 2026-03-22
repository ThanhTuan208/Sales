using Polly;
using Polly.Extensions.Http;

namespace CRUD_asp.netMVC.Service.HttpClientPolicies
{
    public static class GhnRetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Retry 3 lần nếu lỗi tạm thời hoặc response không thành công
            return HttpPolicyExtensions
                .HandleTransientHttpError() // 5xx hoặc HttpRequestException
                .OrResult(msg => !msg.IsSuccessStatusCode) // response không thành công
                .WaitAndRetryAsync(
                    3, // retry 3 lần
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // exponential backoff
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s due to {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                    });
        }
    }
}
