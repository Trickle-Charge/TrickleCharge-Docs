using System;
using System.Collections.Concurrent;
using System.Threading;

using TrickleCharge.DingOS.Core;
using TrickleCharge.DingOS.Unity.Views;

namespace TrickleCharge.DingOS.Unity
{
[Serializable]
public class UnityTerminal : ITerminal
{
    private readonly ConcurrentQueue<string> _outputQueue = new();
    private ITerminalView _activeView;
    private SynchronizationContext _mainThreadContext;

    public UnityTerminal(ITerminalView view = null)
    {
        _activeView = view;
        _mainThreadContext = SynchronizationContext.Current;
    }

    public void SetView(ITerminalView view) => _activeView = view;

    public void Write(string text)
    {
        if (string.IsNullOrEmpty(text)) { return; }

        // Route to main thread context if coming from background thread/async execution
        if (_mainThreadContext != null && SynchronizationContext.Current != _mainThreadContext)
        {
            _mainThreadContext.Post(_ => _activeView?.AppendText(text), null);
        }
        else
        {
            _activeView?.AppendText(text);
        }
    }

    public void WriteLine(string text) => Write(text + "\n");
    public void WriteError(string text) => Write($"<color=#FF5555>{text}</color>");
    public void WriteErrorLine(string text) => WriteLine($"<color=#FF5555>{text}</color>");

    public void Clear() => _activeView?.Clear();

    public string ReadLine() => throw new NotSupportedException("Unity uses event-driven input via TMP_InputField or UI Toolkit TextField.");
}
}