using System;
using System.Collections.Generic;
using System.Text;

namespace Mousourouli.MDE.Recommendation
{
    public class TransactionItem
    {
        public TransactionItem(int item, bool positive)
        {
            _item = item;
            _positive = positive;
        }


        private int _item;
        public int Item
        {
            get { return _item; }
            set { _item = value; }
        }


       
        private bool _positive;
        public bool IsPositive
        {
            get { return _positive; }
            set { _positive = value; }
        }

        public int PositiveValue
        {
            get { return (_positive)?1:-1; }
            
        }

        public override string ToString()
        {
            return string.Format("{0}", PositiveValue * _item);
        }



    }
}
