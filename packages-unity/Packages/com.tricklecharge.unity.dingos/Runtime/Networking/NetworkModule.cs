using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics;
using System.Threading.Tasks;

using TrickleCharge.DingOS.Core;

namespace TrickleCharge.DingOS.Unity.Networking
{
public class NetworkModule : ICommandModule<Command>
{
    private readonly IShellContextStack _contextStack;

    private readonly ITerminalDirectory _terminalDirectory;
    public NetworkModule(IShellContextStack contextStack, ITerminalDirectory terminalDirectory)
    {
        _terminalDirectory = terminalDirectory;
        _contextStack = contextStack
                        ?? throw new ArgumentNullException(nameof(contextStack));
    }

    /// <inheritdoc />
    public IEnumerable<Command> GetCommands(IShellEnvironment environment)
    {
        Command netCommand = new("net", "Network tools.");

        netCommand.Subcommands.Add(Connect(environment, _contextStack, _terminalDirectory));
        netCommand.Subcommands.Add(List(environment, _terminalDirectory));
        netCommand.Subcommands.Add(PingAsync(environment));

        yield return netCommand;
    }

    public static Command Connect(
        IShellEnvironment environment,
        IShellContextStack contextStack,
        ITerminalDirectory terminalDirectory)
    {
        Argument<string> hostArgument = new("host")
        {
            Description = "Hostname or IP address."
        };

        Command connectCmd = new("connect", "Spawns a sub-shell session for a host.")
        {
            hostArgument
        };

        connectCmd.SetAction(parseResult =>
        {
            string host = parseResult.GetValue(hostArgument) ?? string.Empty;

            if(terminalDirectory.TryGetValue(host, out ITerminalHost targetHost))
            {
                environment.Out.WriteLine($"Connecting to {host}...");

                IShellContext remoteContext = targetHost.ContextStack.CurrentContext;

                // remoteContext?.Shell.RegisterModule(
                //     new NetworkModule(contextStack, new TerminalDirectory())
                // );

                if(remoteContext != null) { contextStack.PushContext(remoteContext); }

                return;
            }

            environment.Error.WriteLine($"Failed to connect to {host}.");
        });

        return connectCmd;
    }

    public static Command List(IShellEnvironment environment, ITerminalDirectory terminalDirectory)
    {
        Command listCmd = new("list", "List all available devices.");
        listCmd.Aliases.Add("ls");

        listCmd.SetAction(_ =>
        {
            foreach (KeyValuePair<string, ITerminalHost> host in terminalDirectory)
            {
                environment.Out.WriteLine($"- {host.Key}");
            }
        });

        return listCmd;
    }

    public static Command PingAsync(IShellEnvironment environment)
    {
        Argument<string> hostArgument = new("host")
        {
            Description = "Hostname or IP address."
        };

        Command pingCmd = new("ping", "Ping host.") { hostArgument };

        pingCmd.SetAction(async (parseResult, cancellationToken) =>
        {
            string host = parseResult.GetValue(hostArgument) ?? "unknown";
            Stopwatch sw = new();

            for (int i = 1; i <= 5; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                sw.Restart();
                await Task.Delay(100, cancellationToken);
                sw.Stop();

                await environment.Out.WriteLineAsync($"Reply from {host}: bytes=32 time={sw.ElapsedMilliseconds}ms");
            }
        });

        return pingCmd;
    }
}
}
