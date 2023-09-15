using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectUndo : MonoBehaviour
{
    public Stack<Vector2> originalSize = new Stack<Vector2>();
    public Stack<GameObject> undoRectangle = new Stack<GameObject>();

    public void OnClickUndoBtn()
    {
        if (undoRectangle != null && undoRectangle.Count != 0)
        {
            RectTransform rectTransform = undoRectangle.Peek().GetComponent<RectTransform>();
            undoRectangle.Pop();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = originalSize.Peek();  // Reset the rectangle's size
                originalSize.Pop();
            }
        }
    }

    public void ClearStack()
    {
        originalSize.Clear();
        undoRectangle.Clear();
    }
}
