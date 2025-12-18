using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TextButtonController : MonoBehaviour, IPointerDownHandler
{
    // 버튼의 종류를 미리 정의
    public enum ButtonFunction
    {
        GameStart,      // 게임시작
        OpenHelp,       // 도움말 열기
        OpenQuitConfirm,// 게임종료 판넬 열기
        CloseHelp,      // 도움말 닫기
        ExitGame,       // 실제 게임 종료
        CloseQuitConfirm // 게임종료 판넬 닫기
    }

    [Header("이 버튼의 기능을 선택하세요")]
    public ButtonFunction functionType;

    [Header("연결할 판넬들")]
    public GameObject helpPanel;        // 도움말 판넬
    public GameObject quitConfirmPanel; // 게임종료 확인 판넬

    [Header("이동할 씬 이름")]
    public string gameSceneName = "World Create Wide"; // 실제 게임 씬 이름

    // 클릭했을 때 실행되는 함수
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (functionType)
        {
            case ButtonFunction.GameStart:
                SceneManager.LoadScene(gameSceneName); // 게임 씬으로 이동
                break;

            case ButtonFunction.OpenHelp:
                if (helpPanel != null) helpPanel.SetActive(true);
                break;

            case ButtonFunction.CloseHelp:
                if (helpPanel != null) helpPanel.SetActive(false);
                break;

            case ButtonFunction.OpenQuitConfirm:
                if (quitConfirmPanel != null) quitConfirmPanel.SetActive(true);
                break;

            case ButtonFunction.CloseQuitConfirm:
                if (quitConfirmPanel != null) quitConfirmPanel.SetActive(false);
                break;

            case ButtonFunction.ExitGame:
                Debug.Log("게임 종료!");
                Application.Quit(); // 실제 빌드된 게임에서 종료
                break;
        }
    }
}