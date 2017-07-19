using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoSingleton<GUIManager> {

    private List<BaseDialog> baseDialogs;
    public Transform transfDialog;
    void Awake()
    {
        this.baseDialogs = new List<BaseDialog>();
        GameStartDialog dialog = this.OnShowDialog<GameStartDialog>("Start");
    }
    public T OnShowDialog<T>(string path, object data = null) where T : BaseDialog
    {
        T target = ((GameObject)Instantiate(Resources.Load(path))).GetComponent<T>();
        if (target != null)
        {
            target.gameObject.SetActive(true);
            target.OnShow(this.transfDialog, data);
            this.baseDialogs.Add(target);
        }
        return null;
    }

	public NotifyDialog OnShowNotiFyDialog(string path, NotifyType type, object data = null)
	{
		NotifyDialog target = ((GameObject)Instantiate(Resources.Load(path))).GetComponent<NotifyDialog>();
		if (target != null)
		{
			target.gameObject.SetActive(true);
			target.OnShow(this.transfDialog, data, type);
			this.baseDialogs.Add(target);
		}
		return null;
	}

	public void OnHideDialog(BaseDialog dialog)
    {
        dialog.OnHide();
        if (this.baseDialogs.Contains(dialog))
        {
            this.baseDialogs.Remove(dialog);
        }
    }
    public void OnHideAllDialog()
    {
        foreach (BaseDialog dialog in this.baseDialogs)
        {
            if (dialog != null)
                dialog.OnHide();
        }
    }
}
