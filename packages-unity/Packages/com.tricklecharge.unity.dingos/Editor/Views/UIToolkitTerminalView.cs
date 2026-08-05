using TrickleCharge.DingOS.Unity.Views;

using UnityEngine.UIElements;

namespace TrickleCharge.DingOS.Unity.Editor.Views
{
public class UIToolkitTerminalView : ITerminalView
{
    private readonly ScrollView _scrollView;
    private readonly Label _outputLabel;

    public UIToolkitTerminalView(VisualElement rootElement, string scrollViewName = "terminal-scroll", string labelName = "terminal-output")
    {
        _scrollView = rootElement.Q<ScrollView>(scrollViewName);
        _outputLabel = rootElement.Q<Label>(labelName);

        _outputLabel?.RegisterCallback<GeometryChangedEvent>(_ => ScrollToBottom());
    }

    public void AppendText(string text)
    {
        if (_outputLabel == null) { return; }

        _outputLabel.text += text.Replace("\r\n", "\n");
    }

    public void Clear()
    {
        if (_outputLabel != null)
        {
            _outputLabel.text = string.Empty;
        }
    }

    public void ScrollToBottom()
    {
        // Schedules execution after UI Toolkit completes its layout pass
        _scrollView?.schedule.Execute(() =>
        {
            _scrollView.scrollOffset = new UnityEngine.Vector2(0, _scrollView.verticalScroller.highValue);
        });
    }
}
}
