using System;
using System.Collections.Generic;
using System.Text;

namespace AppGlobal.Constants
{
    public enum StatusType
    {
        Deleted = -1,
        Active = 1,
    }

    public enum ChangeType
    {
        Created = 1,
        Replaced = 2,
        Deleted = 3
    }

    public enum RecurrenceType
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4
    }
}