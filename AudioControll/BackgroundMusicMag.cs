using UnityEngine;
using System.Collections;

public class BackgroundMusicMag {

	#region 
	private AudioSource bgm;
	private static AudioSource audio;
	private static BackgroundMusicMag bgmObj;

	private static bool IsPlay;
	//静态函数中只能调用静态数据成员
	#endregion

	private BackgroundMusicMag(){}
	public static BackgroundMusicMag GetInstance() {
		if (bgmObj == null) {
			bgmObj = new BackgroundMusicMag();
			audio = GameObject.Find("musicFile").GetComponent<AudioSource>();
			IsPlay = true;
		}
		return bgmObj;
	}

	public void SetVol(float vol) {
		if(audio != null) {
			audio.volume = vol;
		}
	}
	public float GetVol() {
		return audio.volume;
	}
	public void SetPit(float pit) {
		if (audio != null) {
			audio.pitch = pit;
		}
	}
	public float GetPit() {
		return audio.pitch;
	}
	/* 改变音乐的播放状态 */
	public void ChangeAudioPlayStatus() {
		if (IsPlay) {
			audio.Pause();
			Debug.Log("Pause");
		} else {
			audio.UnPause();
			Debug.Log("UnPause");
		}
		IsPlay = !IsPlay;
	}
}