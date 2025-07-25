using UnityEngine;

public class CrossLoad : Tile
{
  public CrossLoad()
  {
    // 생성자에서 방향을 초기화합니다.
    this.direction = new char[] { 'u', 'd', 'l', 'r' };
    this.isStop = false; // CrossLoad는 정지 타일이 아닙니다.
  }

  void Start()
  {
    
  }

  public override char NextDir(char dir)
  {
    // cross의 경우에는 방향을 바꾸지 않음
    return dir; 
  }
}