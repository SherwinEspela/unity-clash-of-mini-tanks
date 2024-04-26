using System.Collections;
using UnityEngine;
using Cybermash;

public enum EffectsType
{
    CannonHit,
    CannonHitOneShotKill,
    Fire,
    GroundExplosion,
    TextFx,
    Muzzle,
    Smoke,
    SmokeTrail,
    PowerUpFx,
    PowerUpHealthFx,
    PowerUpShieldFx,
    PowerUpShieldBreakFx
}

public class EffectsManager : MonoBehaviour
{
    private CMPoolManager cannonHitsPool = new CMPoolManager();
    private CMPoolManager cannonOneShotHitPool = new CMPoolManager();
    private CMPoolManager fireFxPool = new CMPoolManager();
    private CMPoolManager groundExplosionPool = new CMPoolManager();
    private CMPoolManager textFxPool = new CMPoolManager();
    private CMPoolManager muzzleFxPool = new CMPoolManager();
    private CMPoolManager smokeFxPool = new CMPoolManager();
    private CMPoolManager smokeTrailPool = new CMPoolManager();
    private CMPoolManager powerUpFxPool = new CMPoolManager();
    private CMPoolManager healthFxPool = new CMPoolManager();
    private CMPoolManager shieldFxPool = new CMPoolManager();
    private CMPoolManager shieldBreakFxPool = new CMPoolManager();

    [SerializeField] GameObject[] cannonHits;
    [SerializeField] GameObject[] fireFxs;
    [SerializeField] GameObject[] groundExplosions;
    [SerializeField] GameObject[] textFxs;
    [SerializeField] GameObject muzzlePrefab;
    [SerializeField] GameObject smokePrefab;
    [SerializeField] GameObject smokeTrailPrefab;
    [SerializeField] GameObject powerUpFxPrefab;
    [SerializeField] GameObject cannonOneShotHitPrefab;
    [SerializeField] GameObject healthFxPrefab;
    [SerializeField] GameObject shieldFxPrefab;
    [SerializeField] GameObject shieldBreakFxPrefab;

    // Events
    public delegate void EffectsManagerDelegate();
    public static event EffectsManagerDelegate OnLoadingComplete;

    public void Load()
    {
        cannonHitsPool.AddAndShuffle(cannonHits, 20);
        groundExplosionPool.AddAndShuffle(groundExplosions, 16);
        fireFxPool.AddAndShuffle(fireFxs, 16);
        textFxPool.AddAndShuffle(textFxs, 16);
        muzzleFxPool.Add(muzzlePrefab, 16);
        smokeFxPool.Add(smokePrefab, 16);
        smokeTrailPool.Add(smokeTrailPrefab, 16);
        powerUpFxPool.Add(powerUpFxPrefab, 5);
        cannonOneShotHitPool.Add(cannonOneShotHitPrefab, 5);
        healthFxPool.Add(healthFxPrefab, 5);
        shieldFxPool.Add(shieldFxPrefab, 5);
        shieldBreakFxPool.Add(shieldBreakFxPrefab, 5);

        if (OnLoadingComplete != null)
        {
            OnLoadingComplete();
        }
    }

    private GameObject SelectEffects(EffectsType type, Transform tr)
    {
        GameObject go = null;
        switch (type)
        {
            case EffectsType.CannonHit:
                go = cannonHitsPool.Spawn(tr.position);
                break;
            case EffectsType.Fire:
                go = fireFxPool.Spawn(tr.position);
                break;
            case EffectsType.GroundExplosion:
                go = groundExplosionPool.Spawn(tr.position);
                break;
            case EffectsType.TextFx:
                go = textFxPool.Spawn(tr.position);
                break;
            case EffectsType.Muzzle:
                go = muzzleFxPool.Spawn(tr.position);
                break;
            case EffectsType.Smoke:
                go = smokeFxPool.Spawn(tr.position);
                break;
            case EffectsType.SmokeTrail:
                go = smokeTrailPool.Spawn(tr.position);
                break;
            case EffectsType.PowerUpFx:
                go = powerUpFxPool.Spawn(tr.position);
                break;
            case EffectsType.CannonHitOneShotKill:
                go = cannonOneShotHitPool.Spawn(tr.position);
                break;
            case EffectsType.PowerUpHealthFx:
                go = healthFxPool.Spawn(tr.position);
                break;
            case EffectsType.PowerUpShieldFx:
                go = shieldFxPool.Spawn(tr.position);
                break;
            case EffectsType.PowerUpShieldBreakFx:
                go = shieldBreakFxPool.Spawn(tr.position);
                break;
            default:
                break;
        }

        return go;
    }

    public void SpawnEffects(EffectsType type, Transform tr)
    {
        GameObject go = SelectEffects(type, tr);
        StartCoroutine(DespawnDelayed(type, go));
    }

    public Transform SpawnEffectsWithoutDespawn(EffectsType type, Transform transform)
    {
        // TODO: fix NullReferenceException occuring here
        GameObject go = SelectEffects(type, transform);
        return go.transform;
    }

    public void DespawnEffectsDelayed(EffectsType type, GameObject go, float delayTime)
    {
        StartCoroutine(DespawnDelayed(type, go, delayTime));
    }

    IEnumerator DespawnDelayed(EffectsType type, GameObject go, float delayTime = 2.0f)
    {
        yield return new WaitForSeconds(delayTime);

        switch (type)
        {
            case EffectsType.CannonHit:
                cannonHitsPool.Despawn(go);
                break;
            case EffectsType.Fire:
                fireFxPool.Despawn(go);
                break;
            case EffectsType.GroundExplosion:
                groundExplosionPool.Despawn(go);
                break;
            case EffectsType.TextFx:
                textFxPool.Despawn(go);
                break;
            case EffectsType.Muzzle:
                muzzleFxPool.Despawn(go);
                break;
            case EffectsType.Smoke:
                smokeFxPool.Despawn(go);
                break;
            case EffectsType.SmokeTrail:
                smokeTrailPool.Despawn(go);
                break;
            case EffectsType.PowerUpFx:
                powerUpFxPool.Despawn(go);
                break;
            case EffectsType.CannonHitOneShotKill:
                cannonOneShotHitPool.Despawn(go);
                break;
            case EffectsType.PowerUpHealthFx:
                healthFxPool.Despawn(go);
                break;
            case EffectsType.PowerUpShieldFx:
                shieldFxPool.Despawn(go);
                break;
            case EffectsType.PowerUpShieldBreakFx:
                shieldBreakFxPool.Despawn(go);
                break;
            default:
                break;
        }
    }
}
