using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
  public MapLoader map;
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
    List<char> path = new List<char>(); // Note: null로 초기화하고 path.Add 전에 검사하면 메모리 절약됨

    if (depth > 100)
    {
      Debug.Log("Error: too deep recursion in Player.Move");
      return path; // stop
    }

    if (this.NextDir(dir) == 's') // stop
    {
      return path; // stop
    }

    path = this.Move(this.NextDir(dir), depth + 1);
    path.Add(dir);

    if (depth == 0)
    {
      path.Reverse();
    }

    return path;
  }

  private char NextDir(char dir)
  {
    int nextX;
    int nextY;
    switch (dir) // nextX, nextY 계산
    {
      case 'u':
        nextX = this.x + 0;
        nextY = this.y - 1;
        break;
      case 'd':
        nextX = this.x + 0;
        nextY = this.y + 1;
        break;
      case 'l':
        nextX = this.x - 1;
        nextY = this.y + 0;
        break;
      case 'r':
        nextX = this.x + 1;
        nextY = this.y + 0;
        break;
      default:
        Debug.Log("Error: invalid direction in Player.NextDir");
        return 's'; // stop
    }
    int nextTile = this.map.mapdata[nextY, nextX] - 10000;
    if (nextTile / 1000 != 2)
    {
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
}
