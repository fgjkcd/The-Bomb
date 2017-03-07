using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PhaseManager {

	public Phase initialPhase;
	public MovieManager movieController;

	public GameObject playerStartMarkers;

	public PlayerHub playerHub;
	public int minPlayersToStart = 4;

	public GameObject gameSystems;
    public GameObject trackingSystems;
    public GameObject lineHub, timer, centerObject, projectileHub, wandRandCollider;
    public Animator timerAnimator;


    private bool gameOver = false;
    private bool victory = true;

	// Use this for initialization
	void Start () {
		registerPhaseEnter(Phase.Intro, onEnterIntro);
		registerPhaseEnter(Phase.Game, onEnterGame);
		registerPhaseEnter(Phase.Start, onEnterStart);
		registerPhaseEnter(Phase.End, onEnterEnd);

		switchToPhase(initialPhase);
	}

// PHASE: INTRO

	private void onEnterIntro() {
        gameSystems.SetActive(false);
        trackingSystems.SetActive(true);
        trackingSystems.GetComponent<TCPListener>().enabled = true;
        trackingSystems.GetComponent<PlayerTrackingSystem>().enabled = true;
        trackingSystems.GetComponent<LineTrackingSystem>().enabled = false;
        playerStartMarkers.SetActive(true);
        timer.SetActive(false);
        centerObject.SetActive(false);
        playerHub.gameObject.SetActive(true);
        lineHub.SetActive(false);
        projectileHub.SetActive(false);
        wandRandCollider.SetActive(false);

        movieController.playAnimation("intro", onIntroAnimDone);
	}

	private void onIntroAnimDone(bool aborted) {
        print(playerHub.activePlayers());

        if (playerHub.activePlayers() < minPlayersToStart)
            // as long as there are not enough players active: repeat the intro video
            movieController.playAnimation("intro", onIntroAnimDone);
        else {
            playerStartMarkers.SetActive(false);
            switchToPhase(Phase.Start);
        }
    }

    // PHASE: START

    private void onEnterStart() {
        gameSystems.SetActive(false);
        trackingSystems.SetActive(false);
        trackingSystems.GetComponent<TCPListener>().enabled = false;
        trackingSystems.GetComponent<PlayerTrackingSystem>().enabled = false;
        trackingSystems.GetComponent<LineTrackingSystem>().enabled = false;
        playerStartMarkers.SetActive(false);
        timer.SetActive(false);
        centerObject.SetActive(false);
        playerHub.gameObject.SetActive(false);
        lineHub.SetActive(false);
        projectileHub.SetActive(false);
        wandRandCollider.SetActive(false);

        movieController.playAnimation("start", onStartAnimDone);
	}

	private void onStartAnimDone(bool aborted) {
		switchToPhase(Phase.Game);
	}

// PHASE: GAME

	private void onEnterGame() {
        gameSystems.SetActive(true);
        trackingSystems.SetActive(true);
        trackingSystems.GetComponent<TCPListener>().enabled = true;
        trackingSystems.GetComponent<PlayerTrackingSystem>().enabled = true;
        trackingSystems.GetComponent<LineTrackingSystem>().enabled = true;
        playerStartMarkers.SetActive(false);
        timer.SetActive(true);
        centerObject.SetActive(true);
        playerHub.gameObject.SetActive(true);
        lineHub.SetActive(true);
        projectileHub.SetActive(true);
        wandRandCollider.SetActive(true);

        timerAnimator.GetBehaviour<AS_Timer>().gm = this;

        //playerHub.setPlayersVisible(true); // DEBUG
        /*
        foreach (GameObject go in gameObjects) {
//			print("Enabling " + go.name);
			go.SetActive(true);
		}
        */
	}

    public void notifyGameOver(bool playerVictory) {
        if (gameOver) // ignore if already game over (prevent possible (or impossible) race condition)
            return;
        if (phase != Phase.Game)
            throw new InternalFuckupException();

        gameOver = true;
		gameSystems.SetActive(false);
        /*
        foreach (GameObject go in gameObjects)
            go.SetActive(false);
            */
        victory = playerVictory;
        switchToPhase(Phase.End);
	}

// PHASE: ENDWIN/ENDLOSE

	private void onEnterEnd() {
        gameSystems.SetActive(false);
        trackingSystems.SetActive(false);
        trackingSystems.GetComponent<TCPListener>().enabled = false;
        trackingSystems.GetComponent<PlayerTrackingSystem>().enabled = false;
        trackingSystems.GetComponent<LineTrackingSystem>().enabled = false;
        playerStartMarkers.SetActive(false);
        timer.SetActive(false);
        centerObject.SetActive(false);
        playerHub.gameObject.SetActive(false);
        lineHub.SetActive(false);
        projectileHub.SetActive(false);
        wandRandCollider.SetActive(false);

        movieController.playAnimation(victory ? "win" : "lose", onEndAnimDone);
	}

	private void onEndAnimDone(bool aborted) {
		int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.UnloadSceneAsync(sceneIndex);
		SceneManager.LoadScene(sceneIndex);
        // FIXME Other displays than primary don't work after reloading scene
		// see https://issuetracker.unity3d.com/issues/multi-display-camera-does-not-render-after-reloading-the-scene-using-scenemanager-dot-loadscene
    }

}
