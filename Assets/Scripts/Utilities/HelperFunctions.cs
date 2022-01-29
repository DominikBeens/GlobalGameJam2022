using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public static class HelperFunctions {

    public static bool EmptyString(string s) {
        return string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s);
    }

    public static float DirectionToAngleInDegrees(Vector3 direction) {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }

    public static float GetAngleToMouseInWorldSpace(Camera camera, Vector3 origin) {
        Vector2 direction = GetViewportDirectionToMouse(camera, origin);
        return DirectionToAngleInDegrees(direction);
    }

    public static Vector2 GetViewportDirectionToMouse(Camera camera, Vector3 origin) {
        Vector2 viewportOrigin = camera.WorldToViewportPoint(origin);
        Vector2 viewportMouse = camera.ScreenToViewportPoint(Input.mousePosition);
        return viewportMouse - viewportOrigin;
    }

    public static bool IsWithinRange(Vector3 v1, Vector3 v2, float range) {
        return (v1 - v2).sqrMagnitude <= (range * range);
    }

    public static bool IsWithinRange(Vector3 v1, Vector3 v2, float rangeMin, float rangeMax) {
        float sqrMagnitude = (v1 - v2).sqrMagnitude;
        return sqrMagnitude >= (rangeMin * rangeMin) && sqrMagnitude <= (rangeMax * rangeMax);
    }

    public static Vector3 GetRandomOffsetWithinRectTransform(RectTransform rectTransform) {
        float maxHorizontalOffset = rectTransform.sizeDelta.x / 2;
        float maxVerticalOffset = rectTransform.sizeDelta.y / 2;
        float randomX = GetRandom(maxHorizontalOffset);
        float randomY = GetRandom(maxVerticalOffset);

        float GetRandom(float max) {
            return Random.Range(-max, max);
        }

        return new Vector3(randomX, randomY, 0);
    }

    public static string ColorToHtmlString(Color color) {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    public static Color HtmlStringToColor(string html) {
        html = html.StartsWith("#") ? html : $"#{html}";
        ColorUtility.TryParseHtmlString(html, out Color color);
        return color;
    }

    public static string EnumToString(Enum e) {
        return string.Join(" ", Regex.Split(e.ToString(), @"(?<!^)(?=[A-Z])"));
    }

    public static string RemoveHtml(string s) {
        return Regex.Replace(s, @"<(.|\n)*?>", string.Empty);
    }

    public static Quaternion GetRotationWithAddedAngle(Quaternion rotation, float angle) {
        Vector3 direction = rotation.eulerAngles;
        direction.y += angle;
        return Quaternion.Euler(direction);
    }

    public static string GetNumberWithSeparator(int number, string separator, int step = 3) {
        string formatedString = string.Empty;
        char[] chars = number.ToString().ToCharArray();
        int currentStep = 0;
        for (int i = chars.Length - 1; i >= 0; i--) {
            currentStep++;
            formatedString = formatedString.Insert(0, chars[i].ToString());
            if (currentStep % step == 0 && i != 0) {
                formatedString = formatedString.Insert(0, separator);
            }
        }
        return formatedString;
    }

    public static float ValueToDecibel(float value) {
        value = Mathf.Max(0.0001f, value);
        return Mathf.Log10(value) * 20;
    }

    public static Vector3 GetRandomPosition(Vector3 center, Vector2 bounds) {
        return GetRandomPosition(center, new Vector3(bounds.x, 0, bounds.y));
    }

    public static Vector3 GetRandomPosition(Vector3 center, Vector3 bounds) {
        float GetRandom(float center, float width) => Random.Range(center - width / 2, center + width / 2);
        return new Vector3(GetRandom(center.x, bounds.x), GetRandom(center.y, bounds.y), GetRandom(center.z, bounds.z));
    }

    public static string SpaceLowercaseToUppercase(string s) {
        string text = string.Empty;
        bool uppercase = false;
        foreach (char c in s) {
            if (c == ' ') {
                uppercase = true;
                continue;
            }

            char newChar = uppercase ? char.ToUpper(c) : c;
            text = $"{text}{newChar}";
            uppercase = false;
        }
        return text;
    }
}
