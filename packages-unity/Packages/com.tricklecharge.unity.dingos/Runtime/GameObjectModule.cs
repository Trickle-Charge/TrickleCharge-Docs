using System.Collections.Generic;
using System.CommandLine;

using TrickleCharge.DingOS.Core;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity
{
public class GameObjectModule : UnityModule
{
    [Tooltip("GameObject the 'active' command toggles. Defaults to this GameObject when left empty.")]
    [SerializeField]
    private GameObject m_gameObject;

    private GameObject Target => m_gameObject != null ? m_gameObject : gameObject;

    /// <inheritdoc />
    public override IEnumerable<Command> GetCommands(IShellEnvironment environment)
    {
        yield return SetActive();
    }

    private Command SetActive()
    {
        Argument<bool> activeStateArg = new("bool");

        Command setActiveCommand = new("active", "Sets the active state of the game object.") { activeStateArg };

        setActiveCommand.SetAction(parseResult => Target.SetActive(parseResult.GetValue(activeStateArg)));

        return setActiveCommand;
    }
}
}
