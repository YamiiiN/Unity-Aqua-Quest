using System;
using System.IO;
using UnityEngine;
using TMPro;

[Serializable]
public class PlayerStats
{
    public RelicStats Relics = new RelicStats(); // Holds individual items
    public PlayerAttributes PlayerAttributes;

    public Kills Kills = new Kills();
    
      
}

[Serializable]
public class Kills
{
    public Monster KanalGoblin;
    public Monster ElNiño;
    public Monster PinsalangKinamada;
    public int OverallKills; 
}

[Serializable]
public class Monster
{
    public int TotalKills;
}

[Serializable]
public class RelicStats
{
    public StatData Health;
    public StatData Damage;
    public StatData Defense;
    public SpeedStatData Speed;
}

[Serializable]
public class PlayerAttributes
{
    public int TotalHealth;
    public int TotalDamage;
    public int TotalDefense;
    public float TotalSpeed;
    public float CalculatedPowerLevel;
}

[Serializable]
public class StatData
{
    public string Name;
    public int ItemEffect;
}

[Serializable]
public class SpeedStatData
{
    public string Name;
    public float ItemEffect;
}

