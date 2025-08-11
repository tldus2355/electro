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
    // TODO: 플레이어가 자기 위치 찾은 이후에 실행: 플레이어 위치 화면에 표시하기
  }

  // Update is called once per frame
  void Update()
  {
    List<char> path = new List<char>();
    if (!this.player.isMoving)
    {
      if (Input.GetKeyDown(KeyCode.UpArrow)) //TODO: 버튼 두개 동시에 입력받는경우 해결, 금지된 입력 처리
      {
        if (!this.player.CanGo('u'))
        {
          Debug.Log("Error: can't go up");
          return;
        }
        path = player.Move('u', 0);
      }
      if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        if (!this.player.CanGo('d'))
        {
          Debug.Log("Error: can't go down");
          return;
        }
        path = player.Move('d', 0);
      }
      if (Input.GetKeyDown(KeyCode.LeftArrow))
      {
        if (!this.player.CanGo('l'))
        {
          Debug.Log("Error: can't go left");
          return;
        }
        path = player.Move('l', 0);
      }
      if (Input.GetKeyDown(KeyCode.RightArrow))
      {
        if (!this.player.CanGo('r'))
        {
          Debug.Log("Error: can't go right");
          return;
        }
        path = player.Move('r', 0);
      }
      if (this.player.isMoving)
      {
        if(path[path.Count - 1] == 'f')
        {
          Debug.Log("Fuse Over in State.cs");
          this.gameover();
          return;
        }
        Debug.Log("Path: " + string.Join(", ", path));
        this.player.MoveAnimation(path);
        this.CheckObject(); // 플레이어가 이동한 후에 오브젝트 체크
        this.player.isMoving = false; // 애니메이션이 끝나면 다시 false로 설정
      }
    }
  }


  private void CheckObject()
  {
    var pos = player.GetPosition();
    int px = pos.x, py = pos.y;
    SimpleRoad currentTile = map.mapdata[py, px];

    if (currentTile.hasInteraction)
    {
      Debug.Log("Interacting with tile at (" + px + ", " + py + ")");
      // Res ********
      if (currentTile is ResTile resTile)
      {
        resTile.interaction(); // ResTile의 interaction 메서드 호출
        player.addVoltage(-1 * resTile.voltage); // 플레이어의 전압 감소
      }
      // Fuse ********
      // else if (currentTile is FuseTile fuseTile)
      // {
      //   fuseTile.interaction(); // FuseTile의 interaction 메서드 호출
      //   if (this.player.voltage >= fuseTile.voltage)
      //   {
      //     Debug.Log("Error: Not enough voltage to interact with FuseTile");
      //     this.gameover();
      //     return;
      //   }
      // }
      // Enemy ********
      else if (currentTile is EnemyTile enemyTile)
      {
        enemyTile.interaction(); // EnemyTile의 interaction 메서드 호출
        if (this.player.voltage >= enemyTile.voltage)
        {
          // TODO: 현재 타일을 SimpleRoad로 바꾸기
          Debug.Log(EnemyTile.enemyCount + " enemies remaining.");
          player.addVoltage(-1 * enemyTile.voltage); // 플레이어의 전압 감소
          EnemyTile.enemyCount--;
          if (EnemyTile.enemyCount <= 0)
          {
            Debug.Log("All enemies defeated!");
            // 게임 승리 로직을 여기에 작성합니다.
          }
          else
          {
            Debug.Log("Enemy defeated, remaining: " + EnemyTile.enemyCount);
          }
          return; // 전압이 부족하면 상호작용하지 않음
        }
        else
        {
          this.gameover();
          return; // 전압이 부족하면 게임 오버
        }
      }
      // Vdd ********
      else if (currentTile is VddTile vddTile)
      {
        if (!vddTile.isUsed) // VddTile의 interaction 메서드 호출
        {
          player.addVoltage(vddTile.voltage); // 플레이어의 전압 증가
          vddTile.interaction(); // VddTile의 interaction 메서드 호출
        }
      }
      // Cap ********
      else if (currentTile is CapTile capTile)
      {
        if (!capTile.isUsed && capTile.voltage == 0)
        {
          capTile.SetVoltage(player.voltage); // 플레이어의 전압을 CapTile에 설정
          player.SetVoltage(0); // 플레이어의 전압을 0으로 설정
        }
        else if (!capTile.isUsed)
        {
          player.addVoltage(capTile.voltage); // 플레이어의 전압 증가
        }
        else
        {
          Debug.Log("CapTile has already been used.");
        }
        capTile.interaction(); // CapTile의 interaction 메서드 호출
      }
      // Inductor ********
      else if (currentTile is IndTile indTile)
      {
        if (player.voltage > indTile.voltage)
        {
          player.SetVoltage(indTile.voltage); // 플레이어의 전압을 IndTile의 전압으로 설정
          Debug.Log("[LOG] Player voltage set to IndTile voltage: " + indTile.voltage);
          return; // 전압이 IndTile보다 크면 게임 오버
        }
        indTile.interaction(); // IndTile의 interaction 메서드 호출
      }
      else
      {
        Debug.Log("[LOG] No interaction defined for this tile type.");
      }
    }
    else
    {
      Debug.Log("[LOG] No interaction available on this tile.");
    }


    return;
  }
  
  public void gameover()
  {
    Debug.Log("Game Over");
    // 게임 오버 로직을 여기에 작성합니다.
    // 예: UI 표시, 재시작 버튼 활성화 등
  }
}
