using System;

using TrickleCharge.DingOS.Core;
using TrickleCharge.DingOS.Shell;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity
{
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

    public void Initialize(ITerminal terminal)
    {
        ContextStack = new ShellContextManager(terminal);
        Shell = new CommandShell()
            .WithInteractiveDefaults();

        ShellContext rootContext = new(RootContextName, PromptFormat, Shell);
        ContextStack.PushContext(rootContext);

        terminal.WriteLine($"Welcome to {Core.SystemInfo.VersionString}");
        terminal.WriteLine("Type 'help' for available commands or 'exit' to quit.\n");
    }

    public virtual void Dispose() { }
}
}
