using System;
using System.Collections.Generic;

namespace SalesHub.Client.ViewModels
{
    public class SellingCompanyTreeViewItem
    {
        public string Text { get; set; }
        public bool Expanded { get; set; }
        public string Path { get; set; }

        public int? CustomerId { get; set; }

        public IList<SellingCompanyTreeViewItem> Items { get; set; }
    }
}