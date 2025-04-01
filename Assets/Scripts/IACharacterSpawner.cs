using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IACharacterSpawner : Spawner<IACharacter>
{
    protected override IACharacter SpawnObject()
    {
        IACharacter character = base.SpawnObject();
        if (!character.FirstTimeSpawning)
            character.HitPointsManager.Revive();

        return character;
    }
}
