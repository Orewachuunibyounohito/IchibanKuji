using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StubSlot
{
    private readonly int boardViewLayer = LayerMask.NameToLayer("BoardView");
    public Transform parent;
    public Vector3 Position{
        get => _position;
        set{
            _position = value;
            if(stubObject == null){ return ; }
            stubObject.transform.localPosition = value;
        }
    }
    public GameObject stubObject;

    private Vector3 _position;

    public StubSlot(Transform parent, Vector3 position){
        this.parent = parent;
        Position = position;
    }

    public void EnterTheSlot(GameObject stubObject){
        this.stubObject = stubObject;
        this.stubObject.transform.parent = parent;
        this.stubObject.transform.localPosition = _position;
        this.stubObject.layer = boardViewLayer;
        this.stubObject.transform.GetChild(0).gameObject.layer = boardViewLayer;
    }
}
