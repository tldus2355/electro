using Unity.VisualScripting;
using UnityEngine;

public class EnemyTile : SimpleRoad, IHasInteraction, IHasVoltage
{
  public static int enemyCount = 0; 
  public int voltage; // 전압을 나타내는 변수
  public override void Init(char[] directions, bool isStart = false, bool hasInteraction = false)
  {
    // 생성자에서 방향과 시작 여부를 초기화합니다.
    this.directions = directions;
    this.isStart = false; // EnemyTile은 시작 타일이 아닙니다
    this.isStop = true; // EnemyTile은 정지 타일입니다.
    this.hasInteraction = true; // EnemyTile은 상호작용이 가능합니다.
    enemyCount++; // EnemyTile이 생성될 때마다 enemyCount 증가
  }

  public void SetVoltage(int voltage)
  {
    this.voltage = voltage; // 전압을 설정하는 메서드
    Debug.Log("EnemyTile voltage set to: " + voltage);
  }

  void Start()
  {

  }

  public void interaction()
  {
    // 타일과 상호작용하는 로직을 여기에 작성합니다.
    Debug.Log("Interacting with EnemyTile with voltage: " + voltage);
  }
}