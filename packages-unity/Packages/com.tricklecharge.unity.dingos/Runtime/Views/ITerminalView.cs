namespace TrickleCharge.DingOS.Unity.Views
{
public interface ITerminalView
{
    /// <summary>
    /// Appends raw rich text to the terminal output buffer.
    /// </summary>
    void AppendText(string text);

    /// <summary>
    /// Clears the terminal output screen.
    /// </summary>
    void Clear();

    /// <summary>
    /// Forces the scroll container to snap to the bottom.
    /// </summary>
    void ScrollToBottom();
}
}