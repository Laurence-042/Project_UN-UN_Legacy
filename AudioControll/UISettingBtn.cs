using UnityEngine;
using System.Collections;

public class UISettingBtn : MonoBehaviour {

	private UISlider[] slider;
	private BackgroundMusicMag bgm;
	private UIToggle switchBtn;

	// Use this for initialization
	void Start () {
		UIButton[] btnArr = GetComponentsInChildren<UIButton>();
		UIEventListener.Get(btnArr[0].gameObject).onClick = CallBack1;

		slider = GetComponentsInChildren<UISlider>();

		bgm = BackgroundMusicMag.GetInstance();

		slider[0].value = bgm.GetVol();
		slider[1].value = bgm.GetPit();
		slider[0].onChange.Add(new EventDelegate(this, "CallBack2"));
		slider[1].onChange.Add(new EventDelegate(this, "CallBack3"));

		switchBtn = GetComponentInChildren<UIToggle>();
		UIEventListener.Get(switchBtn.gameObject).onClick = CallBack4;

	}

	void CallBack1(GameObject obj) {
		Debug.Log ("closing");
		UIWindowMag.GetInstance().CloseWindow("audioSettingwindow");
	}
	void CallBack2(GameObject obj) {
		bgm.SetVol(slider[0].value);
	}
	void CallBack3(GameObject obj) {
		bgm.SetPit(slider[1].value);
	}
	void CallBack4(GameObject obj) {
		bgm.ChangeAudioPlayStatus();
	}
}