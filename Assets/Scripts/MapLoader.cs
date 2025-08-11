#if UNITY_EDITOR //나중엔 빼야됨, 당장 에러 안보려고 임시로 넣음
using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
  public SimpleRoad[,] mapdata;//나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게
  public GameObject SimpleRoadPrefab;

  public GameObject CreateTile<T>() where T : SimpleRoad
{
    GameObject obj = Instantiate(SimpleRoadPrefab);
    
    // 기존 SimpleRoad 제거
    SimpleRoad oldComponent = obj.GetComponent<SimpleRoad>();
    if (oldComponent != null)
        DestroyImmediate(oldComponent);
    
    // 원하는 타입으로 교체
    T newComponent = obj.AddComponent<T>();
    return obj;
}

  public char[] TILE_DIRECTIONS = new char[] { ' ', ' ', 'u', 'd', ' ', 'l', ' ', 'r' };

  void Awake()
  {

  }
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    LoadMapData("temp"); // 실제 맵 로딩
    // TestLoadMap();
    // createMapObjects(); // 맵 오브젝트 생성
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.A))
    {

    }
  }


  private void LoadMapData(string Filename)
  {
    string tmxMapData = Resources.Load<TextAsset>("Level1").text; // 나중에 Filename으로 대체
    XmlDocument xmlMapData = new XmlDocument();
    xmlMapData.LoadXml(tmxMapData);

    // 맵 width, height 파싱
    XmlNode mapNode = xmlMapData.SelectSingleNode("map");

    if (mapNode == null)
    {
      Debug.LogError("<map> 태그를 찾을 수 없습니다.");
      return;
    }
    int mapHeight = int.Parse(mapNode.Attributes["height"].Value);
    int mapWidth = int.Parse(mapNode.Attributes["width"].Value);

    // mapdata를 width, height 맞춰서 할당
    mapdata = new SimpleRoad[mapHeight, mapWidth];


    // Road 데이터 파싱
    XmlNode dataNode = xmlMapData.SelectSingleNode("//map/layer/data");
    string csvText = dataNode.InnerText.Trim();

    string[] rows = csvText.Split('\n');
    for (int y = 0; y < mapHeight; y++)
    {
      string[] cols = rows[y].Trim().Split(',');
      for (int x = 0; x < mapWidth; x++)
      {
        uint rawTileId = uint.Parse(cols[x]);

        bool flipH = (rawTileId & 0x80000000) != 0;
        bool flipV = (rawTileId & 0x40000000) != 0;
        bool flipD = (rawTileId & 0x20000000) != 0;

        // 실제 타일 id 추출
        int actualTileId = (int)(rawTileId & 0x1FFFFFFF) - 1;

        // rotation 정보 추출
        int rotation = 0;

        if (!flipD && !flipH && !flipV) rotation = 0;
        else if (flipD && flipH && !flipV) rotation = 90;
        else if (!flipD && flipH && flipV) rotation = 180;
        else if (flipD && !flipH && flipV) rotation = 270;

        char[] RoadDirections = { };
        // tileId에 따라 directions 설정
        switch (actualTileId)
        {
          case -1: continue;
          case 10: RoadDirections = new char[] { 'l', 'r' }; break;
          case 11: RoadDirections = new char[] { 'u', 'r' }; break;
          case 12: RoadDirections = new char[] { 'l', 'u', 'r' }; break;
          case 13: RoadDirections = new char[] { 'u', 'd', 'r', 'l' }; break;
          case 16: RoadDirections = new char[] { 'r' }; break;
          default:
            RoadDirections = new char[0];
            break;
        }

        // rotation 값에 따라 directions 변경
        char[] IndexToDir = { 'u', 'r', 'd', 'l' };
        Dictionary<char, int> dirToIndex = new Dictionary<char, int> {
          {'u', 0}, {'r', 1}, {'d', 2}, {'l', 3}
        };
        for (int i = 0; i < RoadDirections.Length; i++)
        {
          RoadDirections[i] = IndexToDir[(dirToIndex[RoadDirections[i]] + rotation / 90) % 4];
        }

        // gameobject 생성 및 behaviour component 추출
        SimpleRoad Tile = Instantiate(SimpleRoadPrefab).GetComponent<SimpleRoad>();
        Tile.Init(RoadDirections);
        Debug.Log($"타일ID {actualTileId}, 위치 : ({y},{x}), {new string(RoadDirections)}");

        // 컴포넌트를 맵 배열에 저장
        mapdata[y, x] = Tile;
      }
    }

    // Object Layer 데이터 파싱
    XmlNodeList objectNodes = xmlMapData.SelectNodes("//objectgroup/object");

    foreach (XmlNode obj in objectNodes)
    {
      // gid: 오브젝트 타입 구분용
      int gid = int.Parse(obj.Attributes["gid"].Value) - 1;

      // 위치 값: 픽셀 단위 → 타일 단위로 변환 (타일 크기 32)
      float x = float.Parse(obj.Attributes["x"].Value);
      float y = float.Parse(obj.Attributes["y"].Value);

      int tileX = Mathf.FloorToInt(x / 32f);
      int tileY = Mathf.FloorToInt(y / 32f) - 1;

      Debug.Log($"GID: {gid}, 위치: ({tileY}, {tileX})");
      int voltage = 0;

      // properties가 있다면 추가 속성 가져오기
      XmlNode propertiesNode = obj.SelectSingleNode("properties");
      if (propertiesNode != null)
      {
        foreach (XmlNode prop in propertiesNode.SelectNodes("property"))
        {
          string propName = prop.Attributes["name"].Value;
          string propValue = prop.Attributes["value"].Value;
          Debug.Log($"  - {propName}: {propValue}");

          if (propName == "voltage")
          {
            // 전압 정보가 있다면 int로 변환
            if (int.TryParse(propValue, out voltage))
            {
              Debug.Log($"전압 정보: {voltage}");
            }
            else
            {
              Debug.LogWarning($"전압 정보 변환 실패: {propValue}");
            }
          }
        }
      }

      // 8: player
      // 9 : enemy
      // 17 : vdd
      // 18 : res
      // 19 : cap
      // 20 : diode
      // 21 : fuse
      // 22 : ind
      // 23 : semi

      SimpleRoad Tile = null;

      if (gid == 8)
      {
        Tile = Instantiate(CreateTile<SimpleRoad>()).GetComponent<SimpleRoad>();
        Tile.Init(mapdata[tileY, tileX].directions, isStart: true);
        if (Tile == null)
        {
          Debug.LogError("SimpleRoad 컴포넌트를 찾을 수 없습니다.");
          continue;
        }
      }
      else if (gid == 9)
      {
        Tile = Instantiate(CreateTile<EnemyTile>()).GetComponent<EnemyTile>();
        if (Tile == null)
        {
          Debug.LogError("EnemyTile 컴포넌트를 찾을 수 없습니다.");
          continue;
        }
        EnemyTile.enemyCount++; // EnemyTile이 생성될 때마다 enemyCount 증가
        Tile.Init(mapdata[tileY, tileX].directions, hasInteraction: true);
      }
      else if (gid == 17)
      {
        Tile = Instantiate(CreateTile<VddTile>()).GetComponent<VddTile>();
        Tile.Init(mapdata[tileY, tileX].directions);
      }
      else if (gid == 18)
      {
        Tile = Instantiate(CreateTile<ResTile>()).GetComponent<ResTile>();
        Tile.Init(mapdata[tileY, tileX].directions);
      }
      else if (gid == 19)
      {
        Tile = Instantiate(CreateTile<CapTile>()).GetComponent<CapTile>();
        Tile.Init(mapdata[tileY, tileX].directions);
      }
      else if (gid == 20)
      {
        Tile = Instantiate(CreateTile<DiodeTile>()).GetComponent<DiodeTile>();
        Tile.Init(mapdata[tileY, tileX].directions);
        if (Tile is DiodeTile diodeTile)
          diodeTile.SetDiodeDirection('r'); // 첫 번째 방향을 다이오드 방향으로 설정
      }
      else if (gid == 21)
      {
        Tile = Instantiate(CreateTile<FuseTile>()).GetComponent<FuseTile>();
        Tile.Init(mapdata[tileY, tileX].directions);
      }
      else if (gid == 22)
      {
        Tile = Instantiate(CreateTile<IndTile>()).GetComponent<IndTile>();
        Tile.Init(mapdata[tileY, tileX].directions);
      }
      else if (gid == 23)
      {
        Tile = Instantiate(CreateTile<SemiTile>()).GetComponent<SemiTile>();
        Tile.Init(mapdata[tileY, tileX].directions);
      }

      if (Tile is IHasVoltage voltageTile)
      {
        // properties가 있다면 전압 정보 설정
        if (propertiesNode != null)
        {
          voltageTile.SetVoltage(voltage);
        }
        else
        {
          // 전압 정보가 없으면 기본값 설정
          voltageTile.SetVoltage(0);
        }
      }


      mapdata[tileY, tileX] = Tile;

    }


  }

  private void TestLoadMap()
  {
    // // 테스트용으로 mapdata를 하드코딩
    // this.mapdata = new Tile[,] {
    //   {TILE_EM, TILE_DR, TILE_LR, TILE_LR, TILE_DL, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
    //   {TILE_EM, TILE_UD, TILE_EM, TILE_EM, TILE_UD, TILE_EM, TILE_EM, TILE_ED_D, TILE_EM, TILE_EM},
    //   {TILE_ST, TILE_UL, TILE_EM, TILE_DR, TILE_CROS, TILE_DIEOD_LR_R, TILE_LR, TILE_ULR, TILE_RES_LR, TILE_ED_L},
    //   {TILE_EM, TILE_EM, TILE_EM, TILE_UR, TILE_UDL, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
    //   {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED_U, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM}
    // };

    // // 테스트용으로 mapdata를 하드코딩
    // this.gadgetmap = new int[,] {
    //   {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
    //   {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED, TILE_EM, TILE_EM},
    //   {TILE_ST, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED},
    //   {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
    //   {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM}
    // };
  }

  private void createMapObjects()
  {
    // // 맵 오브젝트 생성
    // for (int i = 0; i < mapdata.GetLength(0); i++)
    // {
    //   for (int j = 0; j < mapdata.GetLength(1); j++)
    //   {
    //     Tile tile = mapdata[i, j];
    //     if (tile != null)
    //     {
    //       GameObject tileObject = Instantiate(tile.gameObject, new Vector3(j, -i, 0), Quaternion.identity);
    //       tileObject.name = $"Tile_{i}_{j}";
    //       tileObject.transform.SetParent(this.transform);
    //     }
    //   }
    // }
  }
}

#endif