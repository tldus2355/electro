using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
  public static State Instance;
  public Player player;
  public MapLoader map;

  void Awake()
  {
    // singleton game manager가 아니라서 싱글톤 아닐수도. 씬마다 따로, 나중에 수정하기
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    // TODO: 플레이어가 자기 위치 찾은 이후에 실행: 플레이어 위치 화면에 표시하기
  }

  // Update is called once per frame
  void Update()
  {
    List<char> path = new List<char>();
    if (!this.player.isMoving)
    {
      if (Input.GetKeyDown(KeyCode.UpArrow)) //TODO: 버튼 두개 동시에 입력받는경우 해결, 금지된 입력 처리
      {
        if (!this.PlayerCanGo('u'))
        {
          Debug.Log("Error: can't go up");
          return;
        }
        path = player.Move('u', 0);
      }
      if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        if (!this.PlayerCanGo('d'))
        {
          Debug.Log("Error: can't go down");
          return;
        }
        path = player.Move('d', 0);
      }
      if (Input.GetKeyDown(KeyCode.LeftArrow))
      {
        if (!this.PlayerCanGo('l'))
        {
          Debug.Log("Error: can't go left");
          return;
        }
        path = player.Move('l', 0);
      }
      if (Input.GetKeyDown(KeyCode.RightArrow))
      {
        if (!this.PlayerCanGo('r'))
        {
          Debug.Log("Error: can't go right");
          return;
        }
        path = player.Move('r', 0);
      }
      if (this.player.isMoving)
      {
        Debug.Log("Path: " + string.Join(", ", path));
        this.PlayerMoveAnimation(path);
        this.CheckObject(); // 플레이어가 이동한 후에 오브젝트 체크
        this.player.isMoving = false; // 애니메이션이 끝나면 다시 false로 설정
      }
    }
  }

  private void PlayerMoveAnimation(List<char> path)
  {
    foreach (char dir in path)
    {
      // 경로에 기본 사각형 스프라이트 표시 TODO: 나중에 애니메이션으로 바꾸기
      GameObject marker = new GameObject("PathMarker");
      marker.transform.position = player.transform.position;
      var sr = marker.AddComponent<SpriteRenderer>();
      sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
      sr.color = Color.yellow; // 원하는 색상
      marker.transform.localScale = new Vector3(50f, 50f, 1); // 크기 조절
      switch (dir)
      {
        case 'u': player.transform.Translate(0, 1, 0); break;
        case 'd': player.transform.Translate(0, -1, 0); break;
        case 'l': player.transform.Translate(-1, 0, 0); break;
        case 'r': player.transform.Translate(1, 0, 0); break;
      }
    }
    // player.isMoving = false;
  }

  private bool PlayerCanGo(char dir)
  {
    var pos = player.GetPosition();
    int px = pos.x, py = pos.y;

    Tile currentTile = map.mapdata[py, px];

    return currentTile.canGo(dir);
    // Debug.Log($"Current Tile: {currentTile} at ({px}, {py})");

    // if (currentTile == MapLoader.TILE_EM) // 빈 타일
    // {
    //   Debug.Log("Error: current tile is empty");
    //   return false;
    // }
    // switch (dir)
    // {
    //   case 'u':
    //     if (currentTile % 2 == 0) return true; // 위로 갈 수 있는지 확인
    //     break;
    //   case 'd':
    //     if (currentTile % 3 == 0) return true; // 아래로 갈 수 있는지 확인
    //     break;
    //   case 'l':
    //     if (currentTile % 5 == 0) return true; // 왼쪽으로 갈 수 있는지 확인
    //     break;
    //   case 'r':
    //     if (currentTile % 7 == 0) return true; // 오른쪽으로 갈 수 있는지 확인
    //     break;
    //   default:
    //     Debug.Log("Error: invalid direction in PlayerCanGo");
    //     return false; // 잘못된 방향
    // }

    // return false; // 해당 방향으로 갈 수 없음
  }

  private void CheckObject()
  {
    var pos = player.GetPosition();
    int px = pos.x, py = pos.y;
    Tile currentTile = map.mapdata[py, px];

    if(currentTile.hasInteraction)
    {
      // 타일과 상호작용하는 로직을 여기에 작성합니다.
      Debug.Log("Interacting with tile at (" + px + ", " + py + ")");
      if (currentTile is ResTile resTile)
      {
        resTile.interaction(); // ResTile의 interaction 메서드 호출
        player.addVoltage(-1 * resTile.voltage); // 플레이어의 전압 감소
      }
      else if (currentTile is FuseTile fuseTile)
      {
        fuseTile.interaction(); // FuseTile의 interaction 메서드 호출
        if(this.player.voltage >= fuseTile.voltage)
        {
          Debug.Log("Error: Not enough voltage to interact with FuseTile");
          //this.gameover();
          return; // 전압이 부족하면 상호작용하지 않음
        }
      }
      else
      {
        Debug.Log("No interaction defined for this tile type.");
      }
    }
    else
    {
      Debug.Log("No interaction available on this tile.");
    }


    return;
  }
}
