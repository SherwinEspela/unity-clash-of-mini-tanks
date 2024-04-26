using UnityEngine;
using Cybermash;

public class CannonPoolManager : MonoBehaviour
{
    [SerializeField] GameObject cannonPrefab;
    [SerializeField] int quantity;

    [SerializeField] GameObject cannonOneShotPrefab;
    [SerializeField] int cannonOneShotQuantity;

    private CMPoolManager cannonPool = new CMPoolManager();
    private CMPoolManager cannonOneShotPool = new CMPoolManager();

    // Events
    public delegate void CannonPoolManagerDelegate();
    public static event CannonPoolManagerDelegate OnInitializeCannonsComplete;

    public void InitializeCannons()
    {
        cannonPool.Add(cannonPrefab, quantity);
        cannonOneShotPool.Add(cannonOneShotPrefab, cannonOneShotQuantity);

        if (OnInitializeCannonsComplete != null)
        {
            OnInitializeCannonsComplete();
        }
    }

    public GameObject SpawnCannon(Vector3 position, Quaternion rotation, bool isOneShot = false)
    {
        GameObject cannon = null;
        if (isOneShot)
        {
            cannon = cannonOneShotPool.Spawn(position, rotation);
            cannonOneShotPool.DespawnSimple(cannon);
        }
        else
        {
            cannon = cannonPool.Spawn(position, rotation);
            cannonPool.DespawnSimple(cannon);
        }
        cannon.GetComponent<Cannon>().Init();
        return cannon;
    }
}
