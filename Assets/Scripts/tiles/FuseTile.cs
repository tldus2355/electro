using UnityEngine;

public class FuseTile : SimpleRoad, IHasVoltage
{
  public int voltage; // 전압을 나타내는 변수

  public override void Init(char[] directions, bool isStart = false, bool hasInteraction = false)
  {
    // 생성자에서 방향과 시작 여부를 초기화합니다.
    this.directions = directions;
    this.isStart = false; // FuseTile은 시작 타일이 아닙니다
    this.isStop = true; // FuseTile은 정지 타일입니다.
    this.hasInteraction = false; // FuseTile은 상호작용이 가능합니다.
  }

  public void SetVoltage(int voltage)
  {
    this.voltage = voltage; // 전압을 설정하는 메서드
    Debug.Log("FuseTile voltage set to: " + voltage);
  }

  void Start()
  {

  }
  
  public override char NextDir(char dir, int v = 0)
  {
    // FuseTile은 전압이 0일 때만 작동합니다.
    if (v <= this.voltage)
    {
      return base.NextDir(dir, v); // 기본 동작을 수행합니다.
    }
    else
    {
      return 'f';
    }
  }
}