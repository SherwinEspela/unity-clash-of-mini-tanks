using UnityEngine;
using Exploder; 

public class ExploderManager : MonoBehaviour {

    [SerializeField] ExploderObject exploderObject;
	private Transform objectToExplode; 

	public GameObject[] exploderTanks; 
	public Transform explosionFX;
	public Transform fireFX; 
	public Transform fireFXBase;
	public GameObject groundExplosionMark; 

    public void DoExplode(GameObject tankToExplode)
    {
        this.PerformMeshExploder(tankToExplode);
        DoExplodeFXOnly(tankToExplode.transform);
    }

	void DoExplodeFXOnly(Transform objectToExplode_)
	{
		this.objectToExplode = objectToExplode_; 

		// add explosion and fire fx
		Vector3 explosionFXPosition = new Vector3 (objectToExplode.position.x, 0.1f,objectToExplode.position.z);
        var fxType = EffectsType.GroundExplosion;
        Transform clone = this.gameObject.GetComponent<EffectsManager>().SpawnEffectsWithoutDespawn(fxType, objectToExplode_);
        clone.parent = this.transform;
        this.gameObject.GetComponent<EffectsManager>().DespawnEffectsDelayed(fxType, clone.gameObject, 5.0f);

        int willShowFire = Random.Range (0,5);
		if (willShowFire > 2) {
            var fireFxType = EffectsType.Fire;
            Transform cloneFireFx = this.gameObject.GetComponent<EffectsManager>().SpawnEffectsWithoutDespawn(fireFxType, objectToExplode_);
            this.gameObject.GetComponent<EffectsManager>().DespawnEffectsDelayed(fireFxType, cloneFireFx.gameObject, 5.0f);
        }
	}

	public void DoExplodeBase(GameObject baseToExplode)
	{
		this.objectToExplode = baseToExplode.transform; 

		// explode process
		this.PerformMeshExploder (baseToExplode, true);
		DoExplodeBaseFXOnly (baseToExplode.transform); 
	}

	public void DoExplodeBaseFXOnly(Transform objectToExplode_)
	{
		this.objectToExplode = objectToExplode_; 

		// add explosion and fire fx
		Vector3 explosionFXPosition = new Vector3 (objectToExplode.position.x, 0.21f,objectToExplode.position.z);
        this.gameObject.GetComponent<EffectsManager>().SpawnEffects(EffectsType.GroundExplosion, objectToExplode_);
        this.gameObject.GetComponent<EffectsManager>().SpawnEffects(EffectsType.Fire, objectToExplode_);

        // add ground explosion mark
        GameObject groundExplosionMarkClone = Instantiate (groundExplosionMark,
		                                                   new Vector3 (objectToExplode.position.x, 0.21f, objectToExplode.position.z),
		                                                   objectToExplode.rotation) as GameObject;
		Transform gemcTransform = groundExplosionMarkClone.transform;
		gemcTransform.rotation = new Quaternion (0f,Random.Range(0f,360f),0f,0f);
	}

	void PerformMeshExploder(GameObject objectToExplode, bool isBaseGeometry = false){
        exploderObject.gameObject.SetActive(true);
		Vector3 exploderPosition = new Vector3 (objectToExplode.transform.position.x,objectToExplode.transform.position.y + 0.58f, objectToExplode.transform.position.z); 
		exploderObject.transform.position = exploderPosition;
        ConfigureExploderForTankMesh();
        exploderObject.ExplodeObject (objectToExplode);
	}

	void ConfigureExploderForBaseMesh()
	{
        exploderObject.FragmentPoolSize = 30;
        exploderObject.Radius = 1f;
        exploderObject.Force = 4.5f;
        exploderObject.FragmentOptions.MaxVelocity = 12f;
        exploderObject.SplitMeshIslands = true;
	}

	void ConfigureExploderForTankMesh()
	{
        exploderObject.FragmentPoolSize = 15;
        exploderObject.Radius = 0.7f;
        exploderObject.Force = 1f;
        exploderObject.FragmentOptions.MaxVelocity = 5f;
        exploderObject.SplitMeshIslands = false;
        exploderObject.DestroyOriginalObject = false;
        exploderObject.HideSelf = true;  
	}
}