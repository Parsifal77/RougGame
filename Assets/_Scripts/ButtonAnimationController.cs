using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAffectsBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Reference to the background object's Animator.
    // Assign this in the Inspector.
    public Animator backgroundAnimator;

    void Awake()
    {
        if (backgroundAnimator == null)
        {
            Debug.LogError("Background Animator is not assigned in ButtonAffectsBackground script on " + gameObject.name);
        }
    }

    // When the pointer hovers over the button,
    // set the background's "IsHover" parameter to true.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (backgroundAnimator != null)
        {
            Debug.Log("Button hovered: affecting background.");
            backgroundAnimator.SetBool("IsHover", true);
        }
    }

    // When the pointer exits the button,
    // set the background's "IsHover" parameter back to false.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (backgroundAnimator != null)
        {
            Debug.Log("Button hover exited: affecting background.");
            backgroundAnimator.SetBool("IsHover", false);
        }
    }

    // When the button is clicked,
    // trigger the "Pressed" action on the background's animator.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (backgroundAnimator != null)
        {
            Debug.Log("Button clicked: triggering background press animation.");
            backgroundAnimator.SetTrigger("Pressed");
        }
    }
}
