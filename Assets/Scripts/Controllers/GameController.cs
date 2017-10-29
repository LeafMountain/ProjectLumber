using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : ViewElement {

    private SelectionController selection;
    public SelectionController Selection { get{ return selection; }}

    private MouseController mouse;
    public MouseController Mouse { get{ return mouse; }}

    private void Awake(){
        selection = new SelectionController(App);
        mouse = new MouseController(App);
    }

    private void Update(){
        mouse.Update();
    }
}
