using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
  public SimpleRoad[,] mapdata;//나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게
  public GameObject SimpleRoadPrefab;

  public char[] TILE_DIRECTIONS = new char[] { ' ', ' ', 'u', 'd', ' ', 'l', ' ', 'r' };
  // public const int TILE_EM = 0; // 빈 타일
  //                               // 10000+n+t: 길, n개 연결되어있음
  //                               // t: u: 2, d: 3, l: 5, r: 7중 연결된 것들의 곱
  // public const int TILE_ST = -1; // start tile
  // public const int TILE_ED = -2; // end tile

  // public const int TILE_U = 11002; // 위 연결 (2)
  // public const int TILE_D = 11003; // 위 아래 연결 (3)
  // public const int TILE_L = 11005; // 왼쪽 연결 (5)
  // public const int TILE_R = 11007; // 오른쪽 연결 (7)
  // public const int TILE_UD = 12006; // 위 아래 연결 (2*3)
  // public const int TILE_UL = 12010; // 위 왼쪽 연결 (2*5)
  // public const int TILE_UR = 12014; // 위 오른쪽 연결 (2*7)
  // public const int TILE_DL = 12015; // 아래 왼쪽 연결 (3*5)
  // public const int TILE_DR = 12021; // 아래 오른쪽 연결 (3*7)
  // public const int TILE_LR = 12035; // 왼쪽 오른쪽 연결 (5*7)
  // public const int TILE_ULR = 13070; // 위 왼쪽 오른쪽 연결 (2*5*7)
  // public const int TILE_DLR = 13105; // 아래 왼쪽 오른쪽 연결 (3*5*7)
  // public const int TILE_UDL = 13030; // 위 아래 왼쪽 연결 (2*3*5)
  // public const int TILE_UDR = 13042; // 위 아래 오른쪽 연결 (2*3*7)

  // public const int TILE_CROS = 12210;

  // public const int TILE_LRUD = 14210; // 왼쪽 오른쪽 위 아래 연결 (2*3*5*7)

  // public static readonly Tile TILE_EM = new EmptyTile();

  // public static readonly Tile TILE_U = new SimpleLoad(new char[] { 'u' });
  // public static readonly Tile TILE_D = new SimpleLoad(new char[] { 'd' });
  // public static readonly Tile TILE_L = new SimpleLoad(new char[] { 'l' });
  // public static readonly Tile TILE_R = new SimpleLoad(new char[] { 'r' });
  // public static readonly Tile TILE_UD = new SimpleLoad(new char[] { 'u', 'd' }, isStop: false);
  // public static readonly Tile TILE_UL = new SimpleLoad(new char[] { 'u', 'l' }, isStop: false);
  // public static readonly Tile TILE_UR = new SimpleLoad(new char[] { 'u', 'r' }, isStop: false);
  // public static readonly Tile TILE_DL = new SimpleLoad(new char[] { 'd', 'l' }, isStop: false);
  // public static readonly Tile TILE_DR = new SimpleLoad(new char[] { 'd', 'r' }, isStop: false);
  // public static readonly Tile TILE_LR = new SimpleLoad(new char[] { 'l', 'r' }, isStop: false);
  // public static readonly Tile TILE_ULR = new SimpleLoad(new char[] { 'u', 'l', 'r' });
  // public static readonly Tile TILE_DLR = new SimpleLoad(new char[] { 'd', 'l', 'r' });
  // public static readonly Tile TILE_UDL = new SimpleLoad(new char[] { 'u', 'd', 'l' });
  // public static readonly Tile TILE_UDR = new SimpleLoad(new char[] { 'u', 'd', 'r' });
  // public static readonly Tile TILE_CROS = new CrossLoad();
  // public static readonly Tile TILE_LRUD = new SimpleLoad(new char[] { 'u', 'd', 'l', 'r' });

  // public static readonly Tile TILE_ST = new SimpleLoad(new char[] { 'r' }, isStart: true); // 시작 타일, 테스트용
  // public static readonly Tile TILE_ED_L = new SimpleLoad(new char[] { 'l' }, isStop: true); // 끝 타일, 테스트용 
  // public static readonly Tile TILE_ED_R = new SimpleLoad(new char[] { 'r' }, isStop: true); // 끝 타일, 테스트용
  // public static readonly Tile TILE_ED_D = new SimpleLoad(new char[] { 'd' }, isStop: true); // 끝 타일, 테스트용
  // public static readonly Tile TILE_ED_U = new SimpleLoad(new char[] { 'u' }, isStop: true); // 끝 타일, 테스트용

  // public static readonly Tile TILE_RES_LR = new ResTile(new char[] { 'l', 'r' }, voltage: 10); // 왼쪽 오른쪽 연결, 전압 10

  // public static readonly Tile TILE_DIEOD_LR_R = new DiodeTile(new char[] { 'l', 'r' }, diodeDirection: 'r'); // 왼쪽 오른쪽 연결, 전압 5

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
