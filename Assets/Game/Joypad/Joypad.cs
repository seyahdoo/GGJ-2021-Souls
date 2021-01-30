﻿using UnityEngine;
using UnityEngine.EventSystems;

public class Joypad : MonoBehaviour {
    public RectTransform joyPad;
    public float touchHorizontalDeadzonePercentage = 0f;
    public float touchVerticalDeadzonePercentage = 0f;
    public float joypadOverallDeadzone = .1f;
    public float sensitivity = .05f;
    public bool normalizeInput = true;
    public Animator joyPadAnimator;
    public string animatorHorizontalParamName = "blendX";
    public string animatorVerticalParamName = "blendY";
    public Vector2 input;
    private const int NON_POINTER = -2973642;
    private int _startPointerId;
    private Vector2 _startPoint;
    public void Init() {
        _startPointerId = NON_POINTER;
        joyPad.gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData) {
        if (_startPointerId == NON_POINTER) {
            joyPad.gameObject.SetActive(true);
            _startPointerId = eventData.pointerId;
            _startPoint = eventData.position;
            joyPad.position = _startPoint;
            UpdateJoypadGraphic(Vector2.zero);
        }
    }
    public void OnDrag(PointerEventData eventData) {
        if (_startPointerId == eventData.pointerId) {
            var diff = eventData.position - _startPoint;
            if (Mathf.Abs(diff.x) / Screen.width < touchHorizontalDeadzonePercentage) diff.x = 0;
            if (Mathf.Abs(diff.y) / Screen.height < touchVerticalDeadzonePercentage) diff.y = 0;
            diff /= Screen.width * sensitivity;
            if (normalizeInput) diff = diff.normalized;
            input = Vector2.ClampMagnitude(diff, 1f);
            input = input.magnitude > joypadOverallDeadzone ? input : Vector2.zero;
            UpdateJoypadGraphic(input);
        }
    }
    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.pointerId == _startPointerId) {
            joyPad.gameObject.SetActive(false);
            _startPointerId = NON_POINTER;
            input = Vector2.zero;
        }
    }
    private void UpdateJoypadGraphic(Vector2 position) {
        joyPadAnimator.SetFloat(animatorHorizontalParamName, position.x);
        joyPadAnimator.SetFloat(animatorVerticalParamName, position.y);
    }
}