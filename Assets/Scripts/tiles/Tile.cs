using UnityEngine;

public abstract class Tile : MonoBehaviour
{
  public char[] direction; // 타일의 방향을 나타내는 문자 ('u', 'd', 'l', 'r' 등)
  public bool isStop = true; // 타일이 정지 타일인지 여부
  public bool isStart = false; // 시작 타일인지 여부
  public bool hasInteraction = false; // 상호작용 여부

  public virtual bool canGo(char dir)
  {
    // 타일이 해당 방향으로 이동할 수 있는지 확인
    return System.Array.Exists(this.direction, d => d == dir);
  }

  public virtual char NextDir(char dir)
  {
    if(this.isStop)
    {
      Debug.Log("Error: Cannot move from a stop tile in Tile.NextDir");
      return 's'; // 정지 타일에서 이동할 수 없음
    }
    // direction 배열에 두 개만 있다고 가정하고, dir의 반대 방향(u <-> d, l <-> r)과 다른 방향을 반환
    foreach (char d in this.direction)
    {
      if (d != this.OppositeDir(dir))
      {
        return d; // 유효한 다음 방향을 찾으면 반환
      }
    }
    
    Debug.Log("Error: No valid next direction found in Tile.NextDir");
    return 's'; // 유효한 방향이 없을 경우 's'를 반환
  }

  public char OppositeDir(char dir)
  {
    // 주어진 방향의 반대 방향을 반환
    switch (dir)
    {
      case 'u': return 'd';
      case 'd': return 'u';
      case 'l': return 'r';
      case 'r': return 'l';
      default:
        Debug.Log("Error: Invalid direction in Tile.OppositeDir");
        return 's'; // 유효하지 않은 방향일 경우 's'를 반환
    }
  }
}