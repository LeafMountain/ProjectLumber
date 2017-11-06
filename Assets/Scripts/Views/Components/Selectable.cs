using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
public class Selectable : MonoBehaviour {

	private bool _selected;
	public Shader outlineShader;
	private Shader originalShader;

	public Color notSelectedColor;
	public Color hoverColor;
	public Color selectedColor;

	private Color currentColor;

	[Range(0, 0.2f)]
	public float outlineWidth = 0.05f;

	[Range(0, 0.2f)]	
	public float outlineWidthHover = 0.1f;
	

	private Renderer _renderer;
	private MaterialPropertyBlock _propBlock;

	void Start(){
		_renderer = GetComponent<Renderer>();
		if(!_renderer){
			_renderer = GetComponentInChildren<Renderer>();

			if(!_renderer){
				Debug.LogError("Missing outline shader");
				this.enabled = false;
			}
		} else {
			originalShader = _renderer.material.shader;
			_renderer.material.shader = outlineShader;
		}

		_propBlock = new MaterialPropertyBlock();
		_renderer.GetPropertyBlock(_propBlock);	
	}

	public void Select(){
		currentColor = selectedColor;
		_selected = true;

		ChangeOutlineColor(currentColor);
	}

	public void DeSelect(){
		currentColor = notSelectedColor;
		_selected = false;

		ChangeOutlineColor(currentColor);
	}

	public void OnRightClick(CommandModel command){
		ICommandListener[] commandListeners = GetComponents<ICommandListener>();

		if(GetComponent<UnitController>()){
			GetComponent<UnitController>().NewCommand(command);
		}
	}

	public void OnShiftRightClick(CommandModel command) {
		GetComponent<UnitController>().AddCommand(command);		
	}

	private void ChangeOutlineColor(Color color){
		_propBlock.SetColor("_OutlineColor", color);
		_renderer.SetPropertyBlock(_propBlock);		
	}

	private void ChangeOutlineWidth(float width){
		_propBlock.SetFloat("_OutlineWidth", width);
		_renderer.SetPropertyBlock(_propBlock);
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