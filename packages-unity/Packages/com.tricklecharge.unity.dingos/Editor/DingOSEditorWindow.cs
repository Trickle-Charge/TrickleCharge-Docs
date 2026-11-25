using System;
using System.Threading;
using System.Threading.Tasks;

using TrickleCharge.DingOS.Shell;
using TrickleCharge.DingOS.Terminal;
using TrickleCharge.DingOS.Unity.Editor.Views;
using TrickleCharge.DingOS.Unity.Modules;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using Debug = UnityEngine.Debug;

namespace TrickleCharge.DingOS.Unity.Editor
{
    public class DingOSEditorWindow : EditorWindow
    {
        private UIToolkitTerminalView _terminalView;
        private UnityTerminal _terminal;
        private ShellContextManager _contextStack;
        private CommandShell _shell;
        private TerminalHost _terminalHost;

        private TextField _inputField;
        private Label _outputLabel;
        private CancellationTokenSource _cancellationTokenSource;

        [MenuItem("Tools/DingOS Console")]
        public static void OpenWindow()
        {
            GetWindow<DingOSEditorWindow>("DingOS Console");
        }

        public void CreateGUI()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            VisualElement root = rootVisualElement;

            // 1. Load UXML and USS from package path
            VisualTreeAsset uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/com.tricklecharge.unity.dingos/Editor/Views/Terminal.uxml"
            );
            StyleSheet uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Packages/com.tricklecharge.unity.dingos/Editor/Views/Terminal.uss"
            );

            if (uxml != null)
            {
                uxml.CloneTree(root);
            }
            else
            {
                Debug.LogError("[DingOS] Could not load Terminal.uxml asset.");
                return;
            }

            if (uss != null)
            {
                root.styleSheets.Add(uss);
            }

            // 2. Query elements from cloned UXML tree
            _inputField = root.Q<TextField>("terminal-input");
            _outputLabel = root.Q<Label>("terminal-output");
            // 3. Initialize Terminal View & Adapter
            _terminalView = new UIToolkitTerminalView(root);
            _terminal = new UnityTerminal(_terminalView);

            // 4. Setup Shell Context & Context Stack
            _contextStack = new ShellContextManager(_terminal);
            _shell = new CommandShell().WithInteractiveDefaults();
            _shell.RegisterModule(new LoggingModule());
            _shell.RegisterModule(new TimeScaleModule());

            ShellContext rootContext = new("Editor", "DingOS> ", _shell);
            _contextStack.PushContext(rootContext);

            // 5. Initialize TerminalHost
            _terminalHost = new TerminalHost(_terminal, _contextStack);

            // 6. Sync active prompt & bind input handler
            // UpdateInputPrompt();
            _inputField?.RegisterCallback<KeyDownEvent>(OnInputKeyDown, TrickleDown.TrickleDown);
            //root?.RegisterCallback<ClickEvent>(_ => _inputField?.Focus());
            //_outputLabel?.RegisterCallback <KeyDownEvent>(_ => _inputField?.Focus());
        }

        private void OnDisable()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _terminalHost?.Dispose();
        }

        private void OnInputKeyDown(KeyDownEvent evt)
        {
            if(evt.keyCode is not (KeyCode.Return or KeyCode.KeypadEnter)) { return; }

            evt.StopPropagation();

            string input = _inputField.value;
            if(string.IsNullOrWhiteSpace(input)) { return; }

            // Clear input box
            _inputField.value = string.Empty;

            // Echo input line with the active prompt before execution
            string prompt = _terminalHost.ContextStack.ActivePrompt;
            _terminal.WriteLine($"{prompt}{input}");

            _inputField.schedule.Execute(() => _inputField.Focus());

            _ = ExecuteCommandAsync(input);

            //Debug.Log(_outputLabel.text);
        }

        private async Task ExecuteCommandAsync(string input)
        {
            try
            {
                await _terminalHost.ExecuteAsync(input, _cancellationTokenSource?.Token ?? CancellationToken.None);

                // Prompt may change if context changed (e.g., net connect / exit)
                //UpdateInputPrompt();
            }
            catch(OperationCanceledException) { }
            catch(Exception ex) { _terminal.WriteErrorLine($"[Error] {ex.Message}"); }
        }

        private void UpdateInputPrompt()
        {
            if (_inputField != null && _terminalHost != null)
            {
                _inputField.label = _terminalHost.ContextStack.ActivePrompt;
            }
        }
    }
}