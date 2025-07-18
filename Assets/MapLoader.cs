using UnityEngine;

public class MapLoader : MonoBehaviour
{
  public int[,] mapdata = new int[5, 10]; //나중에 동적으로 바꾸기, 퍼즐 맵 데이터에 따라 바뀌게
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.A))
    {
      this.MapLoadTest();
    }
  }

  public void MapLoadTest()
  {
    this.mapdata[0, 0]++;
  }

  private void LoadMap()
  {
    // 스테이지 씬 시작될 때 MapLoader 오브젝트(= 이 코드 mapdata)에 tiled 저장하기 
  }
}
