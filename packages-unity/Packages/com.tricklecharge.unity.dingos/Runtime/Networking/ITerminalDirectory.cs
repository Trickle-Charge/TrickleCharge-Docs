using System.Collections.Generic;

using TrickleCharge.DingOS.Core;

namespace TrickleCharge.DingOS.Unity.Networking
{
public interface ITerminalDirectory : IDictionary<string, ITerminalHost> { }
}
