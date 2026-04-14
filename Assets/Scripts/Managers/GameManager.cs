using System.Collections;
using UnityEngine;

public delegate void InitializeEvent();
public delegate void UpdateEvent(float deltaTime);
public delegate void DestroyEvent();

public class GameManager : MonoBehaviour
{
	static GameManager _instance;
	public static GameManager Instance => _instance;

	UIManager _ui;
	public UIManager		UI => _ui;

	DataManager _data;
	public DataManager		Data => _data;

	ObjectManager _objectM;
	public ObjectManager ObjectM => _objectM;

	SaveManager _save;
	public SaveManager		Save => _save;

	SettingManager _setting;
	public SettingManager	Setting => _setting;

	LanguageManager _language;
	public LanguageManager	Language => _language;

	AudioManager _audio;
	public AudioManager	Audio => _audio;

	CameraManager _camera;
	public CameraManager	Camera => _camera;

	InputManager _input;
	public InputManager	Input => _input;

	IEnumerator initializing; 

	public static event InitializeEvent	OnInitializeManager;
	public static event InitializeEvent	OnInitializeController;
	public static event InitializeEvent	OnInitializeCharacter;
	public static event InitializeEvent	OnInitializeObject;
	public static event UpdateEvent		OnUpdateManager;
	public static event UpdateEvent		OnUpdateController;
	public static event UpdateEvent		OnUpdateCharacter;
	public static event UpdateEvent		OnUpdateObject;
	public static event DestroyEvent	OnDestroyManager;
	public static event DestroyEvent	OnDestroyController;
	public static event DestroyEvent	OnDestroyCharacter;
	public static event DestroyEvent	OnDestroyObject;

	bool isLoading = true;
	bool isPlaying = true;
	void Awake()
    {
		if(Instance == null) 
		{
			_instance = this;
		}
		else
		{
			Destroy(this);
			return;
		}
		initializing = InitializeManagers();
		StartCoroutine(initializing);

	}

	void OnDestroy()
	{
		if(initializing != null) StopCoroutine(initializing); //로딩을 진행하는 중이었다면 끊어버릴 수 있도록!
		DeleteManagers(); //하위 매니저들도 없어지게!
	}

	//얘가 문제
	//로딩은... 얼마나 걸릴까
	//1프레임만에 끝낼 수 있을까?
	//1프레임 넘는 시간동안 "이 함수"가 실행되고 있으면 무슨 일이 일어날까?
	//게임이 멈춥니다. 이 함수 끝날 때까지
	//이 상태에서 게임을 클릭하면 어떻게 되는가 => 응답없음 => 유저는 꺼버림
	//"기다림 함수"
	//coroutine = co - routine
	//           함께   루틴
	//         화면출력 유저입력 /   로딩
	//					 요리   /   청소
	//운동을 해야 합니다. 상체루틴 하체루틴
	//                    1시간    1시간
	//오늘 남은 시간이 1시간
	//옆에 있는 친구를 데려와서 상체 1시간 시키고
	//저는 하체 1시간 하면 => 암튼 둘 다 했음
	//IEnumerator => Start
	//WaitForSeconds을 통해서 시간을 "기다린" 적이 있었죠!
	IEnumerator InitializeManagers()
	{
		//UI를 만들어서 로딩창이라던지, 다른 유저에게 보여줄 수 있는 공간
		//데이터 불러오기
		//유저 세이브 불러오기
		//설정값을 찾아서 세팅
		//언어도 세팅
		//사운드도 세팅
		//카메라 초기화
		//유저 입력 받기 시작
		//몇 개가 필요한지 집계를 받을 때 => 필요한 것! 적어둘 공간이 필요해요!
		int totalLoadCount = 0;
		totalLoadCount += CreateManager(ref _ui).LoadCount;
		totalLoadCount += CreateManager(ref _data).LoadCount;
		totalLoadCount += CreateManager(ref _objectM).LoadCount;
		totalLoadCount += CreateManager(ref _save).LoadCount;
		totalLoadCount += CreateManager(ref _setting).LoadCount;
		totalLoadCount += CreateManager(ref _language).LoadCount;
		totalLoadCount += CreateManager(ref _audio).LoadCount;
		totalLoadCount += CreateManager(ref _camera).LoadCount;
		totalLoadCount += CreateManager(ref _input).LoadCount;

		yield return UI.Initialize(this);
		UIBase loadingUI = UIManager.ClaimOpenScreen(UIType.Loading); //UI System이 돌아가기 시작했으니까 기능을 실행해보기!
		IProgress<int> loadingProgress = loadingUI as IProgress<int>;

		loadingProgress?.Set(0, totalLoadCount);
		yield return Data.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return ObjectM.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return UI.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return Save.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return Setting.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return Language.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return Audio.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return Camera.Connect(this);  
		loadingProgress?.AddCurrent(1);
		yield return Input.Connect(this);
		loadingProgress?.AddCurrent(1);
		yield return null;
		UIManager.ClaimOpenScreen(UIType.Title);
		isLoading = false;
	}

	void DeleteManagers()
	{
		//유저입력	InputManager
		Input?.Disconnect();
		//오브젝트	ObjectManager
		ObjectM?.Disconnect();
		//오디오		AudioManager
		Audio?.Disconnect();
		//언어		LanguageManager
		Language?.Disconnect();
		//세팅		SettingManager
		Setting?.Disconnect();
		//세이브		SaveManager
		Save?.Disconnect();
		//카메라		CameraManager
		Camera?.Disconnect();
		//UI		UIManager
		UI?.Disconnect();
		//데이터파일 DataManager
		Data?.Disconnect();
	}
	ManagerType CreateManager<ManagerType>(ref ManagerType targetVariable) where ManagerType : ManagerBase
	{
		if (targetVariable == null)
		{
			targetVariable = this.TryAddComponent<ManagerType>();
		}

		return targetVariable;
	}

	public static void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else 
		Application.Quit();
#endif
		
	}

	public static void Pause()
	{
		Instance.isPlaying = false;
	}

	public static void Unpause()
	{
		Instance.isPlaying = true;
	}

	void InvokeInitializeEvent(ref InitializeEvent OriginEvent)
	{
		if (OriginEvent != null) 
		{
			InitializeEvent CurrentEvent = OriginEvent; 
			OriginEvent = null; 
			CurrentEvent.Invoke(); 
		}
	}
	void InvokeDestroyEvent(ref DestroyEvent OriginEvent)
	{
		if (OriginEvent != null) 
		{
			DestroyEvent CurrentEvent = OriginEvent; 
			OriginEvent = null; 
			CurrentEvent.Invoke();
		}
	}
	void Update()
    {
		if (isLoading) return;

		//초기화
		//매니저를 초기화한다
		InvokeInitializeEvent(ref OnInitializeManager);
		//캐릭터를 초기화한다
		InvokeInitializeEvent(ref OnInitializeCharacter);
		//컨트롤러를 초기화한다 => 캐릭터가 있는 상태에서 돌아가야 하니까!
		InvokeInitializeEvent(ref OnInitializeController);
		//오브젝트를 초기화한다
		InvokeInitializeEvent(ref OnInitializeObject);

		if (isPlaying)
		{
			//프레임 사이에 몇 초가 지났을까?
			float deltaTime = Time.deltaTime;
			//매니저가 업데이트 하는 경우
			OnUpdateManager?.Invoke(deltaTime);
			//컨트롤러를 업데이트한다 => 먼저 판단하고
			OnUpdateController?.Invoke(deltaTime);
			//캐릭터를 업데이트한다 => 캐릭터가 수행하고
			OnUpdateCharacter?.Invoke(deltaTime);
			//오브젝트를 업데이트한다 => 오브젝트 진행
			OnUpdateObject?.Invoke(deltaTime);
		}

		//오브젝트를 제거한다
		InvokeDestroyEvent(ref OnDestroyObject);
		//컨트롤러를 제거한다
		InvokeDestroyEvent(ref OnDestroyController);
		//캐릭터를 제거한다
		InvokeDestroyEvent(ref OnDestroyCharacter);
		//매니저를 제거한다
		InvokeDestroyEvent(ref OnDestroyManager);
	}
}
