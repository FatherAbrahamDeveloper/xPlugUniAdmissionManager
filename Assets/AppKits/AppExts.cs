using FluentValidation.Results;
using System.ComponentModel;
using System.Text;

namespace xPlugUniAdmissionManager.Assets.AppKits;

public static class AppExts
{
    public static string ToErrorListString(this List<ValidationFailure> errorList)
    {
        if (errorList == null) return "";
        var valErrors = new StringBuilder();
        foreach (var item in errorList)
        {
            valErrors.AppendLine(item.ToString());
        }
        return valErrors.ToString();
    }

    public static List<NameValueObject> ToDesList(this Type enumType)
    {
        if (enumType == null)
        {
            return null!;
        }

        var allValues = (int[])Enum.GetValues(enumType);
        var enumNames = Enum.GetNames(enumType);

        var myCollector = new List<NameValueObject>();

        try
        {
            for (var i = 0; i < allValues.GetLength(0); i++)
            {
                var myObj = new NameValueObject { Id = allValues[i] };
                var memInfo = enumType.GetMember(enumNames[i].ToString());
                if (memInfo != null && memInfo.Length > 0)
                {
                    var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs != null && attrs.Length > 0)
                    {
                        var thisItem = (DescriptionAttribute)attrs[0] ?? new DescriptionAttribute();
                        myObj.Name = thisItem.Description;
                    }
                }

                myCollector.Add(myObj);
            }
        }
        catch (Exception ex)
        {
            UtilTools.LogE(ex.StackTrace, ex.Source, ex.GetBaseException().Message);
            return null!;
        }

        return myCollector;
    }
}
