using MeraStore.Shared.Kernel.Logging.Helpers;
using MeraStore.Shared.Kernel.Logging.Interfaces;
using MeraStore.Shared.Kernel.Logging.Loggers;

namespace MeraStore.Services.User.Common.Logging;

public static class LogHelper
{
  // -------------------------------------
  // ApiLog Overloads
  // -------------------------------------
  public static ApiLog GetApiLog(string message) =>
      BuildApiLog(message);

  public static ApiLog GetApiLog(string message, string category) =>
      BuildApiLog(message, category);

  public static ApiLog GetApiLog(string message, Dictionary<string, string> extraInfo) =>
      BuildApiLog(message, null, extraInfo);

  public static ApiLog GetApiLog(string message, string category, Dictionary<string, string> extraInfo) =>
      BuildApiLog(message, category, extraInfo);

  public static ApiLog GetApiLog(string message, string category, Dictionary<string, string> extraInfo, IMaskingFilter[] maskingFilters) =>
      BuildApiLog(message, category, extraInfo, maskingFilters);


  // 🟢 Only Exception
  public static ExceptionLog GetExceptionLog(Exception ex) =>
    BuildExceptionLog(ex, null, null, null);

  // 🟢 Exception with custom message
  public static ExceptionLog GetExceptionLog(Exception ex, string message) =>
    BuildExceptionLog(ex, null, message, null);

  // 🟢 Exception with category + message
  public static ExceptionLog GetExceptionLog(Exception ex, string category, string message) =>
    BuildExceptionLog(ex, category, message, null);

  // 🟢 Exception with category + message + extra fields
  public static ExceptionLog GetExceptionLog(Exception ex, string category, string message, Dictionary<string, string> extraInfo) =>
    BuildExceptionLog(ex, category, message, extraInfo);

  // 🟢 Fallback to old signature
  public static ExceptionLog GetExceptionLog(string message, string category = null, Dictionary<string, string> extraInfo = null) =>
    BuildExceptionLog(null, category, message, extraInfo);

  // -------------------------------------
  // TraceLog Overloads
  // -------------------------------------
  public static TraceLog GetTraceLog(string message, string category = null, Dictionary<string, string> extraInfo = null) =>
      BuildTraceLog(message, category, extraInfo);

  // -------------------------------------
  // WarningLog Overloads
  // -------------------------------------
  // -------------------------------------
  // WarningLog Overloads
  // -------------------------------------

  // 🟢 Just one message (used as both Log.Message and WarningMessage)
  public static WarningLog GetWarningLog(string message) =>
    BuildWarningLog(message, null, null, null);

  // 🟢 One message + category
  public static WarningLog GetWarningLog(string message, string category) =>
    BuildWarningLog(message, category, null, null);

  // 🟢 One message + category + AdditionalData
  public static WarningLog GetWarningLog(string message, string category, Dictionary<string, object> additionalData) =>
    BuildWarningLog(message, category, null, additionalData);

  // 🟢 Separate message and warning message + category + AdditionalData
  public static WarningLog GetWarningLog(string message, string warningMessage, string category = null, Dictionary<string, object> additionalData = null) =>
    BuildWarningLog(message, category, warningMessage, additionalData);


  // -------------------------------------
  // Internal Builders
  // -------------------------------------
  private static ApiLog BuildApiLog(string message, string category = null, Dictionary<string, string> extraInfo = null, IMaskingFilter[] maskingFilters = null)
  {
    var apiLog = new ApiLog(message, category)
    {
      MaskingFilters = maskingFilters?.ToList() ?? [DefaultMaskingFilter.Get()]
    };

    SetExtraFields(apiLog, extraInfo);
    PopulateLogFromContext(apiLog);
    return apiLog;
  }

  private static ExceptionLog BuildExceptionLog(Exception ex, string category, string message, Dictionary<string, string> extraInfo)
  {
    var exceptionLog = new ExceptionLog(message ?? ex?.Message ?? "Unknown exception", category);

    if (ex != null)
    {
      exceptionLog.ExceptionMessage = ex.Message;
      exceptionLog.ExceptionType = ex.GetType().FullName;
      exceptionLog.InnerExceptionMessage = ex.InnerException?.Message;
      exceptionLog.ExceptionDetails = ex.ToString();
    }

    SetExtraFields(exceptionLog, extraInfo);
    PopulateLogFromContext(exceptionLog);
    return exceptionLog;
  }

  private static TraceLog BuildTraceLog(string message, string category = null, Dictionary<string, string> extraInfo = null)
  {
    var log = new TraceLog(message, category);
    SetExtraFields(log, extraInfo);
    PopulateLogFromContext(log);
    return log;
  }

  private static WarningLog BuildWarningLog(string message, string category, string warningMessage, Dictionary<string, object> additionalData)
  {
    var warningLog = new WarningLog(message, category)
    {
      WarningMessage = warningMessage ?? message
    };

    if (additionalData is not null)
    {
      warningLog.AdditionalData = additionalData;
    }

    PopulateLogFromContext(warningLog);
    return warningLog;
  }

  private static void SetExtraFields(BaseLog log, Dictionary<string, string> extraInfo)
  {
    if (extraInfo is null) return;

    foreach (var kvp in extraInfo)
    {
      log.TrySetLogField(kvp.Key, kvp.Value);
    }
  }

  private static void PopulateLogFromContext(BaseLog log)
  {
    var ctx = AppContext.Current;
    if (ctx is null) return;

    log.CorrelationId = ctx.CorrelationId;
    log.ServiceName = ctx.ServiceName;
    log.TransactionId = ctx.TransactionId;
    log.RequestId = ctx.RequestId;
    log.TraceId = ctx.TraceId;
    log.TenantId = ctx.TenantId;
    log.SessionId = ctx.SessionId;
    
  }

  public static TraceLog WithTimings(this TraceLog log, long? dbMs = null, long? apiMs = null, long? cacheMs = null)
  {
    if (dbMs.HasValue)
      log.DbQueryTimeMs = dbMs.Value;

    if (apiMs.HasValue)
      log.ExternalApiTimeMs = apiMs.Value;

    if (cacheMs.HasValue)
      log.CacheLookupTimeMs = cacheMs.Value;

    return log;
  }
}
