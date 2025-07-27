using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
  public SimpleRoad[,] mapdata;//나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게
  public GameObject SimpleRoadPrefab;

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

        char[] RoadDirections = {};
        // tileId에 따라 directions 설정
        switch (actualTileId)
        {
          case -1: continue;
          case 10: RoadDirections = new char[] { 'l', 'r' }; break;
          case 11: RoadDirections = new char[] { 'u', 'r' }; break;
          case 12: RoadDirections = new char[] { 'l', 'u', 'r' }; break;
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
      int tileY = Mathf.FloorToInt(y / 32f)-1;

      Debug.Log($"GID: {gid}, 위치: ({tileY}, {tileX})");

      // properties가 있다면 추가 속성 가져오기
      XmlNode propertiesNode = obj.SelectSingleNode("properties");
      if (propertiesNode != null)
      {
        foreach (XmlNode prop in propertiesNode.SelectNodes("property"))
        {
          string propName = prop.Attributes["name"].Value;
          string propValue = prop.Attributes["value"].Value;
          Debug.Log($"  - {propName}: {propValue}");
        }
      }

      if (gid == 8)
      {
        SimpleRoad Tile = Instantiate(SimpleRoadPrefab).GetComponent<SimpleRoad>();
        Tile.Init(mapdata[tileY, tileX].directions, true);
        mapdata[tileY, tileX] = Tile;
      }
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
