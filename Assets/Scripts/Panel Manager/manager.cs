using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class manager : SingleTon<manager>
{
   public List<panelmodel> Panels;

    private Queue<PanelInstanceModel> _queue = new Queue<PanelInstanceModel>();
   public void ShowPanel(string panelId)
   {
       panelmodel panel = Panels.FirstOrDefault(panel => panel.panelId == panelId);

       if (panel != null)
       {
           var newInstancePanel = Instantiate(panel.panelPrefab, transform);

            _queue.Enqueue(new PanelInstanceModel
            {
                panelId = panelId,
                panelInstance = newInstancePanel
            });

       }
       else
       {
           Debug.LogError("Panel not found");
       }
   }
}
