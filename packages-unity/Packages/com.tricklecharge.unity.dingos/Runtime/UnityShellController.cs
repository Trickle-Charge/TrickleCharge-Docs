using System;
using System.Collections.Generic;
using System.CommandLine;

using TrickleCharge.DingOS.Core;
using TrickleCharge.DingOS.Shell;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity
{
/// <summary>
/// Engine-agnostic shell setup shared by runtime terminals (<see cref="TerminalComponent"/>) and editor tooling.
/// Plain serializable class, not a component: host it from a MonoBehaviour or EditorWindow.
/// </summary>
[Serializable]
public class UnityShellController : IDisposable
{
    [SerializeField]
    private string m_rootContextName = "Root";

    [SerializeField]
    private string m_promptFormat = "DingOS>";

    public virtual string RootContextName
    {
        get => m_rootContextName;
        protected set => m_rootContextName = value;
    }

    public virtual string PromptFormat
    {
        get => m_promptFormat;
        protected set => m_promptFormat = value;
    }

    public CommandShell Shell { get; private set; }

    public ShellContextManager ContextStack { get; private set; }

    public bool IsInitialized => Shell != null;

    public void Initialize(ITerminal terminal, IEnumerable<ICommandModule<Command>> modules = null)
    {
        if (terminal == null) { throw new ArgumentNullException(nameof(terminal)); }

        ContextStack = new ShellContextManager(terminal);
        Shell = new CommandShell()
            .WithInteractiveDefaults();

        if (modules != null)
        {
            foreach (ICommandModule<Command> module in modules)
            {
                if (module != null) { Shell.RegisterModule(module); }
            }
        }

        ShellContext rootContext = new(RootContextName, PromptFormat, Shell);
        ContextStack.PushContext(rootContext);

        terminal.WriteLine($"Welcome to {Core.SystemInfo.VersionString}");
        terminal.WriteLine("Type 'help' for available commands or 'exit' to quit.\n");
    }

    public virtual void Dispose() { }
}
}
