using UnityEngine;
using UnityEngine.Rendering;

public class CapTile : SimpleRoad, IHasInteraction, IHasVoltage
{
  public int voltage; // 전압을 나타내는 변수
  public bool isUsed = false; // VddTile이 사용되었는지 여부를 나타내는 변수

  public override void Init(char[] directions, bool isStart = false, bool hasInteraction = false)
  {
    // 생성자에서 방향과 시작 여부를 초기화합니다.
    this.directions = directions;
    this.isStart = false;
    this.isStop = true; // ResTile은 정지 타일입니다.
    this.hasInteraction = true; // ResTile은 상호작용이 가능합니다.
    this.isUsed = false; // 초기화 시 사용되지 않은 상태로 설정
  }

  public void SetVoltage(int voltage = 0)
  {
    this.voltage = voltage; // 전압을 설정하는 메서드
  }

  void Start()
  {
    // 초기화 작업이 필요할 경우 여기에 작성합니다.
  }

  public void interaction()
  {
    // 타일과 상호작용하는 로직을 여기에 작성합니다.
    if (!isUsed && voltage == 0)
    {
      
    }
    else if (!isUsed)
    {
    }
    else
    {
      Debug.Log("CapTile has already been used.");
    }
  }
}