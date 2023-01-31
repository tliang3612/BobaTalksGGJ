using UnityEngine;
using System;
using System.Collections.Generic;

public enum PowerupType
{
    Crescendo,
    DoubleJump,
    Vase,
    LuckyCoin
}

public static class PowerupInventory
{
    private static Dictionary<PowerupType, bool> _powerupInventory = new Dictionary<PowerupType, bool>()
    {
        {PowerupType.Crescendo, false },
        {PowerupType.DoubleJump, false },
        {PowerupType.Vase, false },
        {PowerupType.LuckyCoin, false }
    };

    public static bool CheckIfAvailable(PowerupType powerupKey)
    {
        if (_powerupInventory.ContainsKey(powerupKey))
            return _powerupInventory[powerupKey];

        return false;
    }

    public static void AddPowerup(PowerupType powerupKey)
    {
        if (_powerupInventory.ContainsKey(powerupKey))
            _powerupInventory[powerupKey] = true;
    }
}
