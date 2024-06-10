using UnityEngine;

namespace CustomTools.UnityExtensions
{
    public static class StringExtensions
    {
        public static string ToDisplayString(this float orig)
        {
            return orig.ToString("F1");
        }

        public static string WithColor(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }
    }
}