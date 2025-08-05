using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public int voltage = 0;
  public MapLoader map;
  public bool isMoving = false; // 플레이어가 움직이는 중인지 여부
  private int x = -1;
  private int y = -1;
  private int mapx;
  private int mapy;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    this.mapy = map.mapdata.GetLength(0); //TODO: map 로드이후에 실행이 보장되게 해야 함
    this.mapx = map.mapdata.GetLength(1);

    Debug.Log($"맵 불러오기 성공? (y, x): {mapy}, {mapx}");

    for (int i = 0; i < this.mapy; i++)
    {
      for (int j = 0; j < this.mapx; j++)
      {
        if (map.mapdata[i, j] == null) continue;
        if (map.mapdata[i, j].isStart)
        {
          this.y = i;
          this.x = j;
        }
      }
    }
    if (this.x == -1)
    {
      Debug.Log("Error: can't find start tile");
      Debug.Log("from Player.cs");
    }

    this.SetVoltage();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void MoveAnimation(List<char> path)
  {
    foreach (char dir in path)
    {
      // 경로에 기본 사각형 스프라이트 표시 TODO: 나중에 애니메이션으로 바꾸기
      GameObject marker = new GameObject("PathMarker");
      marker.transform.position = this.transform.position;
      var sr = marker.AddComponent<SpriteRenderer>();
      sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
      sr.color = Color.yellow; // 원하는 색상
      marker.transform.localScale = new Vector3(50f, 50f, 1); // 크기 조절
      switch (dir)
      {
        case 'u': this.transform.Translate(0, 1, 0); break;
        case 'd': this.transform.Translate(0, -1, 0); break;
        case 'l': this.transform.Translate(-1, 0, 0); break;
        case 'r': this.transform.Translate(1, 0, 0); break;
      }
    }
    // player.isMoving = false;
  }
  
  public bool CanGo(char dir)
  {
    SimpleRoad currentTile = map.mapdata[this.y, this.x];
    return currentTile.canGo(dir);
  }
  public List<char> Move(char dir, int depth) //유효한 방향인지는 state.cs에서 체크
  {
    Debug.Log("Player.Move called with dir: " + dir + ", depth: " + depth);
    this.isMoving = true;
    List<char> path = new List<char>(); // Note: null로 초기화하고 path.Add 전에 검사하면 메모리 절약됨

    if (depth > 100)
    {
      Debug.Log("Error: too deep recursion in Player.Move");
      return path; // stop
    }

    if (dir == 's') // stop
    {
      return path; // stop
    }

    if (dir == 'f')
    {
      Debug.Log("fuse over");
      path.Add('f'); // stop
      return path;
    }

    // 다음 타일로 이동
    switch (dir)
    {
      case 'u':
        this.y -= 1;
        break;
      case 'd':
        this.y += 1;
        break;
      case 'l':
        this.x -= 1;
        break;
      case 'r':
        this.x += 1;
        break;
      default:
        Debug.Log("Error: invalid direction in Player.Move");
        return path; // stop
    }

    path = this.Move(this.NextDir(dir), depth + 1);
    path.Add(dir);

    if (depth == 0)
    {
      path.Reverse();
      // this.isMoving = false;
    }

    return path;
  }

  private char NextDir(char dir)
  {
    SimpleRoad nextTile = this.map.mapdata[this.y, this.x];
    // int nextgadget = this.map.gadgetmap[this.y, this.x];
    if (nextTile.isStop)
    {
      Debug.Log("Error: next tile is stop tile");
      return 's'; // stop
    }
    return nextTile.NextDir(dir, this.voltage);
  }

  public void SetVoltage(int voltage = 0)
  {
    this.voltage = voltage; // 플레이어의 전압을 설정
    Debug.Log("Player voltage set to: " + this.voltage);
  }

  public (int x, int y) GetPosition()
  {
    return (this.x, this.y);
  }

  public void addVoltage(int v)
  {
    this.voltage += v;
    Debug.Log("Player voltage: " + this.voltage);
  }
}
