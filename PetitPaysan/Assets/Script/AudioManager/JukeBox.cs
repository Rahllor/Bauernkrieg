using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour {

	public Sound[] sounds;
	public Sound[] transitions;
	private bool[] played;
	private System.Random rnd;
    public static JukeBox instance;
	private AudioSource currentMusic;
	private bool isPlayingMusic;
	
	void Awake () {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		foreach(Sound s in sounds){
			AudioManager.instance.AddSound(s);
		}
		foreach(Sound s in transitions){
			AudioManager.instance.AddSound(s);
		}
		rnd = new System.Random();
		isPlayingMusic = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void StartPlaying(){
		played = new bool[sounds.Length];
		for(int i = 0; i < sounds.Length; ++i){
			played[i] = false;
		}
		ChangeMusic();
	}
	
	private void ChangeMusic(){
		int nMusicNotPlayed = 0;
		foreach(bool b in played){
			if(!b){
				++nMusicNotPlayed;
			}
		}
		if(nMusicNotPlayed == 0 && played.Length > 0){
			for(int k = 0; k < played.Length; ++k){
				played[k] = false;
			}
			ChangeMusic();
			return;
		}
		int randomValue = rnd.Next(nMusicNotPlayed);
		int i = 0;
		for(int j = 0; j < played.Length; ++j){
			if(!played[j]){
				if(i == randomValue){
					AudioManager.instance.Play(sounds[j].name);
					currentMusic = sounds[j].source;
					played[j] = true;
					StartCoroutine(waitEndOfMusic());
					break;
				}
				++i;
			}
		}
	}
	
	public void NextMusic(){
		if(isPlayingMusic){
			StopCoroutine(waitEndOfMusic());
			AudioManager.instance.StopAll();
			ChangeMusic();
		}
	}
	
	private void Transition(){
		int randomValue = rnd.Next(transitions.Length * 2);
		if(randomValue < transitions.Length){
			AudioManager.instance.Play(transitions[randomValue].name);
			currentMusic = transitions[randomValue].source;
			StartCoroutine(waitEndOfTransition());
		}else{
			ChangeMusic();
		}
	}
	
	IEnumerator waitEndOfMusic(){
		isPlayingMusic = true;
		yield return new WaitForSeconds(currentMusic.clip.length);
		Transition();
		isPlayingMusic = false;
	}
	
	IEnumerator waitEndOfTransition(){
		yield return new WaitForSeconds(currentMusic.clip.length);
		ChangeMusic();
	}
	
}
