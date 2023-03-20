using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popregun.DataBase
{
	public partial class Agent
	{
		public int CountSale => this.ProductSale.Where(x => (int)x.SaleDate.Year == DateTime.Now.Year).Count();
		public int Sale
		{
            get
            {
                var sumProd = this.ProductSale.Sum(x => x.Product.MinCostForAgent * x.ProductCount);

                if (sumProd <= 10000)
                {
                    return 0;
                }
                else if (sumProd > 10000 && sumProd <= 50000)
                {
                    return 5;
                }
                else if (sumProd > 50000 && sumProd <= 150000)
                {
                    return 10;
                }
                else if (sumProd > 150000 && sumProd <= 500000)
                {
                    return 20;
                }
                else
                {
                    return 25;
                }
            }
        }
        public string ColorAgent
        {
            get
            {
                if (this.Sale >= 25)
                    return "#90ee90";
                else
                    return "#F996E";
            }
        }
    }
}
