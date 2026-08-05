using System.Collections.Generic;
using System.CommandLine;

using TrickleCharge.DingOS.Core;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity
{
public abstract class UnityModule : MonoBehaviour, ICommandModule<Command>
{
    /// <inheritdoc />
    public abstract IEnumerable<Command> GetCommands(IShellEnvironment environment);
}
}
