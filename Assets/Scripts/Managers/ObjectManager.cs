using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager : ManagerBase
{
	readonly string[] globalPoolSettings =
	{
		"GlobalCharacterPool",
		"GlobalControllerPool",
		"GlobalEffectPool",
		"GlobalObjectPool",
		"GlobalUIPool",
	};
	List<PoolRequest> loadedPoolRequests = new();

	static Dictionary<string, ObjectPoolModule> poolDictionary = new();

	protected override IEnumerator OnConnected(GameManager newManager)
	{
		RegistrationInHierarchy();
		RegistrationPool(globalPoolSettings);
		InitializePool();

		yield return null;
	}

	protected override void OnDisconnected()
	{

	}

	public static GameObject CreateObject(string wantName, Transform parent = null)
	{
		GameObject result = null;

		wantName = wantName.ToLower();

		if(poolDictionary.TryGetValue(wantName, out ObjectPoolModule pool))
		{
			result = pool.CreateObject(parent);
		}
		else if(DataManager.TryLoadDataFile(wantName, out GameObject prefab) && prefab)
		{
			result = Instantiate(prefab, parent);
		}

		if (!result) UIManager.ClaimErrorMessage(SystemMessage.ObjectNameNotFound(wantName));

		RegistrationObject(result); 

		return result;
	}
	public static GameObject CreateObject(GameObject prefab, Transform parent = null)
	{
		if (prefab == null) return null;
		GameObject result = Instantiate(prefab, parent); 
		RegistrationObject(result);
		return result;
	}

	public static GameObject CreateObject(string wantName, Vector3 position)
	{
		GameObject result = CreateObject(wantName);
		if (result) result.transform.position = position;
		return result;
	}
	public static GameObject CreateObject(GameObject prefab, Vector3 position)
	{
		GameObject result = CreateObject(prefab);
		if(result) result.transform.position = position;
		return result;
	}

	public static GameObject CreateObject(string wantName, Vector3 position, Quaternion rotation)
	{
		GameObject result = CreateObject(wantName);
		if (result)
		{
			result.transform.position = position;
			result.transform.rotation = rotation;
		}
		return result;
	}
	public static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		GameObject result = CreateObject(prefab);
		if (result)
		{
			result.transform.position = position;
			result.transform.rotation = rotation;
		}
		return result;
	}

	public static GameObject CreateObject(string wantName, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		GameObject result = CreateObject(wantName);
		if (result)
		{
			result.transform.position = position;
			result.transform.rotation = rotation;
			result.transform.localScale = scale;
		}
		return result;
	}
	public static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		GameObject result = CreateObject(prefab);
		if (result)
		{
			result.transform.position = position;
			result.transform.rotation = rotation;
			result.transform.localScale = scale;
		}
		return result;
	}

	public static GameObject CreateObject(string wantName, Transform parent, Vector3 position, Space space = Space.Self)
	{
		GameObject result = CreateObject(wantName, parent);
		if (result)
		{
			switch(space)
			{
				case Space.World:
					result.transform.position = position; 
					break;
				case Space.Self:
					result.transform.localPosition = position;
					break;
			}
		}
		return result;
	}
	public static GameObject CreateObject(GameObject prefab, Transform parent, Vector3 position, Space space = Space.Self)
	{
		GameObject result = CreateObject(prefab, parent);
		if (result)
		{
			switch(space)
			{
				case Space.World:
					result.transform.position = position; 
					break;
				case Space.Self:
					result.transform.localPosition = position;
					break;
			}
		}
		return result;
	}

	public static GameObject CreateObject(string wantName, Transform parent, Vector3 position, Quaternion rotation, Space space = Space.Self)
	{
		GameObject result = CreateObject(wantName, parent);
		if (result)
		{
			switch (space)
			{
				case Space.World:
					result.transform.position = position; //절대값을 기준으로
					result.transform.rotation = rotation;
					break;
				case Space.Self:
					result.transform.localPosition = position; //부모를 기준으로
					result.transform.localRotation = rotation; //부모를 기준으로
					break;
			}
		}
		return result;
	}
	public static GameObject CreateObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Space space = Space.Self)
	{
		GameObject result = CreateObject(prefab, parent);
		if (result)
		{
			switch (space)
			{
				case Space.World:
					result.transform.position = position; //절대값을 기준으로
					result.transform.rotation = rotation;
					break;
				case Space.Self:
					result.transform.localPosition = position; //부모를 기준으로
					result.transform.localRotation = rotation; //부모를 기준으로
					break;
			}
		}
		return result;
	}

	public static GameObject CreateObject(string wantName, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale, Space space = Space.Self)
	{
		GameObject result = CreateObject(wantName, parent);
		if (result)
		{
			switch (space)
			{
				case Space.World:
					result.transform.position = position; //절대값을 기준으로
					result.transform.rotation = rotation;
					result.transform.localScale	= scale; //부모를 기준으로
					break;
				case Space.Self:
					result.transform.localPosition  = position; //부모를 기준으로
					result.transform.localRotation  = rotation; 
					result.transform.localScale		= scale; 
					break;
			}
		}
		return result;
	}
	public static GameObject CreateObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale, Space space = Space.Self)
	{
		GameObject result = CreateObject(prefab, parent);
		if (result)
		{
			switch (space)
			{
				case Space.World:
					result.transform.position = position; //절대값을 기준으로
					result.transform.rotation = rotation;
					result.transform.localScale	= scale; //부모를 기준으로
					//"진짜 월드 스케일" 을 본 적 있습니다.
					// lossyScale
					// 저는 "진짜"크기는 (1,1,1)
					// 근데 "부모"크기는 (2,2,2)
					// 이때 "로컬"크기는 (0.5, 0.5, 0.5)
					// 저의 "진짜" 크기를 => (3,3,3)으로 하고 싶다고 해보죠
					// 설정할 "로컬"크기는 몇일까요? (1.5, 1.5, 1.5)
					// 물론 회전하는 순간 끔살
					// 어떻게 제가 원하는 "진짜" 크기가 되기 위한 "로컬"크기를 구할 수 있을까?
					// "진짜 크기"를 원한다 "부모 크기"랑 비교를 해서 값을 가져주면 좋겠다!
					// 여기서 문제! 부모의 부모는 어떻게 해요?
					// 조부모 부모  로컬    월드크기       월드크기
					// 0.5    1.5  1.2  => 0.9        =>  3
					// 0.5    1.5  4    => 3
					// 로컬 => 월드  로컬 * (월드 / 로컬) = 월드
					//               1.2 *  (0.9 / 1.2) = 0.9
					// 월드 => 로컬  월드 * (로컬 / 월드) = 로컬
					//               0.9 *  (1.2 / 0.9) = 1.2
					//               3   *  (4/3)       = 4
					//벡터는 행렬이기 때문에, 행렬 곱을 할 수 있는 조건은 => xy행렬이라면 yx행렬이어야 한다
					//           (0)
					// (1,2,3) x (1)
 					//           (2)
					//Vector3 originLocalScale = result.transform.localScale;
					//Vector3 originLossyScale = result.transform.lossyScale;
					//float scaledScaleX = scale.x * (originLocalScale.x / originLossyScale.x);
					//float scaledScaleY = scale.y * (originLocalScale.y / originLossyScale.y);
					//float scaledScaleZ = scale.z * (originLocalScale.z / originLossyScale.z);
					//result.transform.localScale = new Vector3(scaledScaleX, scaledScaleY, scaledScaleZ);
					break;
				case Space.Self:
					result.transform.localPosition  = position; //부모를 기준으로
					result.transform.localRotation  = rotation; 
					result.transform.localScale		= scale; 
					break;
			}
		}
		return result;
	}

	public static void RegistrationObject(GameObject target) //실제로 등록하는 기능
	{
		if (target)
		{
			//이 친구가 등록 가능한지를 어떻게 체크할까?
			//저희가 만드는 건 "컴포넌트"를 만드는 것이지
			//"게임 오브젝트"를 만드는 것이 아니기 때문에
			//IFunctionable이 들어간 곳은 "컴포넌트"다!
			//GetComponent : 컴포넌트를 가져옴 (제일 첫번째 컴포넌트)
			//GetComponent<IFunctionable>() => IFunctionable 하나
			//GetComponents<IFunctionable>() => IFunctionable을 상속받는 모든 컴포넌트
			//GetComponentsInChild<IFunctionable>() => (나포함) 자식한테 있는 IFunctionable을 상속받는 모든 컴포넌트
			//GetComponentsInChildren<IFunctionable>() => (나포함)자식들한테 있는 IFunctionable을 상속받는 모든 컴포넌트
			foreach (var current in target.GetComponentsInChildren<IFunctionable>())
			{
				current.RegistrationFunctions();
			}
		}
	}

	public static void DestroyObject(GameObject target)
	{
		if (!target) return;
		UnregistrationObject(target);
		if (target.TryGetComponent(out PooledObject pool)) 
		{
			pool.OnEnqueue(); 
		}
		else 
		{
			Destroy(target); 
		}
	}

	public static void UnregistrationObject(GameObject target)
	{
		if (!target) return;

		foreach (var current in target.GetComponentsInChildren<IFunctionable>())
		{
			current.UnregistrationFunctions();
		}
	}
	public void RegistrationInHierarchy()
	{
		foreach(MonoBehaviour current in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
		{
			if(current is IFunctionable currentFunctionable)
			{
				currentFunctionable.RegistrationFunctions();
			}
		}
	}

	public void RegistrationPool(string poolName)
	{
		poolName = poolName.ToLower();

		PoolRequest currentRequest = DataManager.LoadDataFile<PoolRequest>(poolName);
		if (currentRequest == null) return;
		if (currentRequest.settings == null) return;

		loadedPoolRequests.Add(currentRequest);
		//애들마다 하나씩!
		//        학생          다음학생    in   3학년 4반
		foreach (PoolSetting currentSetting in currentRequest.settings)
		{
			string currentName = currentSetting.poolName.ToLower();
			GameObject currentPrefab = currentSetting.target;
			//다음학생이.. 오늘 학교 안왔대요!
			//=> 헌혈차를 접어버리면 안되고
			//=> 다음 학생을 불러야 한다!
			if (currentPrefab == null) continue;
			//문제가 생길 여지가 하나 더 있다!
			//프리팹을 찾아봤으니까, 이름에서 문제가 생길 수 있는 여지!
			//딕셔너리에는 같은 키값을 두 개 넣을 수 없다!
			if (poolDictionary.ContainsKey(currentName)) continue;
			//나의 시련을 모두 통과하다니. 너를 정식 기사로 임명해주마
			poolDictionary.Add(currentName, new(currentSetting));
		}
	}

	public void RegistrationPool(params string[] poolNames)
	{
		foreach (string poolName in poolNames)
		{
			RegistrationPool(poolName);
		}
	}

	public void InitializePool()
	{
		foreach(ObjectPoolModule currentPool in poolDictionary.Values)
		{
			currentPool?.Initialize();
		}
	}
}
