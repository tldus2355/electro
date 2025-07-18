using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class Electro : MonoBehaviour
{
  public MapLoader map;
  private int x = -1;
  private int y = -1;
  private int mapx;
  private int mapy;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    this.mapy = map.mapdata.GetLength(0); //TODO: map 로드이후에 실행이 보장되게 해야 함
    this.mapx = map.mapdata.GetLength(1);

    for (int i = 0; i < this.mapy; i++)
    {
      for (int j = 0; j < this.mapx; j++)
      {
        if (map.mapdata[i, j] == -1)
        {
          this.y = i;
          this.x = j;
        }
      }
    }
    if (this.x == -1)
    {
      Debug.Log("Error: can't find start tile");
      Debug.Log("from Elecrto.cs");
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public int[] Move(char dir)
  {
    int[] path = new int[0];
    if (this.CanGo(dir))
    {
      ///////////////////여기 짜던중
    }


    return path;
  }

  private bool CanGo(char dir)
  {
    return true; //TODO: map 형식에 맞춰서 만들기
  }
}
