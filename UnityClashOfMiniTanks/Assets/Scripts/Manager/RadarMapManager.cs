using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;
using Cybermash;

// TODO: remove later

public class RadarMapManager : MonoBehaviour {

	private Transform playerTank;
	[SerializeField] float scaleFactor = 23f; 
	[SerializeField] float mapScaleFactor = 94f;
    
	public RectTransform radarMap; 
	public RectTransform playerIcon;

	public GameObject panelRadarMap2;

    // for base
    private List<RectTransform> baseIcons = new List<RectTransform>();
    private List<RectTransform> baseAttackedImages = new List<RectTransform>();
    private List<RectTransform> baseCriticalImages = new List<RectTransform>();
    [SerializeField] Transform[] baseRadarMapLocators;
	[SerializeField] Sprite baseSprite; 
	[SerializeField] Sprite baseDestroyedSprite;
    [SerializeField] GameObject tileMapPrefab;
    [SerializeField] GameObject baseAttackedImage; 
	[SerializeField] GameObject baseCriticalImage;

    // for friendly tanks
    private List<Transform> AITankTransforms = new List<Transform>(); 
	private List<RectTransform> AITankIcons = new List<RectTransform>(); 

	private Color colorBase = new Color(0,255f/255,12f/255);
    private Color _colorEnemyTank = new Color(255f / 255, 40f / 255, 40f / 255);

    private CMPoolManager radarMapImagePool = new CMPoolManager();
    [SerializeField] GameObject radarMapImagePrefab;
    [SerializeField] int radarMapImageQuantity = 40;
    [SerializeField] Transform radarMapContainer;

    // Events
    public delegate void RadarMapManagerDelegate();
    public static event RadarMapManagerDelegate OnInitilizationComplete;

    private List<TileData> groundTileData = new List<TileData>();

    public void Init()
    {
        radarMapImagePool.Add(radarMapImagePrefab, this.radarMapImageQuantity, this.radarMapContainer);
        
        FindPlayerTank();
        InitializeBaseIcons();
        InitializeTileMaps();

        if (OnInitilizationComplete != null)
        {
            OnInitilizationComplete();
        }
    }

    void InitializeBaseIcons()
	{
		int nameCounter = 1; 
		foreach (var baseTransform in baseRadarMapLocators) {
			GameObject clone = Instantiate(radarMapImagePrefab) as GameObject;
			clone.transform.parent = panelRadarMap2.transform; 
			Image iScript = clone.GetComponent<Image>();
			iScript.sprite = baseSprite;
			iScript.color = colorBase;  
			RectTransform rtScript = clone.GetComponent<RectTransform>();
			rtScript.localScale = new Vector3(1.8f,1.8f,1); 
			baseIcons.Add(rtScript); 

			GameObject cloneBaseAttackedImage = Instantiate(baseAttackedImage) as GameObject;
			cloneBaseAttackedImage.name = "baseAttackedIcon" + nameCounter;

			GameObject cloneBaseCriticalImage = Instantiate(baseCriticalImage) as GameObject;
			cloneBaseCriticalImage.name = "baseCriticalIcon" + nameCounter;

			nameCounter++; 

			BaseTargetInfo btiScript = baseTransform.gameObject.GetComponent<BaseTargetInfo>();
			btiScript.BaseAttackedRadarMapIcon = cloneBaseAttackedImage; 
			btiScript.BaseCriticalRadarMapIcon = cloneBaseCriticalImage; 

			cloneBaseAttackedImage.transform.SetParent(panelRadarMap2.transform,true);
			cloneBaseCriticalImage.transform.SetParent(panelRadarMap2.transform,true);

			baseAttackedImages.Add(cloneBaseAttackedImage.GetComponent<RectTransform>()); 
			baseCriticalImages.Add(cloneBaseCriticalImage.GetComponent<RectTransform>());
		}

		if (baseIcons.Count > 0) {
			UpdateBaseIconPosition(); 	
		}
	}

    void InitializeTileMaps()
    {
        if (this.groundTileData.Count > 0)
        {
            for (int i = 0; i < this.groundTileData.Count; i++)
            {
                TileData tileData = groundTileData[i];
                if (tileData.IsInnerWall || tileData.IsWater)
                {
                    float xPos = tileData.TilePosition.x * mapScaleFactor / scaleFactor;
                    float yPos = tileData.TilePosition.z * mapScaleFactor / scaleFactor;

                    GameObject clone = Instantiate(tileMapPrefab);
                    clone.SetActive(true);
                    clone.transform.parent = panelRadarMap2.transform;
                    var rt = clone.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(xPos, yPos);
                    float locScale = 0.9f;
                    rt.localScale = new Vector3(locScale, locScale, 1f);
                }
            }
        }
    }

	public void ReplaceDestroyedBaseIcons()
	{
		for (int i = 0; i < baseIcons.Count; i++) {
			Image iScript = baseIcons[i].gameObject.GetComponent<Image>();
			BaseTargetInfo btsScript = baseRadarMapLocators[i].gameObject.GetComponent<BaseTargetInfo>(); 
			
			if (btsScript.IsDestroyed) {
				iScript.sprite = baseDestroyedSprite;
			} else {
				iScript.sprite = baseSprite;
			}
			iScript.color = colorBase;
		}

		UpdateBaseIconPosition (); 
	}

	void FindPlayerTank()
	{
		playerTank = GameObject.FindGameObjectWithTag ("Player").transform; 
	}

	void Update () {
		if (playerTank != null) {
			UpdatePlayerIconPosition(); 
		}

		UpdateAITankPosition (); 
	}

	void UpdatePlayerIconPosition()
	{
		float xPos = playerTank.position.x * mapScaleFactor / scaleFactor;
		float yPos = playerTank.position.z * mapScaleFactor / scaleFactor;
		playerIcon.anchoredPosition = new Vector2 (xPos,yPos); 
	}

	void UpdateBaseIconPosition()
	{
		for (int i = 0; i < baseIcons.Count ; i++) {
			float xPos = baseRadarMapLocators[i].position.x * mapScaleFactor / scaleFactor; 	
			float yPos = baseRadarMapLocators[i].position.z * mapScaleFactor / scaleFactor; 
			baseIcons[i].anchoredPosition = new Vector2(xPos,yPos);
            baseIcons[i].gameObject.SetActive(true);
			baseAttackedImages[i].anchoredPosition = new Vector2(xPos,yPos); 
			baseCriticalImages[i].anchoredPosition = new Vector2(xPos,yPos);
		}

		foreach (var item in baseAttackedImages) {
			item.gameObject.SetActive(false); 	
		}

		foreach (var item in baseCriticalImages) {
			item.gameObject.SetActive(false); 	
		}
	}

	void UpdateAITankPosition()
	{
        if (AITankIcons.Count > 0)
        {
            for (int i = 0; i < AITankIcons.Count; i++)
            {
                float xPos = AITankTransforms[i].position.x * mapScaleFactor / scaleFactor;
                float yPos = AITankTransforms[i].position.z * mapScaleFactor / scaleFactor;
                AITankIcons[i].anchoredPosition = new Vector2(xPos, yPos);
            }
        }
    }

    public void AddTankInRadar(Transform tank, bool isEnemy = true)
    {
        AITankTransforms.Add(tank);
        GameObject clone = radarMapImagePool.Spawn();
        clone.SetActive(false);
        clone.transform.SetParent(panelRadarMap2.transform, false);
        radarMapImagePool.DespawnSimple(clone);

        if (isEnemy)
        {
            Image iScript = clone.gameObject.GetComponent<Image>();
            iScript.color = _colorEnemyTank;
        }
        else
        {
            // friendly tanks
            HealthAI haiScript = tank.gameObject.GetComponent<HealthAI>();
            Image iScript = clone.gameObject.GetComponent<Image>();
            iScript.color = !haiScript.isEnemyTank && haiScript.IsPowerUpTank ? Color.cyan : Color.yellow;
        }

        RectTransform rtScript = clone.gameObject.GetComponent<RectTransform>();
        rtScript.localScale = new Vector3(1.4f, 1.4f, 1.4f);

        AITankIcons.Add(rtScript);
        StartCoroutine(SetAITankIconToActive(clone));
    }

    public void RemoveAITankInRadarMap(Transform AITank)
    {
        int counter = 0;
        foreach (var item in AITankTransforms)
        {
            if (item.name.Equals(AITank.name))
            {
                AITankTransforms.Remove(item);
                break;
            }
            counter++;
        }

        RectTransform rtClone = AITankIcons[counter];
        rtClone.gameObject.SetActive(false);
        AITankIcons.Remove(rtClone);
    }
    
	IEnumerator SetAITankIconToActive(GameObject aiTankIcon)
	{
		yield return new WaitForSeconds(0.4f);
		aiTankIcon.SetActive (true);
    }

    public void DidCollectGroundData(List<TileData> groundData)
    {
        this.groundTileData = groundData;
    }
}