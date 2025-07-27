using UnityEngine;

public class DiodeTile : SimpleRoad
{
  public char diodeDirection;
  public override void Init(char[] directions, bool isStart = false, bool hasInteraction = false)
  {
    // 생성자에서 방향과 다이오드 방향을 초기화합니다.
    this.directions = directions;
    this.isStart = isStart; // DiodeTile은 시작 타일 여부를 설정합니다.
    this.isStop = true; // DiodeTile은 정지 타일입니다.
    this.hasInteraction = false; // DiodeTile은 상호작용이 없습니다.
    // this.diodeDirection = directions[0]; // 첫 번째 방향을 다이오드 방향으로 설정
  }

  public void SetDiodeDirection(char direction)
  {
    this.diodeDirection = direction; // 다이오드 방향을 설정하는 메서드
    // Debug.Log("DiodeTile diodeDirection set to: " + direction);
  }

  void Start()
  {
    // 초기화 작업이 필요하다면 여기에 작성합니다.
  }
  public override bool canGo(char dir)
  {
    // 타일이 해당 방향으로 이동할 수 있는지 확인
    return dir == this.diodeDirection;
  }

  public override char NextDir(char dir, int v = 0)
  {
    Debug.Log("DIODE*****************************************");
    if (this.isStop)
    {
      Debug.Log("Error: Cannot move from a stop tile in Tile.NextDir");
      return 's'; // 정지 타일에서 이동할 수 없음
    }
    // direction 배열에 두 개만 있다고 가정하고, dir의 반대 방향(u <-> d, l <-> r)과 다른 방향을 반환
    foreach (char d in this.directions)
    {
      if (d != this.OppositeDir(dir) && d == this.diodeDirection)
      {
        return d; // 유효한 다음 방향을 찾으면 반환
      }
    }

    return 's'; // 유효한 방향이 없을 경우 's'를 반환
  }
}