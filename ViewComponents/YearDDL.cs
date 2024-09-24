using Microsoft.AspNetCore.Mvc;
using xPlugUniAdmissionManager.Assets.VComponents;

namespace xPlugUniAdmissionManager.ViewComponents;

public class YearDDL : ViewComponent
{
    public YearDDL()
    {

    }

    public async Task<IViewComponentResult> InvokeAsync(string id, int selectedId)
    {
        List<NameValueObject> items = await LoadItemsAsync();
        var retItem = new YearVCM { ItemList = items, Id = id, SelectedId = selectedId };
        return View(retItem);
    }

    private async Task<List<NameValueObject>> LoadItemsAsync()
    {
        try
        {

            List<NameValueObject> items = [];
            var year = DateTime.Now.Year;
            for (int i = year - 7; i < year + 4; i++)
            {
                items.Add(new NameValueObject { Id = i, Name = i.ToString() });
            }
            return await Task.FromResult(items);
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
            return [];
        }

    }
}
