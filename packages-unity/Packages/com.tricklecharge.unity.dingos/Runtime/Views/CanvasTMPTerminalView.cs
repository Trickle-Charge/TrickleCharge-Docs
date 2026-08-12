using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace TrickleCharge.DingOS.Unity.Views
{
public class CanvasTMPTerminalView : MonoBehaviour, ITerminalView
{
    [SerializeField]
    private TMP_Text m_outputText;

    [SerializeField]
    private ScrollRect m_scrollRect;

    [SerializeField]
    private int m_maxCharacterLimit = 10000;

    private bool _needsScrollUpdate;

    private void LateUpdate()
    {
        if (!_needsScrollUpdate) { return; }

        Canvas.ForceUpdateCanvases();
        if (m_scrollRect != null)
        {
            m_scrollRect.verticalNormalizedPosition = 0f;
        }
        _needsScrollUpdate = false;
    }

    public void AppendText(string text)
    {
        if (m_outputText == null) { return; }

        m_outputText.text += text;

        // Truncate old output if it exceeds character limits
        if (m_outputText.text.Length > m_maxCharacterLimit)
        {
            int trimIndex = m_outputText.text.Length - m_maxCharacterLimit;
            m_outputText.text = m_outputText.text[trimIndex..];
        }

        _needsScrollUpdate = true;
    }

    public void Clear()
    {
        if (m_outputText != null)
        {
            m_outputText.text = string.Empty;
        }
        _needsScrollUpdate = true;
    }

    public void ScrollToBottom() => _needsScrollUpdate = true;
}
}
