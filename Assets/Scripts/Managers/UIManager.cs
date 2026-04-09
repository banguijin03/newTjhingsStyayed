using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
	None, Loading, Title, Option, Movable, Menu, Info, Inside,
	_Length
}
public delegate void PopUpEvent(string title, string context, string confirm);

public class UIManager : ManagerBase
{
	public static event PopUpEvent OnPopUp;

	Canvas _mainCanvas;
	public Canvas MainCanvas => _mainCanvas;

	UIBase _movableScreen;

	GraphicRaycaster _raycaster;
	public GraphicRaycaster Raycaster => _raycaster;

	Dictionary<UIType, UIBase> uiDictionary = new();

	Rect _uiBoundary;
	public static Rect UIBoundary => GameManager.Instance?.UI?._uiBoundary ?? Rect.zero;

	UIType _currentScreenType;
	public static UIType CurrentScreen => GameManager.Instance?.UI?._currentScreenType ?? UIType.None;

	float _uiScale = 1.0f;
	public static float UIScale => GameManager.Instance?.UI?._uiScale ?? 1.0f;

	public IEnumerator Initialize(GameManager newManager)
	{
		SetMainCanvas(GetComponentInChildren<Canvas>());
		SetUI(UIType.Loading, GetComponentInChildren<UI_LoadingScreen>());
		yield return null;
	}

	protected override IEnumerator OnConnected(GameManager newManager)
	{
		_movableScreen = CreateUI(UIType.Movable, "MovableScreen");
		GameObject screenSwitcher = new GameObject("ScreenSwitcher");
		RectTransform switcherTransform = screenSwitcher.AddComponent<RectTransform>();
		//메인 캔버스에 넣기
		switcherTransform.SetParent(MainCanvas.transform);
		//캔버스중 맨 위로 올려주기
		switcherTransform.SetAsFirstSibling();
        //anchor를 stretch를 -stretch로
        switcherTransform.anchorMin = Vector3.zero;
		switcherTransform.anchorMax = Vector3.one;
		//여백을 0 0 0 0 
		switcherTransform.offsetMin = Vector3.zero;
		switcherTransform.offsetMax = Vector3.zero;
		//크기를 1로
		switcherTransform.localScale = Vector3.one;

        //시험용 필요한거 부름/ ("", switcherTransform)이면 switcherTransform의 자식
        CreateUI(UIType.Title, "TitleScreen", switcherTransform);
		CreateUI(UIType.Option, "OptionScreen", switcherTransform);
		CreateUI(UIType.Inside, "InsideScreen", switcherTransform);
		CreateUI(UIType.Menu, "MenuWindow", switcherTransform);

        //switcherTransform의 자식들은 끈다
        foreach (Transform currentTransform in switcherTransform)
		{
			currentTransform.gameObject.SetActive(false);
		}

		yield return null;
	}

	protected override void OnDisconnected()
	{
		UnSetAllUI();
	}

	protected void SetMainCanvas(Canvas newCanvas)
	{
		_mainCanvas = newCanvas;
		if (MainCanvas)
		{
			_raycaster = MainCanvas.GetComponent<GraphicRaycaster>();

			if(MainCanvas.transform is RectTransform mainRectTransform)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(mainRectTransform);
				_uiScale = mainRectTransform.lossyScale.x;
				_uiBoundary = mainRectTransform.rect;
				//_uiBoundary.size *= _uiScale;
				//_uiBoundary.position *= _uiScale / 1.0f;
			}
		}
		else
		{
			_raycaster = null;
		}
	}

	protected UIBase CreateUI(UIType wantType, string wantName, Transform parent)
	{
		GameObject instance = ObjectManager.CreateObject(wantName, parent);
		UIBase result = instance?.GetComponent<UIBase>();
		return SetUI(wantType, result);
	}
    protected UIBase CreateUI(UIType wantType, string wantName)
	{
		UIBase result = CreateUI(wantType, wantName, MainCanvas?.transform);
		if (result?.GetComponentInChildren<UI_DraggableWindow>())
		{
			_movableScreen?.SetChild(result.gameObject);
		}
		return result;
	}

    public static UIBase ClaimCreateUI(UIType wantType, string wantName) => GameManager.Instance?.UI?.CreateUI(wantType, wantName);

	protected void UnSetAllUI() 
	{
		foreach(UIBase ui in uiDictionary.Values) 
		{
			UnsetUI(ui);
		}
		uiDictionary.Clear();
	}
	protected void UnsetUI(UIType wantType) 
	{
		if(uiDictionary.TryGetValue(wantType, out UIBase found))
		{
			//처리하고
			UnsetUI(found);
			//지움
			uiDictionary.Remove(wantType);
		}
	}
	protected void UnsetUI(UIBase wantUI) 
	{
		if(!wantUI) return;

		wantUI.Unregistration(this);
	}
	public static void ClaimUnsetUI(UIBase wantUI)						=> GameManager.Instance?.UI?.UnsetUI(wantUI);
	public static void ClaimUnsetUI(GameObject wantObject)				=> ClaimUnsetUI(wantObject?.GetComponent<UIBase>());

	protected UIBase SetUI(UIBase wantUI)
	{
		wantUI?.Registration(this);
		return wantUI;
	}
	protected UIBase SetUI(UIType wantType, UIBase wantUI)
	{
		//Set UI를 하려고 하는데 문제가 무엇일까!
		//InventoryType, InventoryInstance
		if (wantUI == null) return null; // 승상께서 나를 더 필요로 하시지 않는구나

		//어? 뭐야? 이미 Inventory는 있는데? 너는 누구냐! => 서생원
		//일단 문전박대 => 프로그래밍에서는요? 똑같은 기능을 하는 친구면
		//음.. 너가 원본인 건 무슨 상관인데?
		//뒤이어서 들어온 친구는 치워버리겠다!
		if (uiDictionary.TryGetValue(wantType, out UIBase origin)) return origin;

		//두 가지의 시련을 모두 통과하다니. 너는 등록될 수 있는 자격을 갖추었다.
		uiDictionary.Add(wantType, wantUI);
		//등록 완!
		return SetUI(wantUI);
	}
	public static UIBase ClaimSetUI(UIBase wantUI)						=> GameManager.Instance?.UI?.SetUI(wantUI);
	public static UIBase ClaimSetUI(GameObject wantObject)				=> ClaimSetUI(wantObject?.GetComponent<UIBase>());
	public static UIBase ClaimSetUI(UIType wantType, UIBase wantUI)		=> GameManager.Instance?.UI?.SetUI(wantType, wantUI);

	protected UIBase GetUI(UIType wantType)
	{
		if (uiDictionary.TryGetValue(wantType, out UIBase result)) return result; //있으면 result반환
		else return null; //없으면 null
	}
	public static UIBase ClaimGetUI(UIType wantType)					=> GameManager.Instance?.UI?.GetUI(wantType);

	protected UIBase OpenUI(UIType wantType)
	{
		//Result가 누군지 전혀 모름!  리스코프 치환 원칙
		//IOpenable이면 열게 해준다! 세부 요소는 모르겠는데, 상위 요소만으로 실행하기
		UIBase result = GetUI(wantType);
		//이게 "열 수 있는"인 건 어떻게 확인할까요?
		//IOpenable인지 체크해보면 열 수 있는지 알 수 있습니다.
		//IOpenable로서 활동 할 수 있으면 IOpenable
		//result는 IOpenable인 opener인가?
		if(result is IOpenable asOpenable) asOpenable.Open();

		//아랫줄이랑 같은 의미예요!
		//IOpenable opener = result as IOpenable;
		//if(opener != null) opener.Open();
		return result;
	}
	public static UIBase ClaimOpenUI(UIType wantType)					=> GameManager.Instance?.UI?.OpenUI(wantType);

	protected UIBase CloseUI(UIType wantType)
	{
		UIBase result = GetUI(wantType);
		//             자료형    이름   =>  변수 생성
		if(result is IOpenable asOpenable) asOpenable.Close();
		return result;
	}
	public static UIBase ClaimCloseUI(UIType wantType)					=> GameManager.Instance?.UI?.CloseUI(wantType);

	protected UIBase ToggleUI(UIType wantType)
	{
		UIBase result = GetUI(wantType);
		if(result is IOpenable asOpenable) asOpenable.Toggle();
		return result;
	}
	public static UIBase ClaimToggleUI(UIType wantType)					=> GameManager.Instance?.UI?.ToggleUI(wantType);

	protected UIBase OpenScreen(UIType wantType)
	{
		CloseUI(CurrentScreen);			//1. 기존 스크린 닫음
		_currentScreenType = wantType;	//2. 새로운 타입 설정
		return OpenUI(wantType);		//3. 열기
	}
	public static UIBase ClaimOpenScreen(UIType wantType) => GameManager.Instance?.UI?.OpenScreen(wantType);

	public static void ClaimPopUp(string title, string context, string confirm)
	{
		OnPopUp?.Invoke(title, context, confirm);
	}
	public static void ClaimErrorMessage(string context)
	{
		OnPopUp?.Invoke("Error", context, "Confirm");
	}
}
