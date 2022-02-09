using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour {

    [SerializeField] private string _sceneToLoad;

    public void OnBackPressed() {
        SceneManager.LoadScene(_sceneToLoad);
    }
	 public void goToLevel_1() {
        SceneManager.LoadScene("Level1");
		PlayerPrefs.SetInt("currentFuel",10);
		PlayerPrefs.SetInt("Life",99);
		PlayerPrefs.SetInt("fueltokens",0);
		PlayerPrefs.SetInt("ammotokens",0);
		PlayerPrefs.SetInt("parttokens",0);
    }

}