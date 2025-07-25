using UnityEngine;

public class MapLoader : MonoBehaviour
{
  public Tile[,] mapdata = new Tile[5, 10]; //나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게
  public int[,] gadgetmap = new int[5, 10]; //나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게

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

  public static readonly Tile TILE_EM = new EmptyTile();

  public static readonly Tile TILE_U = new SimpleLoad(new char[] { 'u' });
  public static readonly Tile TILE_D = new SimpleLoad(new char[] { 'd' });
  public static readonly Tile TILE_L = new SimpleLoad(new char[] { 'l' });
  public static readonly Tile TILE_R = new SimpleLoad(new char[] { 'r' });
  public static readonly Tile TILE_UD = new SimpleLoad(new char[] { 'u', 'd' }, isStop: false);
  public static readonly Tile TILE_UL = new SimpleLoad(new char[] { 'u', 'l' }, isStop: false);
  public static readonly Tile TILE_UR = new SimpleLoad(new char[] { 'u', 'r' }, isStop: false);
  public static readonly Tile TILE_DL = new SimpleLoad(new char[] { 'd', 'l' }, isStop: false);
  public static readonly Tile TILE_DR = new SimpleLoad(new char[] { 'd', 'r' }, isStop: false);
  public static readonly Tile TILE_LR = new SimpleLoad(new char[] { 'l', 'r' }, isStop: false);
  public static readonly Tile TILE_ULR = new SimpleLoad(new char[] { 'u', 'l', 'r' });
  public static readonly Tile TILE_DLR = new SimpleLoad(new char[] { 'd', 'l', 'r' });
  public static readonly Tile TILE_UDL = new SimpleLoad(new char[] { 'u', 'd', 'l' });
  public static readonly Tile TILE_UDR = new SimpleLoad(new char[] { 'u', 'd', 'r' });
  public static readonly Tile TILE_CROS = new CrossLoad();
  public static readonly Tile TILE_LRUD = new SimpleLoad(new char[] { 'u', 'd', 'l', 'r' });

  public static readonly Tile TILE_ST = new SimpleLoad(new char[] { 'r' }, isStart: true); // 시작 타일, 테스트용
  public static readonly Tile TILE_ED_L = new SimpleLoad(new char[] { 'l' }, isStop: true); // 끝 타일, 테스트용 
  public static readonly Tile TILE_ED_R = new SimpleLoad(new char[] { 'r' }, isStop: true); // 끝 타일, 테스트용
  public static readonly Tile TILE_ED_D = new SimpleLoad(new char[] { 'd' }, isStop: true); // 끝 타일, 테스트용
  public static readonly Tile TILE_ED_U = new SimpleLoad(new char[] { 'u' }, isStop: true); // 끝 타일, 테스트용

  public static readonly Tile TILE_RES_LR = new ResTile(new char[] { 'l', 'r' }, voltage: 10); // 왼쪽 오른쪽 연결, 전압 10



  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    // LoadMap(); // 실제 맵 로딩
    TestLoadMap();
    // createMapObjects(); // 맵 오브젝트 생성
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.A))
    {

    }
  }


  private void LoadMapData()
  {
    // 스테이지 씬 시작될 때 MapLoader 오브젝트(= 이 코드 mapdata)에 tiled 저장하기 
    foreach (var tile in mapdata)
    {
      foreach (var direction in tile.direction)
      {
        Debug.Log($"Tile: {tile.name}, Direction: {direction}");
      }
    }
  }

  private void TestLoadMap()
  {
    // 테스트용으로 mapdata를 하드코딩
    this.mapdata = new Tile[,] {
      {TILE_EM, TILE_DR, TILE_LR, TILE_LR, TILE_DL, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
      {TILE_EM, TILE_UD, TILE_EM, TILE_EM, TILE_UD, TILE_EM, TILE_EM, TILE_ED_D, TILE_EM, TILE_EM},
      {TILE_ST, TILE_UL, TILE_EM, TILE_DR, TILE_CROS, TILE_LR, TILE_LR, TILE_ULR, TILE_RES_LR, TILE_ED_L},
      {TILE_EM, TILE_EM, TILE_EM, TILE_UR, TILE_UDL, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
      {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED_U, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM}
    };

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
