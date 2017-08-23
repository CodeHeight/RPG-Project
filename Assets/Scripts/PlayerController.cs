using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    public LayerMask movementMask;
    public Interactable focus;

    Camera cam;
    PlayerMotor motor;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                // Move our player to what we hit
                motor.MoveToPoint(hit.point);
                // Stop focusing any objects.
                RemoveFocus();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                // check if we hit an interactible.
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                // if we hit interactible, set as focus
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDeFocused();

            focus = newFocus;
            motor.FollowTarget(newFocus);
        }        
        newFocus.OnFocused(transform);        
    }

    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDeFocused();

        focus = null;
        motor.StopFollowingTarget();
    }
}
