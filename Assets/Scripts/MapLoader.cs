using UnityEngine;

public class MapLoader : MonoBehaviour
{
  public int[,] mapdata = new int[5, 10]; //나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게
  public int[,] gadgetmap = new int[5, 10]; //나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게

  public char[] TILE_DIRECTIONS = new char[] { ' ', ' ', 'u', 'd', ' ', 'l', ' ', 'r' };
  public const int TILE_EM = 0; // 빈 타일
                                // 10000+n+t: 길, n개 연결되어있음
                                // t: u: 2, d: 3, l: 5, r: 7중 연결된 것들의 곱
  public const int TILE_ST = -1; // start tile
  public const int TILE_ED = -2; // end tile

  public const int TILE_U = 11002; // 위 연결 (2)
  public const int TILE_D = 11003; // 위 아래 연결 (3)
  public const int TILE_L = 11005; // 왼쪽 연결 (5)
  public const int TILE_R = 11007; // 오른쪽 연결 (7)
  public const int TILE_UD = 12006; // 위 아래 연결 (2*3)
  public const int TILE_UL = 12010; // 위 왼쪽 연결 (2*5)
  public const int TILE_UR = 12014; // 위 오른쪽 연결 (2*7)
  public const int TILE_DL = 12015; // 아래 왼쪽 연결 (3*5)
  public const int TILE_DR = 12021; // 아래 오른쪽 연결 (3*7)
  public const int TILE_LR = 12035; // 왼쪽 오른쪽 연결 (5*7)

  public const int TILE_CROS = 12210;

  public const int TILE_LRUD = 14210; // 왼쪽 오른쪽 위 아래 연결 (2*3*5*7)
  public const int TILE_ULR = 13070; // 위 왼쪽 오른쪽 연결 (2*5*7)
  public const int TILE_DLR = 13105; // 아래 왼쪽 오른쪽 연결 (3*5*7)
  public const int TILE_UDL = 13030; // 위 아래 왼쪽 연결 (2*3*5)
  public const int TILE_UDR = 13042; // 위 아래 오른쪽 연결 (2*3*7)

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    // LoadMap(); // 실제 맵 로딩
    TestLoadMap();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.A))
    {

    }
  }


  private void LoadMap()
  {
    // 스테이지 씬 시작될 때 MapLoader 오브젝트(= 이 코드 mapdata)에 tiled 저장하기 
  }

  private void TestLoadMap()
  {
    // 테스트용으로 mapdata를 하드코딩
    this.mapdata = new int[,] {
      {TILE_EM, TILE_DR, TILE_LR, TILE_LR, TILE_DL, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
      {TILE_EM, TILE_UD, TILE_EM, TILE_EM, TILE_UD, TILE_EM, TILE_EM, TILE_D, TILE_EM, TILE_EM},
      {TILE_R, TILE_UL, TILE_EM, TILE_DR, TILE_CROS, TILE_LR, TILE_LR, TILE_ULR, TILE_LR, TILE_L},
      {TILE_EM, TILE_EM, TILE_EM, TILE_UR, TILE_UDL, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
      {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_U, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM}
    };
    
    // 테스트용으로 mapdata를 하드코딩
    this.gadgetmap = new int[,] {
      {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
      {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED, TILE_EM, TILE_EM},
      {TILE_ST, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED},
      {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM},
      {TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_ED, TILE_EM, TILE_EM, TILE_EM, TILE_EM, TILE_EM}
    };
  }
}
