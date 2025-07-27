using UnityEngine;
using UnityEngine.Rendering;

public class SemiTile : SimpleRoad, IHasVoltage
{
  public int voltage; // 전압을 나타내는 변수
  private char lastDir = 's'; // 마지막으로 온 방향을 저장하는 변수

  public override void Init(char[] directions, bool isStart = false, bool hasInteraction = false)
  {
    this.directions = directions;
    this.isStart = false;
    this.isStop = false;
    this.hasInteraction = false;
  }

  public void SetVoltage(int voltage = 0)
  {
    this.voltage = voltage; // 전압을 설정하는 메서드
  }

  void Start()
  {
    // 초기화 작업이 필요할 경우 여기에 작성합니다.
  }

  public override bool canGo(char dir)
  {
    if (this.lastDir == dir)
    {
      return true; // 마지막 방향과 같은 방향으로 갈 수 있습니다.
    }
    else
    if (this.lastDir == this.OppositeDir(dir))
    {
      return true; // 마지막 방향과 반대 방향으로 갈 수 있습니다.
    }
    return false;
  }

  public override char NextDir(char dir, int v)
  {
    if (v >= this.voltage)
    {
      // 전압이 충분하면 다음 방향을 반환합니다.
      return base.NextDir(dir, v);
    }
    else
    {
      this.lastDir = dir; // 전압이 부족하면 마지막 방향을 저장합니다.
      return 's';
    }
  }
}