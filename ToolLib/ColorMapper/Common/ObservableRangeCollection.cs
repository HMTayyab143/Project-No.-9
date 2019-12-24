/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：ColorMapper.Common
*   文件名称 ：ObservableRangeCollection.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：9/23/2019 5:25:34 PM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修改者   ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
namespace ColorMapper
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    /// <summary> 
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed. 
    /// </summary> 
    /// <typeparam name="T"></typeparam> 
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        public ObservableRangeCollection() : base() { }

        public ObservableRangeCollection(IEnumerable<T> collection) : base(collection) { }

        public ObservableRangeCollection<T> AddRange(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                    Items.Add(item);
                this.NotifyCollectionChanged();
            }
            return this;
        }

        public ObservableRangeCollection<T> RemoveRange(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                    Items.Remove(item);
                this.NotifyCollectionChanged();
            }
            return this;
        }

        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                Items.Clear();
                AddRange(collection);
            }
        }

        public void NotifyCollectionChanged()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
