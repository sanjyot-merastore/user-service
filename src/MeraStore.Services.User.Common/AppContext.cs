using MeraStore.Shared.Kernel.Context;

namespace MeraStore.Services.User.Common;

/// <inheritdoc />
public class AppContext(string serviceName) : AppContextBase(serviceName)
{
  public string UserName { get; set; }
  public static new AppContext Current => (AppContext)AppContextScope.Current;
}