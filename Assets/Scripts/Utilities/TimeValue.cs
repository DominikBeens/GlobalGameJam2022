using UnityEngine;
using System;

[Serializable]
public class TimeValue {

    public enum TimeType { Seconds, Minutes, Hours }

    [SerializeField] private float time;
    [SerializeField] private TimeType timeType;

    public float GetTime(TimeType type) {
        switch (type) {
            default:
            case TimeType.Seconds: return GetSeconds();
            case TimeType.Minutes: return GetMinutes();
            case TimeType.Hours: return GetHours();
        }
    }

    public float GetSeconds() {
        switch (timeType) {
            default:
            case TimeType.Seconds: return time;
            case TimeType.Minutes: return time * 60;
            case TimeType.Hours: return time * 60 * 60;
        }
    }

    public float GetMinutes() {
        switch (timeType) {
            default:
            case TimeType.Seconds: return time / 60;
            case TimeType.Minutes: return time;
            case TimeType.Hours: return time * 60;
        }
    }

    public float GetHours() {
        switch (timeType) {
            default:
            case TimeType.Seconds: return time / 60 / 60;
            case TimeType.Minutes: return time / 60;
            case TimeType.Hours: return time;
        }
    }

    public string GetShortString() {
        switch (timeType) {
            default:
            case TimeType.Seconds: return $"{GetSeconds()}s";
            case TimeType.Minutes: return $"{GetMinutes()}m";
            case TimeType.Hours: return $"{GetHours()}h";
        }
    }

    public string GetTimeStringSuffix() {
        switch (timeType) {
            default:
            case TimeType.Seconds: return "s";
            case TimeType.Minutes: return "m";
            case TimeType.Hours: return "h";
        }
    }
}
