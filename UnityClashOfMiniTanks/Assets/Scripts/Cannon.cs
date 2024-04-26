using UnityEngine;
using System.Collections;
using CMT.AI;

public class Cannon : MonoBehaviour
{
    private GameObject gameManager;

    [SerializeField] GameObject[] cannonHitFX;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] Rigidbody rigidBody;
    public GameObject FiredBy { get; set; }

    // Events
    public delegate void CannonCollissionDelegate();
    public static event CannonCollissionDelegate OnCannonCollided;

    private void OnEnable()
    {
        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager");
        }

        if (!sphereCollider)
        {
            sphereCollider = this.gameObject.GetComponent<SphereCollider>();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }

        StopCoroutine(ActivateCannon());
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        sphereCollider.enabled = false;
        this.gameObject.SetActive(false);

        bool isPlayerTank = col.gameObject.tag.Equals(GameConstants.kTagPlayerTank);
        bool isShotByPlayer = FiredBy.tag.Equals(GameConstants.kTagPlayerTank);
        bool isEnemyTank = col.gameObject.tag.Equals(GameConstants.kTagEnemyTank);
        bool isFriendlyTank = col.gameObject.tag.Equals(GameConstants.kTagFriendlyTank);
        bool isBase = col.gameObject.tag.Equals(GameConstants.kTagBase);

        if (isEnemyTank || isFriendlyTank)
        {
            HealthAI health = col.gameObject.GetComponent<HealthAI>();
            float damageValue = GameConstants.cannonDamage;
            if (isShotByPlayer && PowerUpManager.increasePlayerCannonPowerIsActivated)
            {
                damageValue = GameConstants.cannonDamageOneShotPlayer;
            }
            health.ReduceHealthValue(damageValue, isShotByPlayer);
            
            if (isEnemyTank)
            {
                if (isShotByPlayer)
                {
                    GameConstants.gameManager.GetComponent<ScoreManager>().EnemyKills++;
                }
                EnemyTankAI etaiScript = col.gameObject.GetComponent<EnemyTankAI>();
                etaiScript.ChangeTargetToPlayer();
            }
            
        }
        else if (isPlayerTank)
        {
            HealthPlayer hp = col.gameObject.GetComponent<HealthPlayer>();
            hp.ReducePlayerHealthValue(GameConstants.cannonDamage);
        }
        else if (isBase)
        {
            HealthBase hb = col.gameObject.GetComponent<HealthBase>();
            hb.ReduceHealthValue(GameConstants.cannonDamage, isShotByPlayer);
        }

        this.gameManager.GetComponent<EffectsManager>().SpawnEffects(EffectsType.CannonHit, this.transform);
        if (OnCannonCollided != null)
        {
            OnCannonCollided();
        }
    }

    public void Init()
    {
        sphereCollider.enabled = false;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        StartCoroutine(ActivateCannon());
    }

    private IEnumerator ActivateCannon()
    {
        yield return new WaitForSeconds(0.12f);
        this.sphereCollider.enabled = true;
    }
}
