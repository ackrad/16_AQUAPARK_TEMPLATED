using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
	
	#region Fields

	/// <summary>
	/// The instance.
	/// </summary>
	private static T instance;

	#endregion

	#region Properties

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = FindObjectOfType<T> ();
			}
           
			return instance;
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	protected virtual void Awake ()
	{
		if ( instance == null )
		{
			instance = this as T;
			// DontDestroyOnLoad ( gameObject );
		}
		else if(instance!=this)
		{
			Destroy ( gameObject );
		}
	}

	public static T request(){
		if(!instance){
			Debug.LogError("There is no instance of "+typeof(T).Name+" in the scene");
		}

		return instance;
	}

	public static T forceRequest(){
		if(instance==null){
			GameObject ownerObject=new GameObject();
			instance=ownerObject.AddComponent<T>();
			
		}
		return instance;
	}

	#endregion
	
}