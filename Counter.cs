using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalogger
{
    public class Counter
    {
        public int value;
        public int tempValue;

        public Counter(int _value, int _tempValue)
        {
            value = _value;
            tempValue = _tempValue;
        }

        public Counter CounterIncreased(int newTempCounter)
        {
            Counter result = null;
            if (newTempCounter >= tempValue) result = new Counter(value + (newTempCounter - tempValue), newTempCounter);
            else result = new Counter(value + newTempCounter, newTempCounter);
            return result;
        }
    }
}
