using UnityEngine;
using System;

public static class TextHelper {

    private const int THOUSAND = 1000;
    private const int TENTHOUSAND = 10000;

    private const int MILLION = 1000000;
    private const int TENMILLION = 10000000;

    public static string GetFormattedNumber(int amount) {
        if (amount >= TENMILLION) { return $"{Mathf.RoundToInt(amount / MILLION)}M"; }
        if (amount >= TENTHOUSAND) { return $"{Mathf.RoundToInt(amount / THOUSAND)}K"; }
        return $"{amount}";
    }

    public static string GetNodeLifeTime(float lifetime) {
        lifetime = Mathf.Round(lifetime);
        int minutes = Mathf.FloorToInt(lifetime / 60);
        int seconds = Mathf.FloorToInt(lifetime - minutes * 60);
        return $"<mspace=0.8em>{minutes:00}</mspace>:<mspace=0.8em>{seconds:00}</mspace>";
    }
}
