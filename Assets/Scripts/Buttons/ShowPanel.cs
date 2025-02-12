using UnityEngine;

public class ShowPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string panelId;

    private manager _manager;

    public void Start()
    {
        _manager = manager.Instance;
    }
    public void DoShowPanel()
    {
       _manager.ShowPanel(panelId);
    }
}
