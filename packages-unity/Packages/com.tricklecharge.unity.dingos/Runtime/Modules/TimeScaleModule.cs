using System.Collections.Generic;
using System.CommandLine;

using TrickleCharge.DingOS.Core;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity.Modules
{
public class TimeScaleModule : ICommandModule<Command>
{
    public IEnumerable<Command> GetCommands(IShellEnvironment environment)
    {
        Command cmd = new("timescale", "Modify Time.timescale")
        {
            TimeScaleSet(environment),
            TimeScaleReset(environment)
        };

        yield return cmd;
    }

    private static Command TimeScaleSet(IShellEnvironment env)
    {
        Argument<float> scaleArg = new("scale") { Description = "Target simulation timescale." };
        Command cmd = new("set", "Set the game simulation speed.") { scaleArg };

        cmd.SetAction(parseResult =>
        {
            float scale = parseResult.GetValue(scaleArg);

            Time.timeScale = scale;

            // Route output safely through the environment writer
            env.Out.WriteLine($"Simulation timescale set to {scale}");
        });

        return cmd;
    }

    private static Command TimeScaleReset(IShellEnvironment env)
    {
        Command cmd = new("reset", "Set the game simulation speed to 1.");

        cmd.SetAction(_ =>
        {
            Time.timeScale = 1;

            env.Out.WriteLine("Simulation timescale reset to 1");
        });

        return cmd;
    }
}
}