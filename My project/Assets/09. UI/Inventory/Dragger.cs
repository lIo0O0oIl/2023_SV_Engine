using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragger : MouseManipulator
{
    private Action<MouseUpEvent, VisualElement, VisualElement> _dropCallback;

    private bool _isDrag = false;
    private Vector2 _startPos;
    private VisualElement _beforeSlot;

    public Dragger(Action<MouseUpEvent, VisualElement, VisualElement> DropCallback)
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        _dropCallback = DropCallback;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected void OnMouseDown(MouseDownEvent evt)
    {
        //Debug.Log("다운");
        if (CanStartManipulation(evt))
        {
            var x = target.layout.x;
            var y = target.layout.y;
            _beforeSlot = target.parent;
            var container = target.parent.parent;       // 백그라운드

            target.RemoveFromHierarchy();
            container.Add(target);

            _isDrag = true;
            target.CaptureMouse();
            _startPos = evt.localMousePosition;

            Vector2 offset = evt.mousePosition - container.worldBound.position - _startPos;

            target.style.position = Position.Absolute;
            target.style.left = offset.x;
            target.style.top = offset.y;
        }
    }

    protected void OnMouseMove(MouseMoveEvent evt)
    {
        //Debug.Log("드래그");
        if (!_isDrag || !CanStartManipulation(evt) || !target.HasMouseCapture())
        {
            return;
        }

        Vector2 diff = evt.localMousePosition - _startPos;
        var x = target.layout.x;
        var y = target.layout.y;

        target.style.left = x + diff.x;
        target.style.top = y + diff.y;
    }

    protected void OnMouseUp(MouseUpEvent evt)
    {
        //Debug.Log("업");
        if (!_isDrag || !target.HasMouseCapture())
            return;

        _isDrag = false;
        target.ReleaseMouse();

        // 되돌리거나 슬롯 이동
        target.style.position = Position.Relative;
        target.style.left = 0;
        target.style.top = 0;

        target.RemoveFromHierarchy();
        _beforeSlot.Add(target);
        //이녀석을 다시 relative바꿔주고 DropCallback을 콜해줘야 해.
    }
}
