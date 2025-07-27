using UnityEngine;

public class CrossRoad : SimpleRoad
{
  public override void Init(char[] directions, bool isStart = false, bool hasInteraction = false)
  {
    // 생성자에서 방향을 초기화합니다.
    this.directions = new char[] { 'u', 'd', 'l', 'r' };
    this.isStop = false; // CrossLoad는 정지 타일이 아닙니다.
    this.isStart = false; // CrossLoad는 시작 타일이 아닙니다.
    this.hasInteraction = false; // CrossLoad는 상호작용이 없습니다.
  }

  void Start()
  {
    
  }
  
  public override bool canGo(char dir)
  {
    Debug.Log("CrossLoad.canGo called with dir: " + dir);
    return false; // 호출되면 안됨
  }

  public override char NextDir(char dir, int v = 0)
  {
    // cross의 경우에는 방향을 바꾸지 않음
    return dir;
  }
}