using Unity.VisualScripting;
using UnityEngine;

public class EnemyTile : Tile
{
  public int voltage; // 전압을 나타내는 변수
  public EnemyTile(char[] directions, int voltage)
  {
    // 생성자에서 방향을 초기화합니다.
    this.direction = directions;
    this.isStop = true;
    this.voltage = voltage; // 전압을 설정합니다.
  }

  void Start()
  {
        
  }
}