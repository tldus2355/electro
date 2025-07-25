using UnityEngine;

public class ResTile : Tile
{
  public int voltage; // 전압을 나타내는 변수

  public ResTile(char[] directions, int voltage, bool isStart = false, bool isStop = true)
  {
    // 생성자에서 방향과 시작 여부를 초기화합니다.
    this.direction = directions;
    this.isStart = isStart;
    this.isStop = isStop;
    this.voltage = voltage; // 전압을 설정합니다.
    this.hasInteraction = true; // ResTile은 상호작용이 가능한 타일입니다.
  }

  void Start()
  {
    // 초기화 작업이 필요할 경우 여기에 작성합니다.
  }

  public void interaction()
  {
    // 타일과 상호작용하는 로직을 여기에 작성합니다.
    Debug.Log("Interacting with ResTile with voltage: " + voltage);

  }
}