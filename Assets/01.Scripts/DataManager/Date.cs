using System;

/// <summary>
/// Because DateTime cannot insert to Json file,  we replace it with this class.
/// When this class is generated without any parameter, its unit will have the generated time.
/// If it is created with 3 argument(year, month, day), it will inidicates the day.
/// </summary>
[Serializable]
public class Date
{
    private DateTime _day;

    public DateTime Day {
        get
        {
            if (_day == null)
            {
                _day = DateTime.Parse(dayString);
            }
            return _day;
        }
    }

    public string dayString;

    public Date()
    {
        _day = DateTime.Now;
        dayString = Day.ToString();
    }

    public Date(int year, int month, int day)
    {
        _day = new DateTime(year, month, day);
        dayString = Day.ToString();
    }
}
