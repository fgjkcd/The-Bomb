using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour {

	public enum Phase {
		Intro,
		Start,
		Game,
		End
	}

    public Phase phase { get { return currentPhase; } }
	private Phase currentPhase;

	protected delegate void OnEnterPhase();
	private Dictionary<Phase, OnEnterPhase> onEnter = new Dictionary<Phase, OnEnterPhase>();

	protected void registerPhaseEnter(Phase phase, OnEnterPhase pEvent) {
		onEnter.Add(phase, pEvent);
	}
	
	public void switchToPhase(Phase phase) {
		print("Switching to phase " + phase.ToString().ToUpper());
		currentPhase = phase;
		onEnter[phase].Invoke();
	}

}
