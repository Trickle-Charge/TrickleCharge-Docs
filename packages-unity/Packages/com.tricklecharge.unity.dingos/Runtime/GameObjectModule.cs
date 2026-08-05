using System.Collections.Generic;
using System.CommandLine;

using TrickleCharge.DingOS.Core;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity
{
public class GameObjectModule : UnityModule
{
    [SerializeField]
    private GameObject m_gameObject;

    /// <inheritdoc />
    public override IEnumerable<Command> GetCommands(IShellEnvironment environment)
    {
        yield return SetActive();
    }

    private Command SetActive()
    {
        Argument<bool> activeStateArg = new("bool");

        Command setActiveCommand = new("active", "Sets the active state of the game object.") { activeStateArg };

        setActiveCommand.SetAction(parseResult => m_gameObject.SetActive(parseResult.GetValue(activeStateArg)));

        return setActiveCommand;
    }
}
}
