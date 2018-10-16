using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : TSingleton<GameConfig>
{
    public float _cannonHalfHeight_Common = 0.5f;

    // Scene
    public readonly string _LauncherSceneName = "Launcher";
    public readonly string _PlaySceneName = "Play";

    // sql
    public readonly string _AccountTableName = "Account";
    public readonly string _AccountName = "Local";

    public readonly string _TurretPrefabName = "Turret";

    public readonly string _ = "Turret";
    public readonly bool _PlanetImmuneDamage = false;
    public readonly bool _TurretImmuneDamage = false;
    public readonly int _MaxFOV = 106;
    public readonly int _MinFOV = 50;
}
