using UnityEngine;

public class State : MonoBehaviour
{
  public static State Instance;
  public Player player;
  public MapLoader map;

  void Awake()
  {
    // singleton
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
    if (Input.GetKeyDown(KeyCode.UpArrow)) //TODO: 버튼 두개 동시에 입력받는경우 해결
    {
      player.Move('u', 0);
    }
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      player.Move('d', 0);
    }
    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      player.Move('l', 0);
    }
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      player.Move('r', 0);
    }
  }
}
