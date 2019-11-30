using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObject : MonoBehaviour
{
    public Text ui_text;
    public GameObject selectedPanel;

    private int btnIndex;
    private string btnName;

    private IData data;

    private bool isBtnClicked = false;
    public bool IsBtnClicked
    {
        get { return isBtnClicked; }
        set
        {
            isBtnClicked = value;

            SetButtonSpriteOnClicked(isBtnClicked);
        }
    }

    public void Init(IData data)
    {
        this.data = data;

        btnIndex = data.GetIndex();
        btnName = data.GetName();

        ui_text.text = btnName;
    }

    public void OnButtonClicked()
    {
        UIManager.Instance.OnButtonsClicked(btnIndex);
    }

    public IData GetButtonData()
    {
        return data;
    }

    private void SetButtonSpriteOnClicked(bool isBtnClicked)
    {
        selectedPanel.SetActive(isBtnClicked);
    }

    public void OnDestroyCallBack()
    {
        Destroy(gameObject);
    }
}
