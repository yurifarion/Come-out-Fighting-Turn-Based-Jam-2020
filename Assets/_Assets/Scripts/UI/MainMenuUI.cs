using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    public void OnStartGamePressed() {
        SceneManager.LoadScene("Level1");
		
		PlayerPrefs.SetInt("currentFuel",10);
		PlayerPrefs.SetInt("Life",99);
		PlayerPrefs.SetInt("fueltokens",0);
		PlayerPrefs.SetInt("ammotokens",0);
		PlayerPrefs.SetInt("parttokens",0);
    }

    public void OnCreditsPressed() {
        SceneManager.LoadScene("credits_scene");
    }

    public void OnQuitPressed() {
        Application.Quit();
    }

}