using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
public class Selectable : MonoBehaviour {

	public UnityEvent selected;
	public UnityEvent deselected;

	private bool _selected;

	public Color notSelectedColor;
	public Color hoverColor;
	public Color selectedColor;

	private Color currentColor;

	[Range(0, 0.2f)]
	public float outlineWidth = 0.05f;

	[Range(0, 0.2f)]	
	public float outlineWidthHover = 0.1f;
	

	private Renderer renderer;
	private MaterialPropertyBlock _propBlock;

	void Start(){
		renderer = GetComponent<Renderer>();
		if(!renderer){
			renderer = GetComponentInChildren<Renderer>();

			if(!renderer){
				Debug.LogError("Missing outline shader");
				this.enabled = false;
			}
		}

		_propBlock = new MaterialPropertyBlock();
		renderer.GetPropertyBlock(_propBlock);		
	}

	public void Select(){
		currentColor = selectedColor;
		_selected = true;

		ChangeOutlineColor(currentColor);
		selected.Invoke();
	}

	public void DeSelect(){
		currentColor = notSelectedColor;
		_selected = false;

		ChangeOutlineColor(currentColor);
		deselected.Invoke();
	}

	public void OnRightClick(CommandModel command){
		ICommandListener[] commandListeners = GetComponents<ICommandListener>();

		GetComponent<UnitController>().NewCommand(command);

		// for (int i = 0; i < commandListeners.Length; i++)
		// {
		// 	commandListeners[i].CommandRequested(command);
		// }
	}

	public void OnShiftRightClick(CommandModel command) {
		GetComponent<UnitController>().AddCommand(command);		
	}

	private void ChangeOutlineColor(Color color){
		_propBlock.SetColor("_OutlineColor", color);
		renderer.SetPropertyBlock(_propBlock);		
	}

	private void ChangeOutlineWidth(float width){
		_propBlock.SetFloat("_OutlineWidth", width);
		renderer.SetPropertyBlock(_propBlock);
	}

	private void OnMouseEnter(){
		if(!_selected){
			ChangeOutlineColor(hoverColor);
		}
		ChangeOutlineWidth(outlineWidthHover);
	}

	private void OnMouseExit(){
		ChangeOutlineColor(currentColor);
		ChangeOutlineWidth(outlineWidth);		
	}
}