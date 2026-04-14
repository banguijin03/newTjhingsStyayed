using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UIType
{
	None, Loading, Title, Option, Movable, Menu, Info, Inside, GameQuit,
	Base, Option_1, Option_2, Option_3, Option_4, Option_5, Option_6,
    _Length
}
public delegate void PopUpEvent(string title, string context, string confirm);

public class UIManager : ManagerBase
{
	public static event PopUpEvent OnPopUp;

	readonly KeyValuePair<UIType, string>[] globalScreenArray =
	{
	new(UIType.Title, "TitleScreen"),
	new(UIType.Option, "OptionScreen"),
	new(UIType.Inside, "InsideScreen"),
	new(UIType.Menu, "MenuWindow"),
};

	Canvas _mainCanvas;
	public Canvas MainCanvas => _mainCanvas;

	UIBase _movableScreen;
	RectTransform switcherTransform;
	RectTransform createdTransform;

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

	public RectTransform CreateFullScreen(string wantName)
	{
		GameObject instance = new GameObject(wantName);
		RectTransform result = instance.AddComponent<RectTransform>();
		//메인 캔버스에 넣기
		result.SetParent(MainCanvas.transform);
		//캔버스중 맨 위로 올려주기
		result.SetAsFirstSibling();
        //anchor를 stretch를 -stretch로
        result.anchorMin = Vector3.zero;
		result.anchorMax = Vector3.one;
		//여백을 0 0 0 0 
		result.offsetMin = Vector3.zero;
		result.offsetMax = Vector3.zero;
		//크기를 1로
		result.localScale = Vector3.one;
		return result;
	}

	protected override IEnumerator OnConnected(GameManager newManager)
	{
		createdTransform = CreateFullScreen("CreatedUI");
		_movableScreen = CreateUI(UIType.Movable, "MovableScreen", MainCanvas?.transform);

		switcherTransform = CreateFullScreen("ScreenSwitcher");

        //switcherTransform의 자식들은 끈다
        foreach (var currentPair in globalScreenArray)
		{
			UIBase created = CreateUI(currentPair.Key, currentPair.Value, switcherTransform);
			if (created is IOpenable asOpenable) asOpenable.Close();
		}

		RectTransform changerTransform = CreateFullScreen("ScreenChanger");
		changerTransform.SetAsLastSibling();

		GameObject instance = ObjectManager.CreateObject("ScreenChanger", changerTransform);
		ObjectManager.CreateObject("ScreenChanger", changerTransform);
		if(instance.TryGetComponent(out UI_ScreenChanger asChanger))
		{
			asChanger.ChangeStart();

			yield return new WaitForSeconds(3);
			asChanger.ChangeEnd();
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
		UIBase result = CreateUI(wantType, wantName, createdTransform ?? MainCanvas.transform);
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
			UnsetUI(found);
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
		if (wantUI == null) return null;

		if (uiDictionary.TryGetValue(wantType, out UIBase origin)) return origin;

		uiDictionary.Add(wantType, wantUI);
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
		UIBase result = GetUI(wantType);
		
		if(result is IOpenable asOpenable) asOpenable.Open();
		if (result) EventSystem.current.SetSelectedGameObject(result.gameObject);

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
