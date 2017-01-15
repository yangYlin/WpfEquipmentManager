﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfEquipmentManager
{
    /// <summary>
    /// 租赁设备单项
    /// </summary>
    public class MyListItem:INotifyPropertyChanged
    {
        public int Remain { get; set; }

        public int Num
        {
            get
            {
                return num;
            }

            set
            {
                if (num == value)
                {
                    return;
                }
                num = value;
                Notify("Num");
            }
        }

        protected int num;
        public Equipment Equipment { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    /// <summary>
    /// 归还设备单项
    /// </summary>
    public class ReturnListItem : MyListItem
    {
        public long id { get; set; }
        private bool isReturn;
        public DateTime dateTime { get; set; }
        public int time { get; set; }
        public string name { get; set; }
        private double money;

        public double Money
        {
            get
            {
                return money;
            }

            set
            {
                if (money == value)
                {
                    return;
                }
                money = value;
                Notify("Money");
            }
        }

        public bool IsReturn
        {
            get
            {
                return isReturn;
            }

            set
            {
                if (isReturn == value)
                {
                    return;
                }
                isReturn = value;
                Notify("IsReturn");
            }
        }

        /// <summary>
        /// 计算钱数
        /// </summary>
        public double GetTotal()
        {
            long type =(long)Equipment.Type;
            long detail = (long)Equipment.Detail;
            double price = (double)Equipment.Price;
            int t = 0;
            if (detail == 0)
            {
                if (type == 0)
                {
                    t = Convert.ToInt32(Math.Ceiling(time / 60.0));
                }
                else
                {
                    t = Convert.ToInt32(Math.Ceiling(time / 30.0));
                }
            }
            else
            {
                t = time;
                if (type == 0)
                {
                    price /= 60.0;
                }
                else
                {
                    price /= 30.0;
                }
            }
            double total = price * t * Num;
            total = Math.Round(total, 2);
            return total;
        }
    }

    /// <summary>
    /// 计价规则转换
    /// </summary>
    public class PriceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string type_num = values[1].ToString();
            string detail_num = values[2].ToString();
            string type = type_num == "0" ? "小时" : "半小时";
            string detail = type_num == "0" ? "向上取整" : "精确到分钟";
            return values[0]+"元/"+type+"\n"+detail;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return ((string)value).Split(' ');
        }
    }
}
