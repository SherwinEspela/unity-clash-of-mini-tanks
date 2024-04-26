using UnityEngine;
using Cybermash;

public interface ISpawnManagerDelegate
{
    void TankDidDespawn(GameObject tank);
}


public class SpawnManager : MonoBehaviour
{
    [SerializeField] protected EnvironmentManager environmentManager;
    [SerializeField] protected int maximumNumberOfTanks;
    [SerializeField] protected float movementSpeed = 2f;

    protected CMPoolManager tanksPool = new CMPoolManager();
    protected int tanksInScene = 0;

    public ISpawnManagerDelegate delegateSpawnManager;

    public void AddTankToPool(GameObject tank)
    {
        tanksPool.Add(tank);
    }

    public void DespawnTank(GameObject go)
    {
        if (!go.GetComponent<HealthAI>().IsPowerUpTank)
        {
            tanksInScene--;
            if (tanksInScene < 0)
            {
                tanksInScene = 0;
            }
        }

        DisplayParts(go.transform);
        // radarMapManager.RemoveAITankInRadarMap(go.transform);

        if (delegateSpawnManager != null)
        {
            delegateSpawnManager.TankDidDespawn(go);
        }

        tanksPool.Despawn(go);
    }

    protected void DisplayParts(Transform tr, bool isFriendlyTank = false)
    {
        Transform tankBodyRotation = tr.Find(GameConstants.kTankBodyRotation);
        tankBodyRotation.gameObject.SetActive(true);

        Transform body = tankBodyRotation.Find("Body");
        body.gameObject.SetActive(true);
        body.Find("colliderHealthBody").gameObject.SetActive(true);
        body.Find("colliderExitDetector").gameObject.SetActive(true);
        body.Find("damageFXLocator").gameObject.SetActive(true);

        tankBodyRotation.Find("LeftTrack").gameObject.SetActive(true);
        tankBodyRotation.Find("RightTrack").gameObject.SetActive(true);

        Transform turret = tankBodyRotation.Find(GameConstants.kTurret);
        turret.gameObject.SetActive(true);
        Transform cannon = turret.Find("Cannon");
        cannon.gameObject.SetActive(true);
        cannon.Find("FirePoint").gameObject.SetActive(true);
        cannon.Find("textSpawn").gameObject.SetActive(true);
        turret.Find("colliderHealthTurret").gameObject.SetActive(true);

        tankBodyRotation.Find(GameConstants.kPointerToNewTarget).gameObject.SetActive(true);

        Transform cs = tr.Find("CanvasShadow");
        cs.gameObject.SetActive(true);
        cs.Find("ImageShadow").gameObject.SetActive(true);

        if (isFriendlyTank)
        {
            Transform ct = tr.Find("CanvasTag");
            ct.gameObject.SetActive(true);
            ct.Find("Image").gameObject.SetActive(true);
        }
    }
}
