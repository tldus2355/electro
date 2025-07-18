using UnityEngine;

public class State : MonoBehaviour
{
  public static State Instance;
  public Electro electro;
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
      electro.Move('u');
    }
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
      electro.Move('d');
    }
    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
      electro.Move('l');
    }
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
      electro.Move('r');
    }
  }
}
