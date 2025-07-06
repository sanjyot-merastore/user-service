using MeraStore.Services.User.Common.Logging;
using MeraStore.Shared.Kernel.Context;
using MeraStore.Shared.Kernel.Logging.Interfaces;
using MeraStore.Shared.Kernel.Logging.Loggers;

namespace MeraStore.Services.User.UnitTests.Logging;

public class LogHelperTests
{


  [Fact]
  public void GetApiLog_MessageOnly_ShouldSetMessage()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetApiLog("Test message");
      Assert.Equal("Test message", log.Message);
    }
  }

  [Fact]
  public void GetApiLog_WithCategory_ShouldSetCategory()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetApiLog("Test message", "TestCategory");
      Assert.Equal("TestCategory", log.Category);
    }
  }

  [Fact]
  public void GetApiLog_WithExtraInfo_ShouldSetLogFields()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var extra = new Dictionary<string, string> { { "key", "value" } };
      var log = LogHelper.GetApiLog("Test message", extra);
      Assert.True(log.LoggingFields.ContainsKey("key"));
      Assert.Equal("value", log.LoggingFields["key"]);
    }
  }
  [Fact]
  public void GetApiLog_WithNewExtraInfo_ShouldSetLogFields()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var extra = new Dictionary<string, string> { { "key", "value" } };
      var log = LogHelper.GetApiLog("Test message", "category", extra);
      Assert.True(log.LoggingFields.ContainsKey("key"));
      Assert.Equal("value", log.LoggingFields["key"]);
    }
  }

  [Fact]
  public void GetApiLog_WithNewExtraInfo_WithFilter_ShouldSetLogFields()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var extra = new Dictionary<string, string> { { "key", "value" } };
      var log = LogHelper.GetApiLog("Test message", "category", extra, []);
      Assert.True(log.LoggingFields.ContainsKey("key"));
      Assert.Equal("value", log.LoggingFields["key"]);
    }
  }

  [Fact]
  public void GetExceptionLog_WithExceptionOnly_ShouldSetExceptionFields()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var ex = new InvalidOperationException("boom");
      var log = LogHelper.GetExceptionLog(ex);
      Assert.Contains("boom", log.ExceptionMessage);
      Assert.Equal("System.InvalidOperationException", log.ExceptionType);
    }
  }

  [Fact]
  public void GetExceptionLog_WithExceptionAndMessage_ShouldUseCustomMessage()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var ex = new Exception("fail");
      var log = LogHelper.GetExceptionLog(ex, "Custom message");
      Assert.Equal("Custom message", log.Message);
    }
  }

  [Fact]
  public void GetExceptionLog_WithCategoryAndMessage_ShouldSetAllFields()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var ex = new Exception("fail");
      var log = LogHelper.GetExceptionLog(ex, "MyCategory", "MyMessage");
      Assert.Equal("MyCategory", log.Category);
      Assert.Equal("MyMessage", log.Message);
    }
  }

  [Fact]
  public void GetExceptionLog_WithExtraFields_ShouldPopulateFields()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var ex = new Exception("ex");
      var extra = new Dictionary<string, string> { { "code", "500" } };
      var log = LogHelper.GetExceptionLog(ex, "cat", "msg", extra);
      Assert.True(log.LoggingFields.ContainsKey("code"));
    }
  }

  [Fact]
  public void GetExceptionLog_MessageOnly_ShouldWorkWithoutException()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetExceptionLog("Only message");
      Assert.Equal("Only message", log.Message);
    }
  }

  [Fact]
  public void GetTraceLog_WithMessageOnly_ShouldSetMessage()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetTraceLog("Trace here");
      Assert.Equal("Trace here", log.Message);
    }
  }

  [Fact]
  public void GetTraceLog_WithExtraFields_ShouldAddThem()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetTraceLog("Trace msg", "cat", new Dictionary<string, string> { { "t", "1" } });
      Assert.True(log.LoggingFields.ContainsKey("t"));
    }
  }

  [Fact]
  public void GetWarningLog_WithJustMessage_ShouldCopyToWarningMessage()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetWarningLog("Warning here");
      Assert.Equal("Warning here", log.WarningMessage);
    }
  }

  [Fact]
  public void GetWarningLog_WithCategory_ShouldSetCategory()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetWarningLog("Warning", "WarnCat");
      Assert.Equal("WarnCat", log.Category);
    }
  }

  [Fact]
  public void GetWarningLog_WithAdditionalData_ShouldSetData()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var data = new Dictionary<string, object> { { "foo", 42 } };
      var log = LogHelper.GetWarningLog("Warning", "Cat", data);
      Assert.Equal(42, log.AdditionalData["foo"]);
    }
  }

  [Fact]
  public void GetWarningLog_SeparateMessages_ShouldAssignBoth()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var log = LogHelper.GetWarningLog("Msg", "Real warn", "Cat", null);
      Assert.Equal("Real warn", log.WarningMessage);
      Assert.Equal("Msg", log.Message);
    }
  }

  [Fact]
  public void TraceLog_WithTimings_ExtensionMethod_ShouldSetOnlyProvided()
  {
    using (AppContextScope.BeginScope(new Common.AppContext("service")))
    {
      var trace = new TraceLog("Timing test").WithTimings(dbMs: 100);
      Assert.Equal(100, trace.DbQueryTimeMs);
      Assert.Equal(0, trace.ExternalApiTimeMs);
      Assert.Equal(0, trace.CacheLookupTimeMs);
    }
  }
}