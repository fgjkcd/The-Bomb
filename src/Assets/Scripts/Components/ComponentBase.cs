using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComponentBase : MonoBehaviour {

	public virtual void Start() {
		SystemBase.Register(this);
	}

	public virtual void OnDestroy() {
		SystemBase.Unregister(this);
	}

}
