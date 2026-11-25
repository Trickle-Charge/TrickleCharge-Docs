using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Threading.Tasks;

using TMPro;

using TrickleCharge.DingOS.Core;
using TrickleCharge.DingOS.Terminal;
using TrickleCharge.DingOS.Unity.Views;

using UnityEngine;

namespace TrickleCharge.DingOS.Unity
{
/// <summary>
/// Runtime (in-game) terminal. Wires a <see cref="CanvasTMPTerminalView"/> and a <see cref="TMP_InputField"/>
/// to a DingOS shell via <see cref="UnityShellController"/> and <see cref="TerminalHost"/>.
/// Runtime counterpart of the editor's DingOS Console window.
/// </summary>
public class TerminalComponent : MonoBehaviour
{
    [SerializeField]
    private UnityShellController m_shellController = new();

    [SerializeField]
    private CanvasTMPTerminalView m_terminalView;

    [SerializeField]
    private TMP_InputField m_inputField;

    [Tooltip("Command modules registered on the root shell.")]
    [SerializeField]
    private UnityModule[] m_commandModules = Array.Empty<UnityModule>();

    private UnityTerminal _terminal;
    private TerminalHost _host;

    public UnityShellController ShellController => m_shellController;
    public ITerminalHost Host => _host;
    public ITerminal Terminal => _terminal;

    private void Awake()
    {
        if (m_terminalView == null)
        {
            Debug.LogError($"[DingOS] {nameof(TerminalComponent)} on '{name}' has no terminal view assigned.", this);
            enabled = false;
            return;
        }

        m_terminalView.Clear(); // drop any placeholder text authored on the output label
        _terminal = new UnityTerminal(m_terminalView);

        List<ICommandModule<Command>> modules = new();
        foreach (UnityModule module in m_commandModules)
        {
            if (module != null) { modules.Add(module); }
        }

        m_shellController.Initialize(_terminal, modules);
        _host = new TerminalHost(_terminal, m_shellController.ContextStack);
    }

    private void OnEnable()
    {
        if (m_inputField != null) { m_inputField.onSubmit.AddListener(OnSubmit); }
    }

    private void OnDisable()
    {
        if (m_inputField != null) { m_inputField.onSubmit.RemoveListener(OnSubmit); }
    }

    private void OnDestroy()
    {
        _host?.Dispose();
        m_shellController?.Dispose();
    }

    /// <summary>Executes a line as if it had been typed into the terminal.</summary>
    public Task<ShellResult> Submit(string input)
    {
        if (_host == null || string.IsNullOrWhiteSpace(input)) { return Task.FromResult(ShellResult.Empty); }

        _terminal.WriteLine($"{_host.ContextStack.ActivePrompt} {input}");
        return ExecuteAsync(input);
    }

    private void OnSubmit(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) { return; }

        m_inputField.text = string.Empty;
        m_inputField.ActivateInputField();

        _ = Submit(input);
    }

    private async Task<ShellResult> ExecuteAsync(string input)
    {
        try
        {
            return await _host.ExecuteAsync(input, destroyCancellationToken);
        }
        catch (OperationCanceledException)
        {
            return ShellResult.Empty;
        }
        catch (Exception ex)
        {
            _terminal.WriteErrorLine($"[Error] {ex.Message}");
            return new ShellResult(1, string.Empty, ex.Message);
        }
    }
}
}
