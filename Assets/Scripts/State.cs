using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
  public static State Instance;
  public Player player;
  public MapLoader map;

  void Awake()
  {
    // singleton game manager가 아니라서 싱글톤 아닐수도. 씬마다 따로, 나중에 수정하기
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    List<char> path = new List<char>();
    if (!this.player.isMoving)
    {
      if (Input.GetKeyDown(KeyCode.UpArrow)) //TODO: 버튼 두개 동시에 입력받는경우 해결
      {
        path = player.Move('u', 0);
        Debug.Log("Path: " + string.Join(", ", path));
        this.PlayerMoveAnimation(path);
      }
      if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        path = player.Move('d', 0);
        Debug.Log("Path: " + string.Join(", ", path));
        this.PlayerMoveAnimation(path);
      }
      if (Input.GetKeyDown(KeyCode.LeftArrow))
      {
        path = player.Move('l', 0);
        Debug.Log("Path: " + string.Join(", ", path));
        this.PlayerMoveAnimation(path);
      }
      if (Input.GetKeyDown(KeyCode.RightArrow))
      {
        path = player.Move('r', 0);
        Debug.Log("Path: " + string.Join(", ", path));
        this.PlayerMoveAnimation(path);
      }
    }
  }

  private void PlayerMoveAnimation(List<char> path)
  {
    foreach (char dir in path)
    {
      // 경로에 기본 사각형 스프라이트 표시
      GameObject marker = new GameObject("PathMarker");
      marker.transform.position = player.transform.position;
      var sr = marker.AddComponent<SpriteRenderer>();
      sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0,0,1,1), new Vector2(0.5f,0.5f));
      sr.color = Color.yellow; // 원하는 색상
      marker.transform.localScale = new Vector3(100f, 100f, 1); // 크기 조절
      switch (dir)
      {
        case 'u': player.transform.Translate(0, 1, 0); break;
        case 'd': player.transform.Translate(0, -1, 0); break;
        case 'l': player.transform.Translate(-1, 0, 0); break;
        case 'r': player.transform.Translate(1, 0, 0); break;
      }
    }
    player.isMoving = false;
  }
}
