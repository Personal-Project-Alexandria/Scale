using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDialog : MonoBehaviour {

    public object data;
    public virtual void OnShow(Transform transf, object data)
    {
        this.transform.SetParent(transf);
        this.transform.localScale = Vector3.one;
        this.transform.localPosition = Vector3.zero;
        RectTransform rect = (RectTransform)this.transform;
        rect.sizeDelta = Vector2.zero;
        this.data = data;
        
    }
    public virtual void OnHide()
    {

        Destroy(gameObject);
        //this.gameObject.SetActive(false);
    }

    public void OnCloseDialog()
    {
        GUIManager.Instance.OnHideDialog(this);
    }
}
