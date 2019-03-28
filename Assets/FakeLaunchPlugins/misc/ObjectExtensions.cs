public static class ObjectExtensions
{
    public static bool tryGetIntValue(this object data, out int intValue)
    {
        if (data.GetType() == typeof(int))
        {
            intValue = (int)data;
            return true;
        }
        else if (data.GetType() == typeof(float))
        {
            intValue = (int)(float)data;
            return true;
        }
        intValue = 0;
        return false;
    }

    public static bool tryGetFloatValue(this object data, out float floatValue)
    {
        if (data.GetType() == typeof(int))
        {
            floatValue = (float)(int)data;
            return true;
        }
        else if (data.GetType() == typeof(float))
        {
            floatValue = (float)data;
            return true;
        }
        floatValue = 0;
        return false;
    }
}
