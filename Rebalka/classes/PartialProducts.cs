using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rebalka
{
    public partial class Product
    {
        public string HaveInSclad
        {
            get
            {
                if(ProductQuantityInStock > 0)
                {
                    return "Товар есть на складе";
                }
                return "Товар отсутствует";
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (ProductQuantityInStock > 0)
                {
                    return Brushes.White;
                }
                return Brushes.Gray;
            }
        }

        public string ImageAbsolute
        {
            get
            {
                if(ProductPhoto == null)
                {
                    return null;
                }
                else
                {
                    return Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + ProductPhoto;
                }
            }
        }
    }
}
