using Unity.VisualScripting;
using UnityEngine;

public class EnemyTile : SimpleRoad
{
  public int voltage; // 전압을 나타내는 변수
  public EnemyTile(char[] directions, int voltage)
  {
    // 생성자에서 방향을 초기화합니다.
    this.directions = directions;
    this.isStop = true;
    this.voltage = voltage; // 전압을 설정합니다.
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