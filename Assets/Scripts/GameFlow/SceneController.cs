using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///////////////////////////////////SCENE CONTROLLER////////////////////////////////////////////////
/*

Script that controls scene transitions, reloads, and exit. 
YOU SHOULD NEED THIS SCRIPT AT LEAST ONCE IN EVERY SCENE ATTACHED TO SOMETHING HIGH IN THE HIERARCHY

*/

public class SceneController : MonoBehaviour
{
    public string NewGameScene;
    [SerializeField] private string nextScene;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if player object does not exist in scene, reload the scene
        //intended as preliminary respawn after death
        //if player falls off map or otherwise is rendered no longer visible on screen this will not trigger. player object needs to be destroy for this to trigger
        if(player == null && SceneManager.GetActiveScene().name != "MainMenu"){
            ReloadScene();
        }
        
    }

    private void Awake(){
       
    }

    //Allows for scene transitions in the main menu
    //intended for use on buttons only
    public void ButtonChangeScene(){
        SceneManager.LoadScene(NewGameScene);
    }

    //Allows game to exit. This ain't sword art online baby
    public void ExitGame(){
        //Application is ignored in unity editor. This only works in a build.
        Application.Quit();

        //Allows Exit in editor
        UnityEditor.EditorApplication.isPlaying = false;
    }

    //Method that reloads the scene
    public void ReloadScene(){
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    //Allows for scene transitions in game
    //once collision is detected with object with player tag a desired scene can be loaded
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player")){
            SceneManager.LoadScene(nextScene);
        }

    }
}
