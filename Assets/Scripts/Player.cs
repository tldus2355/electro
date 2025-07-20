using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
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

    for (int i = 0; i < this.mapy; i++)
    {
      for (int j = 0; j < this.mapx; j++)
      {
        if (map.mapdata[i, j] == -1)
        {
          this.y = i;
          this.x = j;
        }
      }
    }
    if (this.x == -1)
    {
      Debug.Log("Error: can't find start tile");
      Debug.Log("from Elecrto.cs");
    }
  }

  // Update is called once per frame
  void Update()
  {

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
    int nextTile = this.map.mapdata[this.y, this.x];
    if (nextTile == MapLoader.TILE_ED)
    {
      Debug.Log("end tile reached");
      return 's'; // stop
    }
    if (nextTile / 10000 == 1) // 길 타일
    {
      nextTile -= 10000;
      if (nextTile / 1000 != 2)
      {
        Debug.Log(nextTile + " ******************* ");
        Debug.Log("Not two-way tile, returning stop");
        return 's'; // stop
      }
      else
      {
        nextTile -= 2000;
        switch (dir)
        {
          case 'u':
            return this.map.TILE_DIRECTIONS[nextTile / 3]; // 아래로 연결된 타일
          case 'd':
            return this.map.TILE_DIRECTIONS[nextTile / 2]; // 위로 연결된 타일
          case 'l':
            return this.map.TILE_DIRECTIONS[nextTile / 7]; // 오른쪽으로 연결된 타일
          case 'r':
            return this.map.TILE_DIRECTIONS[nextTile / 5]; // 왼쪽으로 연결된 타일
          default:
            Debug.Log("Error: invalid direction in Player.NextDir");
            return 's'; // stop
        }
      }
    }
    else
    {
      return 's'; // stop
    }
  }
}
