using System.Collections.Generic;

using TrickleCharge.DingOS.Core;

namespace TrickleCharge.DingOS.Unity.Networking
{
public class TerminalDirectory : Dictionary<string, ITerminalHost>, ITerminalDirectory { }
}
