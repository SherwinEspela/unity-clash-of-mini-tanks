using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] GameObject groundTilePrefab;
    [SerializeField] GameObject wallTilePrefab;
    [SerializeField] GameObject waterTilePrefab;
    
    [SerializeField] Transform tilesContainer;
    [SerializeField] Transform tileShapesContainer;
    [SerializeField] ZoneGizmo groundZoneGizmo;
    [SerializeField] ZoneGizmo outerWallZoneGizmo;
    [SerializeField] ZoneChecker zoneChecker;

    // base
    [SerializeField] GameObject[] bases;
    [SerializeField] BoxCollider[] basePositions;

    // Tile shapes
    [SerializeField] TileShape[] tileShapePrefabs;

    // Nav Mesh
    [SerializeField] Transform navMeshTilesContainer;
    [SerializeField] [Range(0, 10)] float tileShapeFrequency = 8;
    [SerializeField] [Range(5, 25)] float baseZoneSize = 25;

    private List<TileData> groundDataList;
    private List<TileData> outerWallDataList;
    private List<SpawnPoint> spawnPointsEnemyList = new List<SpawnPoint>();
    private List<SpawnPoint> spawnPointsFriendlyList = new List<SpawnPoint>();
    private Dictionary<Vector2Int, WaterTile> waterTiles;
    private int scaleFactor = 25;
    private int minPositionX;
    private int minPositionZ;
    private int maxPositionX;
    private int maxPositionZ;

    // Materials
    [SerializeField] Material[] groundTileMaterials;
    [SerializeField] Material[] wallTileMaterials;
    [SerializeField] Material[] outerWallTileMaterials;
    private Material groundAndWaterTileMaterial;
    private Material wallTileMaterial;
    private Material outerWallTileMaterial;

    private delegate void GenerateDataDelegate(float x, float y);

    public delegate void EnvironmentManagerDelegate();
    public static event EnvironmentManagerDelegate OnInitializeRandomEnvironmentCompleted;

    public void InitializeRandomEnvironment()
    {
        groundDataList = new List<TileData>();
        outerWallDataList = new List<TileData>();
        waterTiles = new Dictionary<Vector2Int, WaterTile>();

        SetRandomTileMaterials();
        InitializeBase();
        InitializeTileShapes();
        InitializeGroundData();
        InitializeOuterWallData();

        PopulateGround();
        PopulateWalls();
        PopulateOuterWalls();
        PopulateSpawnPoints();

        CleanUpWaterTiles();
        BakeNavMesh();

        if (OnInitializeRandomEnvironmentCompleted != null)
        {
            OnInitializeRandomEnvironmentCompleted();
        }        
    }

    private void InitializeData(ZoneGizmo zoneGizmo, GenerateDataDelegate del)
    {
        scaleFactor = zoneGizmo.GetScale();
        minPositionX = -scaleFactor;
        minPositionZ = scaleFactor;
        maxPositionX = scaleFactor;
        maxPositionZ = -scaleFactor;

        for (float x = minPositionX; x < maxPositionX + 1; x += GetTileWidth())
        {
            for (float z = minPositionZ; z > maxPositionZ - 1; z -= GetTileWidth())
            {
                del(x, z);
            }
        }
    }

    private void PopulateSpawnPoints()
    {
        scaleFactor = groundZoneGizmo.GetScale();
        minPositionX = -scaleFactor;
        minPositionZ = scaleFactor;
        maxPositionX = scaleFactor;
        maxPositionZ = -scaleFactor;

        for (float x = minPositionX; x < maxPositionX + 1; x += GetTileWidth())
        {
            for (float z = minPositionZ; z > maxPositionZ - 1; z -= GetTileWidth())
            {
                if (x == minPositionX || x == maxPositionX || z == minPositionZ || z == maxPositionZ)
                {
                    SpawnPoint sp = new SpawnPoint(new Vector3(x, 0, z));
                    if (Random.Range(0, 100) < 50)
                    {
                        spawnPointsEnemyList.Add(sp);
                    } else
                    {
                        spawnPointsFriendlyList.Add(sp);
                    }
                }
            }
        }
    }

    private void InitializeGroundData()
    {
        InitializeData(groundZoneGizmo, GenerateGroundData);
    }

    private void InitializeOuterWallData()
    {
        InitializeData(outerWallZoneGizmo, GenerateOuterWallData);
    }

    private void InitializeTileShapes()
    {
        InitializeData(groundZoneGizmo, PopulateTileShapes);
    }

    private void PopulateTileShapes(float posX, float posZ)
    {
        if (Random.Range(0, 100) < tileShapeFrequency)
        {
            TileShape tileShape = Instantiate(tileShapePrefabs[Random.Range(0, tileShapePrefabs.Length - 1)]).GetComponent<TileShape>();
            tileShape.Initialize();
            tileShape.transform.position = new Vector3(posX, 0, posZ);
            tileShape.transform.parent = tileShapesContainer;
        }
    }

    private void PopulateGround()
    {
        groundTilePrefab.GetComponent<Tile>().SetMaterial(groundAndWaterTileMaterial);
        waterTilePrefab.GetComponent<WaterTile>().SetMaterial(groundAndWaterTileMaterial);
        foreach (var data in groundDataList)
        {
            if (data.IsGround)
            {
                InstantiateTile(groundTilePrefab, data.TilePosition);
            }

            if (data.IsWater)
            {
                InstantiateTile(waterTilePrefab, data.TilePosition, true);
            }
        }
    }

    private void PopulateWalls()
    {
        wallTilePrefab.GetComponent<Tile>().SetMaterial(wallTileMaterial);
        foreach (var data in groundDataList)
        {
            if (data.IsGround && data.IsInnerWall)
            {
                InstantiateTile(wallTilePrefab, new Vector3(data.TilePosition.x, wallTilePrefab.transform.position.y, data.TilePosition.z));
            }
        }
    }

    private void PopulateOuterWalls()
    {
        wallTilePrefab.GetComponent<Tile>().SetMaterial(outerWallTileMaterial);
        foreach (var data in outerWallDataList)
        {
            InstantiateTile(wallTilePrefab, new Vector3(data.TilePosition.x, wallTilePrefab.transform.position.y, data.TilePosition.z));
        }
    }

    private void InstantiateTile(GameObject tilePrefab, Vector3 tilePosition, bool isWater = false)
    {
        GameObject tile = Instantiate(tilePrefab);
        tile.transform.position = tilePosition;
        tile.transform.parent = tilesContainer;
        AddGroundTilesToNavMeshContainer(tile);

        if (isWater)
        {
            waterTiles.Add(new Vector2Int((int)tilePosition.x, (int)tilePosition.z), tile.GetComponent<WaterTile>());
        }
    }

    private void AddGroundTilesToNavMeshContainer(GameObject tile)
    {
        bool isGroundTile = tile.name.ToLower().Contains("ground");
        tile.transform.parent = isGroundTile ? navMeshTilesContainer : tilesContainer;
    }

    private void GenerateGroundData(float posX, float posZ)
    {
        TileData data = new TileData();
        data.TilePosition = new Vector3(posX, 0, posZ);

        float groundZoneScaleFactor = groundZoneGizmo.GetScale();
        if (Mathf.Abs(posX) >= groundZoneScaleFactor || Mathf.Abs(posZ) >= groundZoneScaleFactor)
        {
            groundDataList.Add(data);
            return;
        }

        Vector3 zoneCheckerPosition = new Vector3(posX, zoneChecker.transform.position.y, posZ);
        if (zoneChecker.FoundZone(zoneCheckerPosition, "zone"))
        {
            groundDataList.Add(data);
            return;
        }

        if (zoneChecker.FoundZone(zoneCheckerPosition, "tileshapewall"))
        {
            data.IsInnerWall = true;
            groundDataList.Add(data);
            return;
        }

        if (zoneChecker.FoundZone(zoneCheckerPosition, "tileshapewater"))
        {
            data.IsGround = false;
            data.IsWater = true;
            groundDataList.Add(data);
            return;
        }

        groundDataList.Add(data);
    }

    private void GenerateOuterWallData(float posX, float posZ)
    {
        float groundZoneScaleFactor = groundZoneGizmo.GetScale();
      
        if (Mathf.Abs(posX) <= groundZoneScaleFactor && Mathf.Abs(posZ) <= groundZoneScaleFactor) 
        {
            return;
        }

        TileData data = new TileData();
        data.TilePosition = new Vector3(posX, 0, posZ);
        data.IsInnerWall = false;
        data.IsGround = false;
        data.IsBorderWall = true;

        outerWallDataList.Add(data);
    }

    private void InitializeBase()
    {
        Utility.Shuffle<BoxCollider>(basePositions, 20);
        for (int i = 0; i < bases.Length ; i++)
        {
            bases[i].transform.position = basePositions[i].transform.position;
            bases[i].GetComponent<Base>().RotateBase();
        }
    }

    private float GetTileWidth()
    {
        Transform child = groundTilePrefab.transform.GetChild(0);
        return child.localScale.x;
    }

    private void CleanUpWaterTiles()
    {
        foreach (var item in waterTiles)
        {
            WaterTile waterTile = item.Value;
            HideWaterTileEdge(waterTile, new Vector2Int((int)item.Key.x, (int)item.Key.y + (int)GetTileWidth()), WaterTileEdge.Rear);
            HideWaterTileEdge(waterTile, new Vector2Int((item.Key.x - (int)GetTileWidth()), (int)item.Key.y), WaterTileEdge.Left);
            HideWaterTileEdge(waterTile, new Vector2Int((item.Key.x + (int)GetTileWidth()), (int)item.Key.y), WaterTileEdge.Right);
        }
    }

    private void HideWaterTileEdge(WaterTile waterTile, Vector2Int waterTileVector, WaterTileEdge edgeType)
    {
        try
        {
            bool hasWaterTileNeighbor = waterTiles[waterTileVector];
            waterTile.HideEdge(edgeType, hasWaterTileNeighbor);
        }
        catch{}
    }

    private void BakeNavMesh()
    {
        NavMeshSurface nms = navMeshTilesContainer.GetComponent<NavMeshSurface>();
        nms.BuildNavMesh();
    }

    private void SetRandomTileMaterials()
    {
        groundAndWaterTileMaterial = SetPrefabTileMaterial(groundTileMaterials);
        wallTileMaterial = SetPrefabTileMaterial(wallTileMaterials);
        outerWallTileMaterial = SetPrefabTileMaterial(outerWallTileMaterials);
    }

    private Material SetPrefabTileMaterial(Material[] tileMaterials)
    {
        return tileMaterials[Random.Range(0, tileMaterials.Length - 1)];
    }

    public Vector3 GetSpawnPoint(bool enemy = true)
    {
        SpawnPoint sp = null;
        do
        {
            int randomNumber = Random.Range(0, (enemy ? spawnPointsEnemyList.Count : spawnPointsFriendlyList.Count) - 1);
            sp = enemy ? spawnPointsEnemyList[randomNumber] : spawnPointsFriendlyList[randomNumber];
        } while (sp.IsVisible());
        
        return sp.SpawnPosition;
    }

    public List<TileData> GetGroundDataList()
    {
        return this.groundDataList;
    }
}
