using UnityEngine;

public class LoadSequenceManager : MonoBehaviour
{
    [SerializeField] EnvironmentManager environmentManager;
    [SerializeField] PlayerLoader playerLoader;
    [SerializeField] CannonPoolManager cannonPoolManager;
    [SerializeField] SpawnEnemyTanksManager spawnEnemyTanksManager;
    [SerializeField] SpawnFriendlyTanksManager spawnFriendlyTanksManager;
    [SerializeField] EffectsManager effectsManager;
    [SerializeField] RadarMapViewController radarMapVC;
    [SerializeField] BaseHealthManager baseHealthManager;

    void Start()
    {
        environmentManager.InitializeRandomEnvironment();
    }

    private void OnEnable()
    {
        EnvironmentManager.OnInitializeRandomEnvironmentCompleted += playerLoader.LoadTanks;
        PlayerLoader.OnLoadingTanksComplete += cannonPoolManager.InitializeCannons;
        CannonPoolManager.OnInitializeCannonsComplete += effectsManager.Load;
        EffectsManager.OnLoadingComplete += radarMapVC.Init;
        RadarMapViewController.OnInitilizationComplete += baseHealthManager.HideNavMeshGeos;
    }

    private void OnDisable()
    {
        EnvironmentManager.OnInitializeRandomEnvironmentCompleted -= playerLoader.LoadTanks;
        PlayerLoader.OnLoadingTanksComplete -= cannonPoolManager.InitializeCannons;
        CannonPoolManager.OnInitializeCannonsComplete -= effectsManager.Load;
        EffectsManager.OnLoadingComplete -= radarMapVC.Init;
        RadarMapViewController.OnInitilizationComplete -= baseHealthManager.HideNavMeshGeos;
    }
}
