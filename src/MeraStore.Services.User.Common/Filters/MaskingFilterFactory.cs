using MeraStore.Shared.Kernel.Logging.Filters;
using MeraStore.Shared.Kernel.Logging.Interfaces;

namespace MeraStore.Services.User.Common.Filters;

/// <summary>
/// Factory for creating preconfigured masking filters specific to the Auth Service.
/// Used to mask sensitive fields in request and response payloads for secure logging.
/// </summary>
public static class MaskingFilterFactory
{
  /// <summary>
  /// Creates an API masking filter that applies masking to sensitive fields
  /// like passwords and credit card numbers in both request and response payloads.
  /// </summary>
  /// <returns>An instance of <see cref="IMaskingFilter"/> configured with request and response field masks.</returns>
  public static IMaskingFilter ApiMaskingFilter()
  {
    // Configure request masking filter
    var requestFilter = new JsonPayloadRequestFilter();
    requestFilter.AddField("password");
    requestFilter.AddField("creditCardNumber");
    requestFilter.AddField("ssn");

    // Configure response masking filter
    var responseFilter = new JsonPayloadResponseFilter();
    responseFilter.AddField("password");
    responseFilter.AddField("creditCardNumber");
    responseFilter.AddField("ssn");
    responseFilter.AddField("summary");

    // Create MaskingFilter with both filters
    return new MaskingFilter(
      [requestFilter],
      [responseFilter]
    );
  }
}