using UnityEngine;
using System.Threading;
using System.Collections;

public class ToastExampleScreen : MonoBehaviour {
    private AndroidJavaObject toastExample = null;
    private AndroidJavaObject activityContext = null;
	private UnityCommands uc = null;
	private MeshRenderer cube;

    void Start() {
		Debug.Log("started Start...");
        if(toastExample == null) {
            using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            }

            using(AndroidJavaClass pluginClass = new AndroidJavaClass("com.github.listvin.testplugin.ToastExample")) {
                if(pluginClass != null) {
					Debug.Log("successfully got TestExample");
					toastExample = pluginClass.CallStatic<AndroidJavaObject>("instance");
                    toastExample.Call("setContext", activityContext);
                    activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                        toastExample.Call("showMessage", "This is a Toast message");
                    }));
                } else {
					Debug.Log("fucked while getting AndroidJavaClass");
				}
            }

			using(AndroidJavaClass pluginClass = new AndroidJavaClass("com.github.listvin.testplugin.TelegramExample")) {
				if (pluginClass != null) {
					Debug.Log("successfully got TelegramExample");
					pluginClass.CallStatic("main");
				}
			}

			using(AndroidJavaClass pluginClass = new AndroidJavaClass("com.github.listvin.testplugin.Server")) {
				uc = new UnityCommands();
				if (pluginClass != null) {
					Debug.Log("successfully launched Server");
					pluginClass.CallStatic("start", uc);
				}
			}
        }
		cube = GameObject.Find("Cube").GetComponent<MeshRenderer>();
    }

	void FixedUpdate(){
		if (uc.repaintCalled == 1) {
			cube.material.color = new Color(Mathf.Round(Random.value), Mathf.Round(Random.value), Mathf.Round(Random.value), 1F);
			Interlocked.Exchange(ref uc.repaintCalled, 0);
		}
	}
}
