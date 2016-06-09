using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MenuLogic : MonoBehaviour {


    public bool isMainMenu = true;
    public bool isStageMenu = false;

    GameObject mainmenu;
    GameObject stageselect;

    // Use this for initialization
    void Start () {
        mainmenu = this.gameObject.transform.Find("MainMenu").gameObject;
        stageselect = this.gameObject.transform.Find("StageSelect").gameObject;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            

            if (isMainMenu)
            {
                if(mainmenu.activeSelf)
                    mainmenu.SetActive(false);
                else
                    mainmenu.SetActive(true);
            }

            if (isStageMenu)
            {
                isMainMenu = true;
                isStageMenu = false;
                if (stageselect.activeSelf)
                    stageselect.SetActive(false);
                // Debug.Log("ENTREI");
                /*
                 if (stageselect.activeSelf)
                     stageselect.SetActive(false);
                 else
                     stageselect.SetActive(true);
                     */
            }


        }

    }

    public void startGame()
    {

        SceneManager.LoadScene("Clone1");
    }

    public void goToStageSelect()
    {
        isMainMenu = false;
        mainmenu.SetActive(false);
        isStageMenu = true;
        stageselect.SetActive(true);

    }

    public void goToMainMenu()
    {
        isMainMenu = true;
        mainmenu.SetActive(true);
        isStageMenu = false;
        stageselect.SetActive(false);
    }
    public void restartStage()
    {
      
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadStage()
    {

       // SceneManager.LoadScene(name);
    }

    public void loadStage(string name)
    {

         SceneManager.LoadScene(name);
    }
    public void quitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
