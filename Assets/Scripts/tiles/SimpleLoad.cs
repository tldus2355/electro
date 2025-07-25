using UnityEngine;

public class SimpleLoad : Tile
{
  // SimpleLoad는 Tile을 상속받아 방향을 설정하는 간단한 타일 클래스입니다.
  public SimpleLoad(char[] directions, bool isStart = false, bool isStop = true)
  {
    // 생성자에서 방향과 시작 여부를 초기화합니다.
    this.direction = directions;
    this.isStart = isStart;
    this.isStop = isStop;
  }
  void Start()
  {
    
  }
}